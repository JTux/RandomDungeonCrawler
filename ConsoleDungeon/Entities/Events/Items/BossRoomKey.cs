using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleDungeon.Entities.Events.Items
{
    public class BossRoomKey : Loot
    {
        public BossRoomKey() 
            : base("Large Key", 1000, 0) { }
    }
}
