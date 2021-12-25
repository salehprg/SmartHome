using System;
using Microsoft.EntityFrameworkCore;
using smarthome.Model.Modules.MainModule;
using smarthome.Model.Modules.Devices;
using smarthome.Model.Modules;
using smarthome.Model.Camera;

namespace smarthome.Model
{
    public class DbContextModel : DbContext
    {
        public DbContextModel(DbContextOptions<DbContextModel> options)
            : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = false;
        }
        public DbSet<Room> rooms {get;set;}
        public DbSet<BaseDevice> devices {get;set;}
        public DbSet<Module> modules {get;set;}
        public DbSet<Home> Homes {get;set;}
        public DbSet<StrokeArea> StrokeAreas {get;set;}
        public DbSet<Scenario> Scenarios {get;set;}
        public DbSet<CameraModel> CameraModels {get;set;}
        public DbSet<ScenarioAction> ScenarioActions {get;set;}
        public DbSet<SmartplugInfoModel> SmartplugInfoModels {get;set;}
        
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Home>().HasData(
                new Home { HomeId = 1 });

            modelBuilder.Entity<StrokeArea>().HasData(
                new StrokeArea { Id = 1 , stroke = "M562.71,225V354h-290V319H418.37a6.09,6.09,0,0,0,6.09-6.09V225Z" , HomeId = 1 });
            modelBuilder.Entity<StrokeArea>().HasData(
                new StrokeArea { Id = 2 , stroke = "M8.09,130V347.91A6.09,6.09,0,0,0,14.18,354h54V130Z" , HomeId = 1 });
            modelBuilder.Entity<StrokeArea>().HasData(
                new StrokeArea { Id = 3 , stroke = "M216.37,49.82H358.8V92.5H216.37Z" , HomeId = 1 });

            modelBuilder.Entity<Room>().HasData(
                new Room { RoomId = 2 , HomeId = 1 , roomName = "آشپزخانه" , X = 142 , Y = 240.8f 
                            , area = "M68.18,130V359.9A6.09,6.09,0,0,0,74.27,366h136a6.09,6.09,0,0,0,6.09-6.09V160H186.21V130Z"
                            , border = "M96,130H68.18V359.9A6.09,6.09,0,0,0,74.27,366h136a6.09,6.09,0,0,0,6.09-6.09V225 M152.71,130H186.21V160H218.5" });
            
            modelBuilder.Entity<Room>().HasData(
                new Room { RoomId = 3 , HomeId = 1 , roomName = "اتاق خواب" , X = 106 , Y = 66
                            , area = "M152.71,130h63.66V8.09A6.09,6.09,0,0,0,210.27,2H8.09A6.09,6.09,0,0,0,2,8.09V123.95A6.09,6.09,0,0,0,8.09,130H96Z"
                            , border = "M152.71,130h63.66V8.09A6.09,6.09,0,0,0,210.27,2H8.09A6.09,6.09,0,0,0,2,8.09V123.95A6.09,6.09,0,0,0,8.09,130H96" });
            
            modelBuilder.Entity<Room>().HasData(
                new Room { RoomId = 4 , HomeId = 1 , roomName = "پذیرایی" , X = 468 , Y = 134
                            , area = "M358.8,160V49.82a6.09,6.09,0,0,1,6.09-6.09H570.78a6.09,6.09,0,0,1,6.09,6.09V218.9a6.09,6.09,0,0,1-6.09,6.09h-212Z"
                            , border = "M358.8,160V49.82a6.09,6.09,0,0,1,6.09-6.09H570.78a6.09,6.09,0,0,1,6.09,6.09V218.9a6.09,6.09,0,0,1-6.09,6.09h-212" });

            modelBuilder.Entity<Room>().HasData(
                new Room { RoomId = 5 , HomeId = 1 , roomName = "هال" , X = 320 , Y = 273
                            , area = "M216.37,354V92.5H358.8V225H424.39V319H272.71V354Z"
                            , border = "M216.37,225V356 M216.21,162V92.5H358.8V160 M358.8,225H424.39V312.91a6.09,6.09,0,0,1,-6.09,6.09H272.71V356" });
            

