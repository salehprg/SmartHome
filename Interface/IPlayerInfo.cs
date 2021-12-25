using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tmds.DBus;

namespace smarthome.Interface
{
    [DBusInterface("org.bluez.MediaControl1")]
    public interface IPlayerInfo : IDBusObject
    {
        Task PlayAsync();

        Task<IDictionary<string, object>> GetAllAsync();
        Task<T> GetAsync<T>(string prop);
        Task SetAsync(string prop, object val);
        Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
    }
}
