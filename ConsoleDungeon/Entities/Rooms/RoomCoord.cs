using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleDungeon.Entities.Rooms
{
    public struct RoomCoords
    {
        public RoomCoords(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public override string ToString() => $"[{X}, {Y}]";
    }
}
