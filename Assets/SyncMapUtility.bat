cd ..\..\StateOfWarUtility\
make build
cd ..\StateOfWarMapEditor\Assets\
xcopy /Y ..\..\StateOfWarUtility\Library\bin\StateOfWarUtility.dll .\Library\StateOfWarUtility.dll
