 dotnet build -c Release
 dotnet publish -c Release --framework netcoreapp3.0
 # publish
 copy .\sounds\*.* .\bin\Release\netcoreapp3.0\publish\sounds
 copy .\db\*.* .\bin\Release\netcoreapp3.0\publish\db
 copy .\config\*.* .\bin\Release\netcoreapp3.0\publish\config
 # release
 copy .\sounds\*.* .\bin\Release\sounds
 copy .\db\*.* .\bin\Release\db
 copy .\config\*.* .\bin\Release\config


 rm EveAlert.zip
"c:\program files\7-zip\7z.exe" a -r \EveAlert.zip .\bin\Release\netcoreapp3.0\publish
# git commit -m "updated"
# git push origin master
dotnet run .\bin\Release\netcoreapp3.0\EveIntelReader.dll

