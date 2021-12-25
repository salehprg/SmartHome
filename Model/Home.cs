using System.Collections.Generic;

namespace smarthome.Model
{
    public class Home
    {
        public int HomeId {get;set;}
        public virtual List<Room> rooms {get;set;}
        public virtual List<StrokeArea> strokedAreas {get;set;}
    }
}