using System;
using System.Collections.Generic;

namespace smarthome.Model.Bluetooth
{
    public class PlayerProperties
    {
        public UInt32 Duration {get;set;}
        public UInt32 Position {get;set;}
        public bool PlaybackStatus {get;set;}
        public string Track {get;set;}
        public string Status {get;set;}
        public IDictionary<string,object> metadatas {get;set;}
    }
}