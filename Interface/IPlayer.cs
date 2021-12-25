using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tmds.DBus;

namespace smarthome.Interface
{
    [DBusInterface("org.bluez.MediaPlayer1")]
    public interface IPlayer : IDBusObject
    {
        Task PlayAsync();
        Task PauseAsync();
        Task NextAsync();
        Task PreviousAsync();
        Task StopAsync();

        Task VolumeUpAsync();
        Task VolumeDownAsync();

        Task<IDictionary<string, object>> GetAllAsync();
        Task<T> GetAsync<T>(string prop);
        Task SetAsync(string prop, object val);
        Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
    }
}