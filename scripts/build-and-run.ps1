# Microservices Build and Run Script
# This script builds and runs all microservices in the refactored architecture

param(
    [switch]$BuildOnly,
    [switch]$RunOnly,
    [switch]$Clean
)

Write-Host "🚀 Microservices Architecture - Build and Run Script" -ForegroundColor Green
Write-Host "==================================================" -ForegroundColor Green

# Configuration
$Services = @(
    @{ Name = "DepartmentService"; Path = "src\Services\DepartmentService\API"; Port = "5299" },
    @{ Name = "EmployeeService"; Path = "src\Services\EmployeeService\API"; Port = "5148" },
    @{ Name = "ProjectService"; Path = "src\Services\ProjectService\API"; Port = "5147" }
)

# Function to check if a port is in use
function Test-Port {
    param([string]$Port)
    $connection = Get-NetTCPConnection -LocalPort $Port -ErrorAction SilentlyContinue
    return $connection -ne $null
}

# Function to kill processes on specific ports
function Stop-ServiceOnPort {
    param([string]$Port)
    $processes = Get-NetTCPConnection -LocalPort $Port -ErrorAction SilentlyContinue | 
                 ForEach-Object { Get-Process -Id $_.OwningProcess -ErrorAction SilentlyContinue }
    foreach ($process in $processes) {
        Write-Host "Stopping process $($process.ProcessName) (PID: $($process.Id)) on port $Port" -ForegroundColor Yellow
        Stop-Process -Id $process.Id -Force -ErrorAction SilentlyContinue
    }
}

# Clean option
if ($Clean) {
    Write-Host "🧹 Cleaning solution..." -ForegroundColor Yellow
    dotnet clean
    Write-Host "✅ Clean completed" -ForegroundColor Green
    exit 0
}

# Build all services
if (-not $RunOnly) {
    Write-Host "🔨 Building all microservices..." -ForegroundColor Yellow
    
    # Build main solution
    $buildResult = dotnet build MicroservicesArchitecture.sln
    if ($LASTEXITCODE -ne 0) {
        Write-Host "❌ Build failed!" -ForegroundColor Red
        exit 1
    }
    
    Write-Host "✅ Build completed successfully" -ForegroundColor Green
}

# Run services
if (-not $BuildOnly) {
    Write-Host "🚀 Starting microservices..." -ForegroundColor Yellow
    
    # Stop any existing services on the ports
    foreach ($service in $Services) {
        if (Test-Port $service.Port) {
            Write-Host "⚠️  Port $($service.Port) is in use. Stopping existing process..." -ForegroundColor Yellow
            Stop-ServiceOnPort $service.Port
        }
    }
    
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
}

Write-Host "`n✨ Script completed!" -ForegroundColor Green 