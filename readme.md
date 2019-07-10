Eveonline console app which reads logs, this is started because other systems are not open sourced.
Foxjazz will guide and manage updates to this project


To Install and configurer.

First download/install [code](https://code.visualstudio.com/docs?dv=win&wt.mc_id=DX_841432&sku=codewin)

Add terminal in code or open a console to (clone)[https://help.github.com/en/articles/cloning-a-repository] the project.

This may mean installing yet another tool called git along with gitbash.

(Download)[https://dotnet.microsoft.com/download/dotnet-core/3.0] dotnet core 3.0 or 2.2 if you don't want 3

Config:
Open config.txt and place the eve logs Folder in there:
chatFolder, "C:\Users\YourUserName\Documents\EVE\logs"
Chat logs are auto searched for currently. If no in correct folder, please use the config.

Now open code, then open the folder the project is cloned at.
From command line type  dotnet build
you will see the output built and folder.
Then type "dotnet run" "The folderapp.dll" 
i.e. dotnet run G:\EveAlerts\EveIntelReader\bin\Debug\netcoreapp3.0\EveIntelReader.dll


Contributing:
currently this is configed for 75C position. And this could use some database work to add more systems and longer alert range. I think 9 jumps would be sufficient to cover, but for me 6 is perfect.
Update the database, create a Pull request from your own repo. I will verify the database.

Consider suggesting features.







