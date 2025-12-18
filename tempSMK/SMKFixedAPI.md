# SMK Fixed API Documentation

## Problem Summary

The ASP.NET Core API was only accessible locally on `localhost` (127.0.0.1) and could not be reached from external devices via Tailscale network, even though:
- Ping to the Tailscale IP (100.86.132.97) was working
- Port 5207 firewall rules were configured
- The application appeared to be running correctly

**Error Messages:**
- `curl: (7) Failed to connect to summer01.tailb43185.ts.net port 5207`
- `curl: (7) Failed to connect to 100.86.132.97 port 5207`

## Root Cause

The application was **only binding to localhost (127.0.0.1)** instead of all network interfaces (0.0.0.0). This meant:
- ✅ The API worked locally on the server
- ❌ External devices (PC, Talend, etc.) could not connect via Tailscale

### Initial Attempts and Issues

1. **First Attempt**: Using `app.Urls.Add("http://0.0.0.0:5207")` after `app.Build()`
   - **Problem**: This was being overridden by `launchSettings.json` configuration

2. **Second Attempt**: Using `builder.WebHost.UseUrls("http://0.0.0.0:5207")`
   - **Problem**: Still conflicted with launch settings

3. **Third Attempt**: Using `builder.WebHost.ConfigureKestrel(options => { options.ListenAnyIP(5207); })`
   - **Problem**: The console showed a warning:
     ```
     Microsoft.AspNetCore.Server.Kestrel: Warning: Overriding address(es) 'http://0.0.0.0:5207'. 
     Binding to endpoints defined via IConfiguration and/or UseKestrel() instead.
     Microsoft.Hosting.Lifetime: Information: Now listening on: http://[::]:5207
     ```
   - **Issue**: `ListenAnyIP()` was defaulting to **IPv6 only** (`[::]:5207`), not IPv4
   - **Result**: The app was listening on IPv6, but Tailscale and most clients expect IPv4

## Solution

### Step 1: Explicit IPv4 and IPv6 Binding

Configured Kestrel to **explicitly listen on both IPv4 and IPv6** interfaces:

```csharp
builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(System.Net.IPAddress.Any, 5207);      // IPv4 - 0.0.0.0
    options.Listen(System.Net.IPAddress.IPv6Any, 5207);   // IPv6 - [::]
});
```

**Why this works:**
- `IPAddress.Any` (0.0.0.0) binds to all IPv4 interfaces, including Tailscale
- `IPAddress.IPv6Any` ([::]) binds to all IPv6 interfaces
- Explicit configuration prevents Kestrel from defaulting to IPv6-only

### Step 2: Remove Conflicting Configuration

Removed `applicationUrl` settings from `launchSettings.json` that were overriding the Kestrel configuration:

**Before:**
```json
"http": {
  "commandName": "Project",
  "applicationUrl": "http://0.0.0.0:5207",
  ...
}
```

**After:**
```json
"http": {
  "commandName": "Project",
  // applicationUrl removed - Kestrel configuration takes precedence
  ...
}
```

## Final Configuration

### Program.cs

```csharp
var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel to listen on all interfaces (both IPv4 and IPv6) for Tailscale access
builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(System.Net.IPAddress.Any, 5207);      // IPv4 - 0.0.0.0
    options.Listen(System.Net.IPAddress.IPv6Any, 5207);   // IPv6 - [::]
});

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();
```

### Verification

After the fix, verify the binding with:

```cmd
netstat -ano | findstr :5207 | findstr LISTENING
```

**Expected Output:**
```
TCP    0.0.0.0:5207           0.0.0.0:0              LISTENING       [PID]
TCP    [::]:5207              [::]:0                 LISTENING       [PID]
```

Both IPv4 (`0.0.0.0:5207`) and IPv6 (`[::]:5207`) should be in LISTENING state.

## Results

The API is now accessible from:

1. **Locally on summer01:**
   ```
   http://localhost:5207
   http://127.0.0.1:5207
   ```

2. **Via Tailscale IP:**
   ```
   http://100.86.132.97:5207
   ```

3. **Via Tailscale hostname:**
   ```
   http://summer01.tailb43185.ts.net:5207
   ```

## API Endpoints

### GET /Name
Returns a test message to verify connectivity.

**Request:**
```bash
curl http://summer01.tailb43185.ts.net:5207/Name
```

**Response:**
```json
"API is working! Connected successfully."
```

### POST /Name
Processes a name request.

**Request:**
```bash
curl -X POST http://summer01.tailb43185.ts.net:5207/Name \
  -H "Content-Type: application/json" \
  -d "{\"Input\":\"What is my name\"}"
```

**Response (Success):**
```json
"I am SMK!"
```

**Response (Error):**
```json
"Hahaha!!!"
```

## Testing

### Local Test (on summer01)
```cmd
curl http://localhost:5207/Name
```

### Tailscale IP Test
```cmd
curl http://100.86.132.97:5207/Name
```

### Tailscale Hostname Test (from any device on Tailscale network)
```cmd
curl http://summer01.tailb43185.ts.net:5207/Name
```

## Key Learnings

1. **`ListenAnyIP()` can default to IPv6 only** - Always explicitly configure both IPv4 and IPv6 if you need both
2. **`launchSettings.json` can override Kestrel configuration** - Remove conflicting `applicationUrl` settings
3. **Always verify with `netstat`** - Check that the app is actually listening on `0.0.0.0`, not just `127.0.0.1`
4. **Console warnings are important** - The Kestrel warning about overriding addresses was the key clue

## Troubleshooting

If the API still doesn't work:

1. **Check Windows Firewall:**
   ```cmd
   netsh advfirewall firewall add rule name="Allow Port 5207" dir=in action=allow protocol=TCP localport=5207
   ```

2. **Verify Tailscale is running:**
   ```cmd
   tailscale status
   tailscale ip -4
   ```

3. **Check if port is listening:**
   ```cmd
   netstat -ano | findstr :5207 | findstr LISTENING
   ```

4. **Test local connectivity first:**
   ```cmd
   curl http://localhost:5207/Name
   ```

## Conclusion

The API is now fully accessible from any device on the Tailscale network, including:
- Talend
- Postman
- curl
- Any HTTP client that can reach summer01 via Tailscale

The fix ensures the application binds to all network interfaces (both IPv4 and IPv6), making it accessible via Tailscale while maintaining localhost access for development.
