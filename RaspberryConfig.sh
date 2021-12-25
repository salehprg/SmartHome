

cat <<EOF > /etc/systemd/system/smarthome.service
    [Unit]
    Description=Example .NET Web API App running on Ubuntu

    [Service]
    WorkingDirectory=/home/pi/Desktop/SmartHome/smarthome-raspberry/Main
    ExecStart=/home/pi/Desktop/SmartHome/smarthome-raspberry/Main/webAPI --urls http://0.0.0.0:80/
    Restart=always
    # Restart service after 10 seconds if the dotnet service crashes:
    RestartSec=10
    KillSignal=SIGINT
    SyslogIdentifier=dotnet-example
    User=root
    Environment=ASPNETCORE_ENVIRONMENT=Production
    Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false

    [Install]
    WantedBy=multi-user.target

EOF

#add as service
sudo systemctl enable smarthome.service 
sudo systemctl start smarthome.service 


sudo systemctl enable NetworkManager.service
sudo systemctl start NetworkManager.service 

cat <<EOF > /etc/systemd/system/smarthomeUI.service

    [Unit]
    Description= Robin part SmartHome UI webApplication Copyright 2021
    After=smarthome.service

    [Service]
    #WorkingDirectory=/home/pi/Desktop/SmartHome/smarthome-raspberry/Main
    TimeoutStartSec=infinity
    ExecStartPre=/bin/sleep 20
    ExecStart=startx /usr/bin/chromium-browser http://smart.home/ --window-size=1280,1024 --start-fullscreen --kiosk  --no-sandbox
    Restart=always
    # Restart service after 10 seconds if the dotnet service crashes:
    RestartSec=10
    KillSignal=SIGINT
    SyslogIdentifier=UI-example
    User=root

    [Install]
    WantedBy=multi-user.target

EOF

sudo systemctl enable smarthomeUI.service 
sudo systemctl start smarthomeUI.service 



cat <<EOF > /etc/systemd/system/bt-agent.service

    [Unit]
    Description=Bluetooth Auth Agent
    After=bluetooth.service
    PartOf=bluetooth.service

    [Service]
    Type=simple
    ExecStart=/usr/bin/bt-agent -c NoInputNoOutput

    [Install]
    WantedBy=bluetooth.target

EOF

sudo systemctl enable bt-agent.service
sudo systemctl start bt-agent.service