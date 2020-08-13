using ConsoleDungeon.Entities.Events.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleDungeon.Resources
{
    public static class LootTable
    {
        private static readonly Random Random = new Random();

        static LootTable()
        {
            SetLootTable();
        }

        public static List<Loot> Loots { get; private set; } = new List<Loot>();

        public static Loot GetRandomLoot()
        {
            var collectiveDropChance = Loots.Sum(l => l.DropChance);
            var itemDropValue = Random.Next(0, collectiveDropChance);

            foreach (var item in Loots)
            {
                itemDropValue -= item.DropChance;

                if (itemDropValue < 0)
                    return item;
            }

            return null;
        }

        private static void SetLootTable()
        {
            AddGoldItems();
        }

        private static void AddGoldItems()
        {
            Loots.AddRange(new List<Loot>
            {
                new Loot("Small Sack of Gold", 10, 50),
                new Loot("Large Sack of Gold", 100, 5)
            });
        }
    }
}
