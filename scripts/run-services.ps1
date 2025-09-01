# Run All Microservices Script
# This script runs all microservices with proper process management

Write-Host "🚀 Starting All Microservices..." -ForegroundColor Green

# First, stop any existing services
Write-Host "🛑 Stopping existing services..." -ForegroundColor Yellow
& "$PSScriptRoot\stop-services.ps1"

# Wait a moment for processes to fully stop
Start-Sleep -Seconds 2

# Build the solution
Write-Host "🔨 Building solution..." -ForegroundColor Yellow
dotnet build MicroservicesArchitecture.sln
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Build failed!" -ForegroundColor Red
    exit 1
}

Write-Host "✅ Build successful!" -ForegroundColor Green

# Configuration for services
$Services = @(
    @{ Name = "DepartmentService"; Path = "src\Services\DepartmentService\API"; Port = "5299" },
    @{ Name = "EmployeeService"; Path = "src\Services\EmployeeService\API"; Port = "5148" },
    @{ Name = "ProjectService"; Path = "src\Services\ProjectService\API"; Port = "5147" }
)

# Start each service
$jobs = @()
foreach ($service in $Services) {
    Write-Host "Starting $($service.Name) on port $($service.Port)..." -ForegroundColor Cyan
    
    $job = Start-Job -ScriptBlock {
        param($ServicePath, $ServiceName, $Port)
        Set-Location $ServicePath
        Write-Host "$ServiceName starting on port $Port..."
        dotnet run --urls "http://localhost:$Port"
    } -ArgumentList $service.Path, $service.Name, $service.Port
    
    $jobs += $job
    
    # Wait a moment for the service to start
    Start-Sleep -Seconds 3
}

Write-Host "`n🎉 All microservices are starting..." -ForegroundColor Green
Write-Host "=================================" -ForegroundColor Green
Write-Host "Department Service: http://localhost:5299" -ForegroundColor Cyan
Write-Host "Employee Service:   http://localhost:5148" -ForegroundColor Cyan
Write-Host "Project Service:    http://localhost:5147" -ForegroundColor Cyan
Write-Host "=================================" -ForegroundColor Green

Write-Host "`n📋 Available endpoints:" -ForegroundColor Yellow
Write-Host "• Department Service: http://localhost:5299/api/departments" -ForegroundColor White
Write-Host "• Employee Service:   http://localhost:5148/api/employees" -ForegroundColor White
Write-Host "• Project Service:    http://localhost:5147/api/projects" -ForegroundColor White

Write-Host "`n💡 Use the Postman collection in docs/Microservices_Postman_Collection.json for testing" -ForegroundColor Yellow
Write-Host "Press Ctrl+C to stop all services" -ForegroundColor Red

try {
    # Wait for user to stop
    while ($true) {
        Start-Sleep -Seconds 1
    }
}
catch {
    Write-Host "`n🛑 Stopping all services..." -ForegroundColor Yellow
    $jobs | Stop-Job -PassThru | Remove-Job
    Write-Host "✅ All services stopped" -ForegroundColor Green
} 