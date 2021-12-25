using smarthome.Model.Modules.Devices;
using smarthome.Model.Modules.MainModule;

namespace smarthome.Model.Modules.Interface
{
    public interface IBaseDevice
    {
        int Id {get;set;}
        int moduleParentId {get;set;}
        Module moduleParent {get;set;}

        int registerid {get;set;}

        int RoomId {get;set;}

        string name {get; set;}
        string status {get;set;}
        DeviceType deviceType {get;set;}

        BaseDevice GetBase();
    }
}