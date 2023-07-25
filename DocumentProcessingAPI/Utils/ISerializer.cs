namespace DocumentProcessingAPI.Utils;

public interface ISerializer
{
    bool Serialize<T>(T input, ref string? text);
    bool Deserialize<T>(string text, ref T? output);
}
