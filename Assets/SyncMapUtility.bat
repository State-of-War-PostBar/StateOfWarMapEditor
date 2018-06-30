cd ..\..\StateOfWarUtility\
dotnet build .\Library\x.csproj
cd ..\StateOfWarMapEditor\Assets\
xcopy /F /Y ..\..\StateOfWarUtility\Library\bin\StateOfWarUtility.dll .\Libs\StateOfWarUtility.dll
