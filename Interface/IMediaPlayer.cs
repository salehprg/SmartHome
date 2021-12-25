
using System.Threading.Tasks;
using Tmds.DBus;

namespace smarthome.Interface
{

    [DBusInterface("org.bluez.MediaPlayer1.")]
    public interface IMediaPlayer : IPlayer , IPlayerInfo
    {
        Task QuitAsync();
    }
}