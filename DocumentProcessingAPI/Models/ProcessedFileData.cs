namespace DocumentProcessingAPI.Models;

public class ProcessedFileData
{
    public List<Document> Documents { get; set; }
    public int LineCount { get; set; }
    public long CharCount { get; set; }
    public decimal Sum { get; set; }
    public int XCount { get; set; }
    public string ProductsWithMaxNetValue { get; set; }
}
