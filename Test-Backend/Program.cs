using MySql.Data.MySqlClient;
using Test_Backend.Services;
using Test_Backend.Interfaces;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Services
builder.Services.AddSingleton<INameService, NameService>();
builder.Services.AddSingleton<ICprService, CprService>();
builder.Services.AddSingleton<IPhoneNumberService, PhoneNumberService>();
builder.Services.AddTransient<IAddressService, AddressService>();
builder.Services.AddSingleton<IPersonService, PersonService>();
builder.Services.AddTransient<MySqlConnection>(_ => new MySqlConnection(connectionString));

// Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS for frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable CORS
app.UseCors("AllowFrontend");

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.Run();