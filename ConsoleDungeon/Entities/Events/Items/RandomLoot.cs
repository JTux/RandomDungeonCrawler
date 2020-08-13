using ConsoleDungeon.Entities.Events.Items;
using ConsoleDungeon.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleDungeon.Entities.Events.Items
{
    public class RandomLoot : Encounter
    {
        public RandomLoot()
        {
            Loot = LootTable.GetRandomLoot();
        }

        public Loot Loot { get; private set; }
    }
}
