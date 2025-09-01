# Quick Fix for File Lock Issues
# Run this script whenever you get file lock errors

Write-Host "🔧 Quick Fix for File Lock Issues" -ForegroundColor Yellow
Write-Host "=================================" -ForegroundColor Yellow

# Kill all possible processes that might be locking files
$processesToKill = @(
    "DepartmentService.API",
    "EmployeeService.API", 
    "ProjectService.API",
    "dotnet"
)

foreach ($process in $processesToKill) {
    try {
        $runningProcesses = Get-Process -Name $process -ErrorAction SilentlyContinue
        if ($runningProcesses) {
            Write-Host "🛑 Stopping $process processes..." -ForegroundColor Cyan
            Stop-Process -Name $process -Force -ErrorAction SilentlyContinue
            Write-Host "✅ $process stopped" -ForegroundColor Green
        }
    }
    catch {
        Write-Host "ℹ️  No $process processes found" -ForegroundColor Gray
    }
}

# Wait for processes to fully terminate
Start-Sleep -Seconds 2

Write-Host "`n🎉 File lock issues resolved!" -ForegroundColor Green
Write-Host "You can now build and run your services." -ForegroundColor Yellow
Write-Host "`n💡 To run all services: .\scripts\run-services.ps1" -ForegroundColor Cyan 