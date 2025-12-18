var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel to listen on all interfaces (both IPv4 and IPv6) for Tailscale access
builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(System.Net.IPAddress.Any, 5207); // IPv4 - 0.0.0.0
    options.Listen(System.Net.IPAddress.IPv6Any, 5207); // IPv6 - [::]
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS to allow cross-host access via Tailscale
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowTailscale", policy =>
    {
        policy.AllowAnyOrigin()  // Allow requests from any Tailscale machine
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Remove HTTPS redirection since we're only using HTTP for Tailscale
// app.UseHttpsRedirection();

// Enable CORS for Tailscale cross-host access
app.UseCors("AllowTailscale");

app.UseAuthorization();

app.MapControllers();

// Log the URLs the app is listening on
var logger = app.Services.GetRequiredService<ILoggerFactory>().CreateLogger("Startup");
logger.LogInformation("Application starting. Listening on URLs: {Urls}", string.Join(", ", app.Urls));
Console.WriteLine($"🚀 Application is listening on: {string.Join(", ", app.Urls)}");

// Get and display network interfaces for Tailscale access
try
{
    var hostName = System.Net.Dns.GetHostName();
    var addresses = System.Net.Dns.GetHostAddresses(hostName);
    Console.WriteLine($"\n📡 Network interfaces on '{hostName}':");
    foreach (var addr in addresses)
    {
        if (addr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork || 
            addr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
        {
            Console.WriteLine($"   → http://{addr}:5207/api/allocation");
        }
    }
    
    // Try to get Tailscale domain name
    string? tailscaleDomain = null;
    try
    {
        // Check if we can resolve the Tailscale domain (common pattern)
        var tailscaleHosts = System.Net.Dns.GetHostEntry(hostName);
        foreach (var alias in tailscaleHosts.Aliases)
        {
            if (alias.Contains(".ts.net"))
            {
                tailscaleDomain = alias;
                break;
            }
        }
    }
    catch { }
    
    Console.WriteLine($"\n✅ API Access URLs:");
    Console.WriteLine($"   Short hostname: http://{hostName}:5207/api/allocation");
    if (!string.IsNullOrEmpty(tailscaleDomain))
    {
        Console.WriteLine($"   Tailscale domain: http://{tailscaleDomain}:5207/api/allocation ⭐ (RECOMMENDED)");
    }
    else
    {
        Console.WriteLine($"   ⚠️  Tailscale domain not detected. Use: http://{hostName}.tailb43185.ts.net:5207/api/allocation");
        Console.WriteLine($"   💡 Tip: Short hostname may not work on all machines. Use the full Tailscale domain for reliability.");
    }
    Console.WriteLine($"   Or use the Tailscale IP address shown above.\n");
}
catch (Exception ex)
{
    Console.WriteLine($"⚠️  Could not enumerate network addresses: {ex.Message}");
}

app.Run();
