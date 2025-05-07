using DevicesManager;
using DevicesManager.Logic;

var builder = WebApplication.CreateBuilder(args);

// to register IDeviceRepository<Device>
builder.Services.AddSingleton<IDeviceRepository<Device>>(service =>
{
    var connectionString = builder.Configuration.GetConnectionString("MyDatabase");
    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException("Connection string 'MyDatabase' is not configured.");
    }

    return new DeviceRepository<Device>(connectionString);
});

// to register IDeviceService<Device>
builder.Services.AddSingleton<IDeviceService<Device>>(service =>
{
    var repository = service.GetRequiredService<IDeviceRepository<Device>>();
    return new DeviceService<Device>(repository);
});

// to register open generics (for flexibility, if needed)
builder.Services.AddScoped(typeof(IDeviceRepository<>), typeof(DeviceRepository<>));
builder.Services.AddScoped(typeof(IDeviceService<>), typeof(DeviceService<>));

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