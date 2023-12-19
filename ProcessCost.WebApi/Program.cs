using ProcessCost.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(x => { x.EnableAnnotations(); })
    .RegisterServices()
    .RegisterMediatr();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app
        .UseSwagger()
        .UseSwaggerUI();
}

app.UseHttpsRedirection();
app.SetupStagesApiV1();

app.Run();

public partial class Program
{
}