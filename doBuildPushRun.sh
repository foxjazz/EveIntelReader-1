 dotnet build -c Release
 dotnet publish -c Release --framework netcoreapp3.0
 rm EveAlert.zip
"c:\program files\7-zip\7z.exe" a -r \EveAlert.zip .\bin\Release\netcoreapp3.0\publish
