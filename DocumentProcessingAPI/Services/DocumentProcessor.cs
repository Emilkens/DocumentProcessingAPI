using DocumentProcessingAPI.Models;
using DocumentProcessingAPI.Utils;

namespace DocumentProcessingAPI.Services;

/// <summary>
/// Provides .PUR file processing, binding it's properties to appropriate models
/// </summary>
public class DocumentProcessor : IDocumentProcessor
{
    private readonly ISerializer _serializer;

    private List<Document> _documents = new List<Document>();
    private Document? _currentDocument = null;
    private string _message = string.Empty;
    private int _processedLineNumber = 0;
    private long _charCount = 0;

    public DocumentProcessor(ISerializer serializer)
    {
        _serializer = serializer;
    }

    public async Task<ProcessingResult> Process(Stream fileStream, int x)
    {
        if (fileStream == null)
        {
            throw new ArgumentNullException();
        }

        using StreamReader reader = new(fileStream);
        string line;

        while ((line = await reader.ReadLineAsync()) != null)
        {
            if (line.Length < 2)
            {
                return Fail();
            }

            string? rowType = line.Split(',', 2).FirstOrDefault();
            if (rowType == null || rowType.Length > 1)
            {
                return Fail();
            }

            line = line.Substring(2);
            switch (rowType)
            {
                case "B":
                    if (!ProcessItem(line))
                    {
                        return Fail();
                    }
                    // Additional 2 represents row type and comma
                    _charCount += line.Length + 2;
                    break;

                case "H":
                    if (!ProcessHeader(line))
                    {
                        return Fail();
                    }
                    _charCount += line.Length + 2;
                    break;

                case "C":
                    if (!ProcessComment(line))
                    {
                        return Fail();
                    }
                    _charCount += line.Length + 2;
                    break;

                default:
                    _message = "Document contains unknown row type identifier.";
                    break;
            }

            _processedLineNumber++;
        }

        return Success(x);
    }

    private bool ProcessHeader(string line)
    {
        if (_currentDocument != null && _currentDocument?.Items.Count == 0)
        {
            _message = "Invalid document format - Document contains no items.";
            return false;
        }

        _currentDocument = new Document();
        DocumentHeader? header = null;

        bool result = _serializer.Deserialize<DocumentHeader>(line, ref header);
        if (!result)
        {
            _message = "Invalid document header format.";
            return false;
        }

        _currentDocument.Header = header;
        _currentDocument.Items = new List<DocumentItem>();
        _documents.Add(_currentDocument);
        return true;
    }

    private bool ProcessItem(string line)
    {
        if (_currentDocument == null || _currentDocument?.Header == null)
        {
            _message = "Invalid document format - Document contains no header.";
            return false;
        }

        DocumentItem? item = null;

        bool result = _serializer.Deserialize<DocumentItem>(line, ref item);
        if (!result)
        {
            _message = "Invalid document item format.";
            return false;
        }

        _currentDocument.Items.Add(item);
        return true;
    }

    private bool ProcessComment(string line)
    {
        if (_currentDocument == null || _currentDocument?.Header == null)
        {
            _message = "Invalid document format - Document contains no header.";
            return false;
        }

        if (!string.IsNullOrEmpty(_currentDocument.Comment))
        {
            _message = "Invalid document format - Document contains multiple comments.";
            return false;
        }

        if (_currentDocument.Items.Count > 0)
        {
            _message = "Invalid document format - Comment row is not adjacent to the header.";
            return false;
        }

        _currentDocument.Comment = line;
        return true;
    }

    private ProcessingResult Fail()
    {
        return new ProcessingResult()
        {
            Message = $"Parsing failed on line: {_processedLineNumber}. Reason: {_message}",
            Data = null,
            Success = false
        };
    }

    private ProcessingResult Success(int x)
    {
        List<string> maxValueProducts = _documents
                    .SelectMany(doc => doc.Items)
                    .GroupBy(item => item.NetValue)
                    .OrderByDescending(group => group.Key)
                    .FirstOrDefault()
                    ?.Select(item => item.Name)
                    .ToList();

        string productsWithMaxNetValue = string.Empty;
        if (maxValueProducts != null && maxValueProducts.Count > 0)
        {
            productsWithMaxNetValue = string.Join(",", maxValueProducts);
        }

        return new ProcessingResult()
        {
            Message = string.Empty,
            Data = new ProcessedFileData()
            {
                Documents = _documents,
                LineCount = _processedLineNumber,
                CharCount = _charCount,
                Sum = _documents.Sum(doc => doc.Header.GrossValue),
                XCount = _documents.Where(doc => doc.Items.Count > x).Count(),
                ProductsWithMaxNetValue = productsWithMaxNetValue
            },
            Success = true
        };
    }
}
