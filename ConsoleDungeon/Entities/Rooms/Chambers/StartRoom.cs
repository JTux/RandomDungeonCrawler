using ConsoleDungeon.Entities.Events.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleDungeon.Entities.Rooms.Chambers
{
    public class StartRoom : Chamber
    {
        public StartRoom()
        {
            Coords = new RoomCoords(0,0);
            Name = "Starting Chamber";
        }

        public override Loot Loot => null;
    }
}
