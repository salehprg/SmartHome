@REM git pull

@REM git add -A
@REM git commit -m "Auto publish"

@REM git pull
@REM git push

dotnet publish -r linux-arm

copy .\ClientApp\Electron\Index.js ..\smarthome-raspberry\Electron
copy .\ClientApp\Electron\package.json ..\smarthome-raspberry\Electron
copy .\ClientApp\Electron\package-lock.json ..\smarthome-raspberry\Electron

cd ..\smarthome-raspberry\Main\
rmdir ClientApp

cd ..\..\smarthome

xcopy .\bin\Debug\net5.0\linux-arm\publish ..\smarthome-raspberry\Main /E/H/y

cd ..\smarthome-raspberry
git pull

git add .
git commit -m "Auto publish"

git pull
git push

pause cd ./
