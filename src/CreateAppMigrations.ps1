# How to use
# .\CreateMigration.ps1 -MigrationName "MigrationName"

param(
    [Parameter(Mandatory=$true)]
    [string]$MigrationName
)

function Pause
{
    # Prompt the user to press any key before exiting
    Write-Host ""
    Write-Host "Press any key to continue..."
    $null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown')
}

dotnet build --no-restore

try {
    dotnet ef --project DataModel\DataModel.csproj -s KristenCookiesMvc\KristenCookiesMvc.csproj migrations add $MigrationName --context ApplicationDbContext -o Migrations\Application


    Write-Host "Migration task complete!" -ForegroundColor Green
} catch {
    Write-Host "An error occurred while generating the migrations: $_" -ForegroundColor Red
}

Pause