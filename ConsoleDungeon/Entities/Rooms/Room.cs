using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleDungeon.Entities.Rooms
{
    public abstract class Room
    {
        public List<Room> AdjacentRooms =>
            new List<Room> { AdjacentNorth, AdjacentEast, AdjacentSouth, AdjacentWest };

        public int TotalEmptySides => AdjacentRooms.Where(r => r is null).Count();

        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsExplored { get; set; }

        public Room AdjacentNorth { get; set; }
        public Room AdjacentEast { get; set; }
        public Room AdjacentSouth { get; set; }
        public Room AdjacentWest { get; set; }

        public RoomCoords Coords { get; set; }
    }
}
