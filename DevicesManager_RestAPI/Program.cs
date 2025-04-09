using DevicesManager;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<DeviceManager>();
builder.Services.AddSingleton<IDeviceParser, DeviceParser>();
builder.Services.AddSingleton<IDeviceFileHandler, DeviceFileHandler>();

var app = builder.Build();

app.MapGet("/devices", (DeviceManager manager) =>
{
    var devices = manager.GetDevices()
        .Select(d => new { d.Id, d.Name, d.IsEnabled });
    return Results.Ok(devices);
});

app.MapGet("/devices/{id}", (DeviceManager manager, string id) =>
{
    var device = manager.GetDeviceById(id);
    return device != null ? Results.Ok(device) : Results.NotFound();
});

app.MapPost("/devices", (DeviceManager manager, Device device) =>
{
    manager.AddDevice(device);
    return Results.Created($"/devices/{device.Id}", device);
});

app.MapPut("/devices/{id}", (DeviceManager manager, string id, Device updatedDevice) =>
{
    var existingDevice = manager.GetDeviceById(id);
    if (existingDevice == null) return Results.NotFound();

    manager.EditDevice(updatedDevice);
    return Results.NoContent();
});

app.MapDelete("/devices/{id}", (DeviceManager manager, string id) =>
{
    var device = manager.GetDeviceById(id);
    if (device == null) return Results.NotFound();

    manager.RemoveDeviceById(id);
    return Results.NoContent();
});

app.Run();