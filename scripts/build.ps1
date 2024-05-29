dotnet build -c Release Community.PowerToys.Run.Plugin.LeagueOfLegends.csproj

$leagueOfLegendsDir = "bin/Release/LeagueOfLegends"
if (Test-Path -Path $leagueOfLegendsDir) {
    Remove-Item -Recurse -Force $leagueOfLegendsDir
}
New-Item -ItemType Directory -Path bin/Release/LeagueOfLegends

Copy-Item -Recurse -Path bin/Release/net8.0-windows/Images -Destination bin/Release/LeagueOfLegends/Images
Copy-Item -Path bin/Release/net8.0-windows/Community.PowerToys.Run.Plugin.LeagueOfLegends.dll -Destination bin/Release/LeagueOfLegends/Community.PowerToys.Run.Plugin.LeagueOfLegends.dll
Copy-Item -Path bin/Release/net8.0-windows/plugin.json -Destination bin/Release/LeagueOfLegends/plugin.json
Copy-Item -Path bin/Release/net8.0-windows/champion.json -Destination bin/Release/LeagueOfLegends/champion.json