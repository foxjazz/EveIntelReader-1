 dotnet build -c Release
 dotnet publish -c Release --framework netcoreapp3.1
 # publish
 cp ./sounds/*.wav ./bin/Release/netcoreapp3.1/publish/sounds
 cp ./db/*.csv ./bin/Release/netcoreapp3.1/publish/db
 cp ./config/*.txt ./bin/Release/netcoreapp3.1/publish/config
 # release
 cp ./sounds/*.wav ./bin/Release/netcoreapp3.1/sounds
 cp ./db/*.csv ./bin/Release/netcoreapp3.1/db
 cp ./config/*.txt ./bin/Release/netcoreapp3.1/config


 rm d:\EveAlerts\EveAlert.zip
7z a -tZip d:\EveAlerts\EveAlert.zip "D:\EveAlerts\EveIntelReaderPublished\*"
cp "D:\EveAlerts\EveIntelReader\bin\Release\netcoreapp3.1\EveI*.dll" "C:\EVE\EveAlerts\"

# git commit -m "updated"
# git push origin master
# dotnet run .\bin\Release\netcoreapp3.0\EveIntelReader.dll
