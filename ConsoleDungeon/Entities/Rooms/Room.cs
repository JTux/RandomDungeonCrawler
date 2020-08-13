﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleDungeon.Entities.Rooms
{
    public abstract class Room
    {
        public List<Room> AdjacentRooms =>
            new List<Room> { AdjacentNorth, AdjacentEast, AdjacentSouth, AdjacentWest };

        public Room AdjacentNorth { get; set; }
        public Room AdjacentEast { get; set; }
        public Room AdjacentSouth { get; set; }
        public Room AdjacentWest { get; set; }

        public RoomCoord Coords { get; set; }
    }
}