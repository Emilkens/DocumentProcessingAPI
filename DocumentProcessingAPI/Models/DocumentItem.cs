namespace DocumentProcessingAPI.Models;

public class DocumentItem
{
    public string Code { get; set; }
    public string Name { get; set; }
    public double Amount { get; set; }
    public decimal NetPrice { get; set; }
    public decimal NetValue { get; set; }
    public decimal Vat { get; set; }
    public double AmountBefore { get; set; }
    public double AvgBefore { get; set; }
    public double AmountAfter { get; set; }
    public double AvgAfter { get; set; }
    public uint Group { get; set; }
}
