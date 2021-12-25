#------------------------------------------------------------------------
#                                                                       |
# For how to compile and push app to raspberry look at Publish.bat      |
#                                                                       |
#------------------------------------------------------------------------

#create empty file name "ssh" in the root directory memory
echo -e "http_proxy=http://admin:Squidpass.24@hr.hamid-najafi.ir:3128/\nhttps_proxy=http://admin:Squidpass.24@hr.hamid-najafi.ir:3128/" | sudo tee -a /etc/environment

mkdir $HOME/Desktop
mkdir $HOME/Desktop/dotnetsetup

cd $HOME/Desktop

wget https://download.visualstudio.microsoft.com/download/pr/f456f253-db24-45ea-9c73-f507f93a8cd2/6efe7bed8639344d9c9afb8a46686c99/dotnet-sdk-5.0.302-linux-arm.tar.gz
mkdir -p $HOME/dotnet && tar zxf dotnet-sdk-5.0.302-linux-arm.tar.gz -C $HOME/dotnet
echo -e "DOTNET_ROOT=$HOME/dotnet" | sudo tee -a /etc/environment
echo -e "PATH=$PATH:/home/pi/dotnet:/home/pi/.dotnet/tools" | sudo tee -a ~/.bashrc

dotnet tool install --global dotnet-ef

#add these to end of .bashrc in ~/.bashrc
export PATH=$PATH:$HOME/.dotnet/tools

cat <<EOF > /etc/resolv.conf
#set DNS from shecan.ir
nameserver 178.22.122.100
nameserver 185.51.200.2
EOF

sudo apt-get update
sudo apt-get upgrade -y
sudo apt-get install chromium-browser pulseaudio-*. bluetooth pi-bluetooth bluez blueman bluez-tools git mariadb-server apt-transport-https nodejs npm network-manager screen -y

#install frontend dependency

sudo mysql_secure_installation 

sudo mysql -u root -p
#run this in mysql
    #ALTER USER 'root'@'localhost' IDENTIFIED BY 'Saleh-1379';

#builded file for raspberry
mkdir $HOME/Desktop/SmartHome
cd $HOME/Desktop/SmartHome

#save password for git
git config --global credential.helper store
git clone https://gitlab.com/saleh_prg/smarthome-raspberry.git

chown pi -R $HOME/Desktop/SmartHome/smarthome-raspberry/Main/
chmod -R +x $HOME/Desktop/SmartHome/smarthome-raspberry/Main/

#source file for update database

mkdir $HOME/Desktop/SmartHome-source
cd $HOME/Desktop/SmartHome-source

git clone https://gitlab.com/jMoghaddam/smarthome.git


#add as service
sudo systemctl enable smarthome.service 
sudo systemctl start smarthome.service 

sudo systemctl start wpa_supplicant.service

sudo systemctl enable NetworkManager.service
sudo systemctl start NetworkManager.service 

#swap serial0 with serial1 => /dev/ttyS0 <-> /dev/ttyAMA0

#comment dtoverlay under [pi4]
echo -e "enable_uart=1" | sudo tee -a  /boot/config.txt
echo -e "dtoverlay=pi3-miniuart-bt" | sudo tee -a  /boot/config.txt
echo -e "avoid_warnings=1" | sudo tee -a  /boot/config.txt  

#and add the line (at the bottom):

sudo apt-get --no-install-recommends install xserver-xorg xserver-xorg-video-fbdev xinit pciutils xinput xfonts-100dpi xfonts-75dpi xfonts-scalable -y
sudo startx /usr/bin/chromium-browser http://localhost/ --window-size=1280,1024 --start-fullscreen --kiosk  --no-sandbox

#add auto start for UI 

#hide cursor guide
#https://raspberrypi.stackexchange.com/questions/53127/how-to-permanently-hide-mouse-pointer-or-cursor-on-raspberry-pi

##To work with any X server (I'm using the stretch version), Edit the /usr/bin/startx file and change the defaultserverargs line to: defaultserverargs="-nocursor"

#Hide warning Voltage


sudo nano /boot/cmdline.txt
#remove the line: console=serial0,115200

echo -e "PRETTY_HOSTNAME=MyHome" | sudo tee -a /etc/machine-info
#Bletooth Setup

sudo usermod -a -G bluetooth pi

sudo nano /etc/bluetooth/main.conf

# And add / uncomment / change

# Class = 0x41C

# DiscoverableTimeout = 0

sudo systemctl restart bluetooth

bluetoothctl

# [bluetooth]# power on
# [bluetooth]# discoverable on
# [bluetooth]# pairable on
# [bluetooth]# agent on
# [bluetooth]# default-agent
# [bluetooth]# quit

pulseaudio --start

sudo nano /etc/pulse/daemon.conf

#add
#resample-method = trivial

systemctl --user enable pulseaudio

sudo nano /etc/hosts

# Change hostname to smart.home

sudo nano /etc/hostname
#Change raspberrypi to smart.home

