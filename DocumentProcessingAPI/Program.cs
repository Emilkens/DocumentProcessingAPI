using DocumentProcessingAPI.Extensions;
using DocumentProcessingAPI.Models;
using DocumentProcessingAPI.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
});

builder.Services.AddBasicAuth();
builder.Services.AddTransient<IDocumentProcessor, DocumentProcessor>();
builder.Services.UseCsvSeserializer();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHsts();
app.UseHttpsRedirection();

app.UseBasicAuth();

app.MapPost("/api/test", async (int x, HttpContext context, IDocumentProcessor documentProcessor) =>
{
    if (!context.Request.Headers.Keys.Contains("Content-Type"))
    {
        return Results.BadRequest("Content-Type header not provided.");
    }

    if (!context.Request.Headers["Content-Type"].ToString().StartsWith("multipart/form-data"))
    {
        return Results.BadRequest("Wrong Content-Type header value." +
            " Provide file as form-data and make sure Content-Type header is set to multipart/form-data.");
    }

    if (context.Request.Form.Files.Count == 0)
    {
        return Results.BadRequest("No form data provided.");
    }

    try
    {
        Stream fileStream = context.Request.Form.Files[0].OpenReadStream();
        ProcessingResult processedData = await documentProcessor.Process(fileStream, x);

        if (processedData.Success)
        {
            return Results.Ok(processedData.Data);
        }

        return Results.BadRequest(processedData.Message);
    }
    catch (Exception)
    {
        return Results.Problem("Request processing failed - Exception occured.");
    }
});

app.Run();
