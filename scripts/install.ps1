Invoke-WebRequest -Uri "https://github.com/vigovlugt/PowerToysRunLeagueOfLegends/releases/latest/download/LeagueOfLegends.zip" -OutFile "$env:LocalAppData\Microsoft\PowerToys\PowerToys Run\Plugins\LeagueOfLegends.zip"
Add-Type -AssemblyName System.IO.Compression.FileSystem
Remove-Item -Recurse -Force -Path "$env:LocalAppData\\Microsoft\PowerToys\PowerToys Run\Plugins\LeagueOfLegends" -ErrorAction SilentlyContinue
[System.IO.Compression.ZipFile]::ExtractToDirectory("$env:LocalAppData\Microsoft\PowerToys\PowerToys Run\Plugins\LeagueOfLegends.zip", "$env:LocalAppData\Microsoft\PowerToys\PowerToys Run\Plugins\")
Remove-Item -Path "$env:LocalAppData\\Microsoft\PowerToys\PowerToys Run\Plugins\LeagueOfLegends.zip"
Write-Host "League of Legends plugin installed. Please restart PowerToys."