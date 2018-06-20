cd ..\..\StateOfWarUtility\
make build
cd ..\StateOfWarMapEditor\Assets\
xcopy /F /Y ..\..\StateOfWarUtility\Library\bin\StateOfWarUtility.dll .\Libs\StateOfWarUtility.dll
