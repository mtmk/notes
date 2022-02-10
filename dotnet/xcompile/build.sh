rm -rf dist
dotnet publish -p:PublishSingleFile=true -p:DebugType=embedded -c release --sc -r win-x64 -o dist
