pushd "%~dp0"
del /F /Q ".\bin\Release\*.nupkg"
dotnet build -c Release ObjectDump.csproj
cd ".\bin\Release"
nuget push *.nupkg -Source D:\cache\NuGetPackages
pause
nuget push *.nupkg -Source https://nuget.toycloud.com/v3/index.json -SkipDuplicate