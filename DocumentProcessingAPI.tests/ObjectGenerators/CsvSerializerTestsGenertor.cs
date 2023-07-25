using DocumentProcessingAPI.Models;

namespace DocumentProcessingAPI.tests.ObjectGenerators;

internal class CsvSerializerTestsGenertor
{
    public DocumentHeader DocumentHeaderObject { get; set; }
    public DocumentItem DocumentItemObject { get; set; }

    public CsvSerializerTestsGenertor()
    {
        DocumentHeaderObject = new DocumentHeader()
        {
            BACode = "5308",
            Type = "02",
            Number = "00130",
            OperationDate = DateTime.Parse("29-01-2015"),
            DocumentDayNumber = "5222",
            ContractorCode = "10140",
            ContractorName = "KOL S.A.",
            ExternalDocumentNumber = "20150128099911",
            ExternalDocumentDate = DateTime.Parse("28-01-2015"),
            NetValue = -34.37m,
            Vat = -2.75m,
            GrossValue = -37.12m,
            F1 = 0,
            F2 = 0,
            F3 = 0
        };

        DocumentItemObject = new DocumentItem()
        {
            Code = "19556",
            Name = "NASZ DZIENNIK",
            Amount = -3.000,
            NetPrice = 1.63000m,
            NetValue = -4.89m,
            Vat = -0.39m,
            AmountBefore = 5.000,
            AvgBefore = 1.73552,
            AmountAfter = 2.000,
            AvgAfter = 1.89379,
            Group = 1117,
        };

    }
}
