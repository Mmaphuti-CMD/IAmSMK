# API Testing Guide

## Application URL
- **Base URL**: `http://localhost:5207`
- **Name Endpoint**: `http://localhost:5207/Name`
- **Swagger UI**: `http://localhost:5207/swagger`

---

## Talend API Tester / Postman Configuration

### Endpoint Details:
- **Method**: `POST`
- **URL**: `http://localhost:5207/Name`
- **Content-Type**: `application/json`

### Request Body (Success Case):
```json
{
  "Input": "What is my name"
}
```

### Request Body (Failure Case):
```json
{
  "Input": "Anything else"
}
```

### Expected Responses:
- **Success (200 OK)**: `"I am SMK!"`
- **Bad Request (400)**: `"Hahaha!!!"`

---

## CMD / PowerShell Commands

### Success Case (Returns "I am SMK!"):
```cmd
curl -X POST http://localhost:5207/Name -H "Content-Type: application/json" -d "{\"Input\": \"What is my name\"}"
```

### Failure Case (Returns "Hahaha!!!"):
```cmd
curl -X POST http://localhost:5207/Name -H "Content-Type: application/json" -d "{\"Input\": \"Test\"}"
```

### PowerShell (Success Case):
```powershell
Invoke-RestMethod -Uri "http://localhost:5207/Name" -Method POST -ContentType "application/json" -Body '{"Input": "What is my name"}'
```

### PowerShell (Failure Case):
```powershell
Invoke-RestMethod -Uri "http://localhost:5207/Name" -Method POST -ContentType "application/json" -Body '{"Input": "Test"}'
```

### PowerShell (with Error Handling):
```powershell
try {
    $body = @{ Input = "What is my name" } | ConvertTo-Json
    Invoke-RestMethod -Uri "http://localhost:5207/Name" -Method POST -ContentType "application/json" -Body $body
} catch {
    Write-Output "Error: $($_.Exception.Message)"
    Write-Output "Response: $($_.Exception.Response)"
}
```






