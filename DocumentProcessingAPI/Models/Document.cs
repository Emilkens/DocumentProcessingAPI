namespace DocumentProcessingAPI.Models;

public class Document
{
    public DocumentHeader Header { get; set; }
    public List<DocumentItem> Items { get; set; }
    public string? Comment { get; set; }
}
