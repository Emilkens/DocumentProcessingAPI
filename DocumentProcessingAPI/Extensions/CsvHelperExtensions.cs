using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using DocumentProcessingAPI.Utils;
using System.Globalization;

namespace DocumentProcessingAPI.Extensions;

public static class CsvHelperExtensions
{
    public static IServiceCollection UseCsvSeserializer(this IServiceCollection services)
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

        return services
            .AddSingleton<TypeConverterOptions>(typeConverterOptions)
            .AddSingleton<CsvConfiguration>(csvConfiguration)
            .AddTransient<ISerializer, CsvSerializer>();
    }
}

