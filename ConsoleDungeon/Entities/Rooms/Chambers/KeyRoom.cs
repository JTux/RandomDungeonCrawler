using ConsoleDungeon.Entities.Events.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleDungeon.Entities.Rooms.Chambers
{
    public class KeyRoom : Chamber
    {
        public override Loot Loot => new BossRoomKey();
    }
}
