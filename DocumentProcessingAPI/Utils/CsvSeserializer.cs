using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace DocumentProcessingAPI.Utils;

public class CsvSerializer : ISerializer
{
    private readonly TypeConverterOptions _typeConverterOptions;
    private readonly CsvConfiguration _csvConfiguration;

    public CsvSerializer(CsvConfiguration csvConfiguration, TypeConverterOptions typeConverterOptions)
    {
        _csvConfiguration = csvConfiguration;
        _typeConverterOptions = typeConverterOptions;
    }

    public bool Deserialize<T>(string text, ref T? output)
    {
        using (StringReader reader = new(text))
        using (CsvReader csvReader = new(reader, _csvConfiguration))
        {
            csvReader.Context.TypeConverterOptionsCache.AddOptions<DateTime>(_typeConverterOptions);
            csvReader.Read();

            try
            {
                output = csvReader.GetRecord<T>();
            }
            catch (Exception)
            {
                return false;
            }

            if (output == null)
            {
                return false;
            }
        }

        return true;
    }

    public bool Serialize<T>(T input, ref string? text)
    {
        throw new NotImplementedException();
    }
}
