using System;

namespace Model.Hotel.Modules
{
    [Serializable]
    public struct DoorWay
    {
        public CompassDirection direction;
        public int sort;
    }
}