            modelBuilder.Entity<Module>().HasData(new Module {Id = 1 , slave_id = 1 , serialNumber = "01 01 123698 0000", moduleType = ModuleType.Relay});
            modelBuilder.Entity<Module>().HasData(new Module {Id = 2 , slave_id = 2 , serialNumber = "02 02 767892 0000", moduleType = ModuleType.Measurement});

            
            modelBuilder.Entity<BaseDevice>().HasData(new BaseDevice {Id = 1 , RoomId = 2 , serialNumber = "01 123144" , name = "لامپ کم مصرف", status = "false" , deviceType = DeviceType.LED , registerid = 1 , moduleParentId = 1});
            modelBuilder.Entity<BaseDevice>().HasData(new BaseDevice {Id = 2 , RoomId = 2 , serialNumber = "01 663525" , name = "مهتابی", status = "false" , deviceType = DeviceType.LED , registerid = 2 , moduleParentId = 1});
            modelBuilder.Entity<BaseDevice>().HasData(new BaseDevice {Id = 3 , RoomId = 2 , serialNumber = "01 767648" , name = "2مهتابی", status = "false" , deviceType = DeviceType.LED , registerid = 3 , moduleParentId = 1});
            modelBuilder.Entity<BaseDevice>().HasData(new BaseDevice {Id = 4 , RoomId = 2 , serialNumber = "01 134214" , name = "3مهتابی", status = "false" , deviceType = DeviceType.LED , registerid = 4 , moduleParentId = 1});
            modelBuilder.Entity<BaseDevice>().HasData(new BaseDevice {Id = 5 , RoomId = 2 , serialNumber = "01 534634" , name = "4مهتابی", status = "false" , deviceType = DeviceType.LED , registerid = 5 , moduleParentId = 1});
            modelBuilder.Entity<BaseDevice>().HasData(new BaseDevice {Id = 6 , RoomId = 2 , serialNumber = "01 346213" , name = "5مهتابی", status = "false" , deviceType = DeviceType.LED , registerid = 6 , moduleParentId = 1});
            modelBuilder.Entity<BaseDevice>().HasData(new BaseDevice {Id = 7 , RoomId = 2 , serialNumber = "01 258754" , name = "6مهتابی", status = "false" , deviceType = DeviceType.LED , registerid = 7 , moduleParentId = 1});
            modelBuilder.Entity<BaseDevice>().HasData(new BaseDevice {Id = 8 , RoomId = 2 , serialNumber = "01 245234" , name = "7مهتابی", status = "false" , deviceType = DeviceType.LED , registerid = 8 , moduleParentId = 1});

            modelBuilder.Entity<BaseDevice>().HasData(new BaseDevice {Id = 9 , RoomId = 2 , serialNumber = "02 315661" , name = "پرده", status = "50" , deviceType = DeviceType.Curtain , registerid = 9 , moduleParentId = 1});

            modelBuilder.Entity<BaseDevice>().HasData(new BaseDevice {Id = 10 , RoomId = 2 , serialNumber = "03 632647" , name = "پنجره", status = "true" , deviceType = DeviceType.Windoor , registerid = 10 , moduleParentId = 1});
            modelBuilder.Entity<BaseDevice>().HasData(new BaseDevice {Id = 11 , RoomId = 2 , serialNumber = "03 732577" , name = "در", status = "false" , deviceType = DeviceType.Windoor , registerid = 11 , moduleParentId = 1});

            modelBuilder.Entity<Scenario>().HasData(
                new Scenario {ScenarioId = 1 , ScenarioName = "FlipFlop" , cronjob = "0/15 * * ? * * *" , running = false}
            );

            modelBuilder.Entity<ScenarioAction>().HasData(new ScenarioAction {Id = 1 , ScenarioId = 1 , deviceId = 2 , status = "true" , delay = 0});
            modelBuilder.Entity<ScenarioAction>().HasData(new ScenarioAction {Id = 2 , ScenarioId = 1 , deviceId = 4 , status = "true" , delay = 0});
            modelBuilder.Entity<ScenarioAction>().HasData(new ScenarioAction {Id = 3 , ScenarioId = 1 , deviceId = 2 , status = "false" , delay = 0});
            modelBuilder.Entity<ScenarioAction>().HasData(new ScenarioAction {Id = 4 , ScenarioId = 1 , deviceId = 4 , status = "false" , delay = 0});
        
            modelBuilder.Entity<BaseDevice>().HasData(new BaseDevice {Id = 12 , serialNumber = "04 343252" , RoomId = 3 , name = "کامپیوتر", status = "true" , deviceType = DeviceType.SmartPlug , registerid = 1 , moduleParentId = 2});
            modelBuilder.Entity<BaseDevice>().HasData(new BaseDevice {Id = 13 , serialNumber = "04 863431" , RoomId = 4 , name = "تلویزیون", status = "true" , deviceType = DeviceType.SmartPlug , registerid = 2 , moduleParentId = 2});

            modelBuilder.Entity<SmartplugInfoModel>().HasData(new SmartplugInfoModel {Id = 1 , current = 5 , voltage = 209 , watt = 1045 , baseDeviceId = 12 , time = DateTime.Now});
            modelBuilder.Entity<SmartplugInfoModel>().HasData(new SmartplugInfoModel {Id = 2 , current = 6.3f , voltage = 208.8f , watt = 1315.44f , baseDeviceId = 12 , time = DateTime.Now});
            modelBuilder.Entity<SmartplugInfoModel>().HasData(new SmartplugInfoModel {Id = 3 , current = 4.4f , voltage = 208.5f , watt = 917.4f , baseDeviceId = 13 , time = DateTime.Now});
            modelBuilder.Entity<SmartplugInfoModel>().HasData(new SmartplugInfoModel {Id = 4 , current = 5.7f , voltage = 210.2f , watt = 1198.14f , baseDeviceId = 13 , time = DateTime.Now});

            
        }
    }
}