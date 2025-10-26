using MySql.Data.MySqlClient;
using Test_Backend.Services;
using Test_Backend.Interfaces;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
builder.Services.AddSingleton<INameService, NameService>();
builder.Services.AddSingleton<ICprService, CprService>();
builder.Services.AddSingleton<IPhoneNumberService, PhoneNumberService>();
builder.Services.AddTransient<IAddressService, AddressService>();
builder.Services.AddSingleton<IPersonService, PersonService>();
builder.Services.AddTransient<MySqlConnection>(_ => new MySqlConnection(connectionString));


builder.Services.AddControllers();
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

app.UseAuthorization();

app.MapControllers();

app.Run();
