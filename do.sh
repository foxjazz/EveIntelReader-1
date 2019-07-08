 dotnet build -c Release
 dotnet publish -c Release --framework netcoreapp3.0
 # publish
 cp ./sounds/*.wav ./bin/Release/netcoreapp3.0/publish/sounds
 cp ./db/*.csv ./bin/Release/netcoreapp3.0/publish/db
 cp ./config/*.txt ./bin/Release/netcoreapp3.0/publish/config
 # release
 cp ./sounds/*.wav ./bin/Release/netcoreapp3.0/sounds
 cp ./db/*.csv ./bin/Release/netcoreapp3.0/db
 cp ./config/*.txt ./bin/Release/netcoreapp3.0/config


 rm EveAlert.zip
7z a -tZip \EveAlert.zip "G:\EveAlerts\EveIntelReader\bin\Release\netcoreapp3.0\publish"
# git commit -m "updated"
# git push origin master
dotnet run .\bin\Release\netcoreapp3.0\EveIntelReader.dll
