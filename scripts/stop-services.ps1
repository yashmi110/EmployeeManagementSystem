# Stop All Microservices Script
# This script stops all running microservices to prevent file lock issues

Write-Host "🛑 Stopping All Microservices..." -ForegroundColor Yellow

# List of service names to stop
$Services = @(
    "DepartmentService.API",
    "EmployeeService.API", 
    "ProjectService.API"
)

# Stop each service
foreach ($service in $Services) {
    $processes = Get-Process -Name $service -ErrorAction SilentlyContinue
    if ($processes) {
        Write-Host "Stopping $service..." -ForegroundColor Cyan
        Stop-Process -Name $service -Force -ErrorAction SilentlyContinue
        Write-Host "✅ $service stopped" -ForegroundColor Green
    } else {
        Write-Host "ℹ️  $service is not running" -ForegroundColor Gray
    }
}

# Also stop any dotnet processes that might be running the services
$dotnetProcesses = Get-Process -Name "dotnet" -ErrorAction SilentlyContinue
if ($dotnetProcesses) {
    Write-Host "Stopping dotnet processes..." -ForegroundColor Cyan
    Stop-Process -Name "dotnet" -Force -ErrorAction SilentlyContinue
    Write-Host "✅ dotnet processes stopped" -ForegroundColor Green
}

Write-Host "🎉 All services stopped successfully!" -ForegroundColor Green
Write-Host "You can now build and run services without file lock issues." -ForegroundColor Yellow 