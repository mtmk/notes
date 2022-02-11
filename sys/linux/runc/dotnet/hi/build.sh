rm -rf bin obj dist
# https://dotnetcoretutorials.com/2021/11/10/single-file-apps-in-net-6/
# https://docs.microsoft.com/en-gb/dotnet/core/deploying/#publish-with-readytorun-images
dotnet publish -c release --sc -p:PublishTrimmed=true -p:EnableCompressionInSingleFile=true -p:PublishReadyToRun=true -p:PublishSingleFile=true -p:DebugType=embedded -r linux-x64  -o dist

