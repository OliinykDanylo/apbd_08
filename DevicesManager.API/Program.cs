using DevicesManager;
using DevicesManager.API;

var builder = WebApplication.CreateBuilder(args);

// Register services in the container
builder.Services.AddSingleton<IDeviceManager, DeviceManager>();
builder.Services.AddSingleton<IDeviceParser, DeviceParser>();
builder.Services.AddSingleton<IDeviceFileHandler, DeviceFileHandler>();
builder.Services.AddControllers(); // Add controllers for the API

// Add Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Device routes
app.MapGet("/devices", (IDeviceManager manager) =>
{
    var devices = manager.GetDevices();
    var result = devices.Select(d => new DeviceAPI
    {
        Id = d.Id,
        Name = d.Name,
        IsEnabled = d.IsEnabled,
        Type = d switch
        {
            Smartwatch => "Smartwatch",
            PersonalComputer => "PC",
            EmbeddedDevice => "EmbeddedDevice",
            _ => "Unknown"
        }
    });
    return Results.Ok(result);
});

app.MapPost("/devices", (IDeviceManager manager, CreateDevice dto) =>
{
    Device newDevice = dto.Type switch
    {
        "Smartwatch" => new Smartwatch(dto.Id, dto.Name, dto.IsEnabled, dto.BatteryLevel ?? 100),
        "PC" => new PersonalComputer(dto.Id, dto.Name, dto.IsEnabled, dto.OperatingSystem),
        "EmbeddedDevice" => new EmbeddedDevice(dto.Id, dto.Name, dto.IsEnabled, dto.IpAddress!, dto.NetworkName!),
        _ => throw new ArgumentException("Invalid device type")
    };

    manager.AddDevice(newDevice);
    return Results.Created($"/devices/{dto.Id}", null);
});

app.MapPut("/devices/{id}", (string id, IDeviceManager manager, UpdateDevice dto) =>
{
    var existing = manager.GetDeviceById(id);
    if (existing is null) return Results.NotFound();

    Device updated = dto.Type switch
    {
        "Smartwatch" => new Smartwatch(dto.Id, dto.Name, dto.IsEnabled, dto.BatteryLevel ?? 100),
        "PC" => new PersonalComputer(dto.Id, dto.Name, dto.IsEnabled, dto.OperatingSystem),
        "EmbeddedDevice" => new EmbeddedDevice(dto.Id, dto.Name, dto.IsEnabled, dto.IpAddress!, dto.NetworkName!),
        _ => throw new ArgumentException("Invalid device type")
    };

    manager.EditDevice(updated);
    return Results.NoContent();
});

app.MapDelete("/devices/{id}", (string id, IDeviceManager manager) =>
{
    manager.RemoveDeviceById(id);
    return Results.NoContent();
});

app.Run();