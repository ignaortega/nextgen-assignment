using Mapster;

using PaySpace.Calculator.API.Models;
using PaySpace.Calculator.Data;
using PaySpace.Calculator.Data.Models;
using PaySpace.Calculator.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddMapster();

//// Configure Mapster
//TypeAdapterConfig<PostalCodeDto, PostalCode>.NewConfig()
//    .Ignore(dest => dest.Id) // Assuming Id is auto-generated
//    .Map(dest => dest.Calculator, src => Enum.Parse<CalculatorType>(src.Calculator));

//TypeAdapterConfig<PostalCode, PostalCodeDto>.NewConfig()
//    .Map(dest => dest.Calculator, src => src.Calculator.ToString());


builder.Services.AddCalculatorServices();
builder.Services.AddDataServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();
app.InitializeDatabase();

app.Run();