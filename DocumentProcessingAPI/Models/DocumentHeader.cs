namespace DocumentProcessingAPI.Models;

public class DocumentHeader
{
    public string BACode { get; set; }
    public string Type { get; set; }
    public string Number { get; set; }
    public DateTime OperationDate { get; set; }
    public string? DocumentDayNumber { get; set; }
    public string? ContractorCode { get; set; }
    public string? ContractorName { get; set; }
    public string? ExternalDocumentNumber { get; set; }
    public DateTime ExternalDocumentDate { get; set; }
    public decimal NetValue { get; set; }
    public decimal Vat { get; set; }
    public decimal GrossValue { get; set; }
    public double F1 { get; set; }
    public double F2 { get; set; }
    public double F3 { get; set; }
}
