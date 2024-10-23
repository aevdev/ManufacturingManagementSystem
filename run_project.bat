@echo off
echo Migrating DB...
dotnet ef database update --project ManufacturingManagementSystem\
echo Running unit tests...
dotnet test
IF %ERRORLEVEL% NEQ 0 (
    echo Unit tests failed. Exiting.
    exit /b %ERRORLEVEL%
)
echo Starting application...
dotnet run --project ManufacturingManagementSystem\
pause
