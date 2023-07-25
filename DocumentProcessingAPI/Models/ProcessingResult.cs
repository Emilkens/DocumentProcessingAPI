namespace DocumentProcessingAPI.Models;

public class ProcessingResult
{
    public bool Success { get; set; }
    public ProcessedFileData? Data { get; set; }
    public string? Message { get; set; }

}
