using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using DocumentProcessingAPI.Models;
using DocumentProcessingAPI.tests.ObjectGenerators;
using DocumentProcessingAPI.Utils;
using FluentAssertions;
using System.Globalization;
using System.Reflection;

namespace DocumentProcessingAPI.tests;

internal class CsvSerializerTests
{
    private static CsvSerializer _serializer;
    private static Assembly _assembly;
    private static CsvSerializerTestsGenertor _objectGenerator;

    [SetUp]
    public static void Setup()
    {
        TypeConverterOptions typeConverterOptions = new()
        {
            DateTimeStyle = DateTimeStyles.None,
            Formats = new[] { "dd-MM-yyyy" }
        };

        CsvConfiguration csvConfiguration = new(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = false
        };

        _serializer = new CsvSerializer(csvConfiguration, typeConverterOptions);

        _assembly = Assembly.GetExecutingAssembly();

        _objectGenerator = new CsvSerializerTestsGenertor();
    }

    [Test]
    public static void Deserialize_ProperHeader()
    {
        DocumentHeader header = new();
        bool result = false;

        using (Stream stream = _assembly.GetManifestResourceStream("DocumentProcessingAPI.tests.TestData.ProperDocumentHeader.txt"))
        {
            if (stream == null)
            {
                Assert.Fail($"Document not found.");
            }

            using (StreamReader reader = new StreamReader(stream))
            {
                string line = reader.ReadLine();
                line = line.Substring(2);

                result = _serializer.Deserialize<DocumentHeader>(line, ref header);
            }
        }

        Assert.True(result);
        header.Should().BeEquivalentTo(_objectGenerator.DocumentHeaderObject);
    }

    [Test]
    public static void Deserialize_ProperItem()
    {
        DocumentItem item = new();
        bool result = false;

        using (Stream stream = _assembly.GetManifestResourceStream("DocumentProcessingAPI.tests.TestData.ProperDocumentItem.txt"))
        {
            if (stream == null)
            {
                Assert.Fail($"Document not found.");
            }

            using (StreamReader reader = new StreamReader(stream))
            {
                string line = reader.ReadLine();
                line = line.Substring(2);

                result = _serializer.Deserialize<DocumentItem>(line, ref item);
            }
        }

        Assert.True(result);
        item.Should().BeEquivalentTo(_objectGenerator.DocumentItemObject);
    }

    [Test]
    public static void Deserialize_InvalidHeader()
    {
        DocumentHeader header = new();
        bool result = false;

        using (Stream stream = _assembly.GetManifestResourceStream("DocumentProcessingAPI.tests.TestData.InvalidDocumentHeader.txt"))
        {
            if (stream == null)
            {
                Assert.Fail($"Document not found.");
            }

            using (StreamReader reader = new StreamReader(stream))
            {
                string line = reader.ReadLine();
                line = line.Substring(2);

                _serializer.Deserialize<DocumentHeader>(line, ref header);
            }
        }

        Assert.False(result);
    }

    [Test]
    public static void Deserialize_InvalidItem()
    {
        DocumentItem item = new();
        bool result = false;

        using (Stream stream = _assembly.GetManifestResourceStream("DocumentProcessingAPI.tests.TestData.InvalidDocumentItem.txt"))
        {
            if (stream == null)
            {
                Assert.Fail($"Document not found.");
            }

            using (StreamReader reader = new StreamReader(stream))
            {
                string line = reader.ReadLine();
                line = line.Substring(2);

                _serializer.Deserialize<DocumentItem>(line, ref item);
            }
        }

        Assert.False(result);
    }
}
