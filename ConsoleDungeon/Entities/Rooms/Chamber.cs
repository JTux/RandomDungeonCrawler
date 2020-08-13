using ConsoleDungeon.Entities.Events;
using ConsoleDungeon.Entities.Events.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleDungeon.Entities.Rooms
{
    public class Chamber : Room
    {
        public virtual Loot Loot { get; set; }
        public virtual Encounter Encounter { get; set; }
    }
}
