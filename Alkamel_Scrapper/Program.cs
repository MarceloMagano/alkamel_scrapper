using Alkamel_Scrapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// GET WEC  with url parameter year
app.MapGet("/wec", async (HttpContext context, int year, string eventNumber) =>
{
    var urls = await WEC.ScrapeData(year, eventNumber);
    await WEC.DownloadFiles(urls);
    await context.Response.WriteAsJsonAsync(urls);
});

// GET IMSA
app.MapGet("/imsa", async (HttpContext context) =>
{
    await context.Response.WriteAsJsonAsync("Hello IMSA!");
});

app.Run();