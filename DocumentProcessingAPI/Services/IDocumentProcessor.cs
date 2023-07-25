using DocumentProcessingAPI.Models;

namespace DocumentProcessingAPI.Services;

public interface IDocumentProcessor
{
    Task<ProcessingResult> Process(Stream fileStream, int x);
}
