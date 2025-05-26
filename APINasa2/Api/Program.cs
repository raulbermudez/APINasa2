using APINasa2.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Registrar HttpClient y servicio
builder.Services.AddHttpClient<AsteroidsService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();