using DevicesManager;
using DevicesManager.Logic;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IDeviceService<Device>>(service =>
{
    var connectionString = builder.Configuration.GetConnectionString("MyDatabase");
    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException("Connection string 'UniversityDatabase' is not configured.");
    }
    return new DeviceService<Device>(connectionString);
});

builder.Services.AddScoped(typeof(DeviceService<>));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();