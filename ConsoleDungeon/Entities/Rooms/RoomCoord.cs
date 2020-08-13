using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleDungeon.Entities.Rooms
{
    public struct RoomCoord
    {
        public int X { get; set; }
        public int Y { get; set; }

        public override string ToString() => $"[{X}, {Y}]";
    }
}
