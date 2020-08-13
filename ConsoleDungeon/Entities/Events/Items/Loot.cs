using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleDungeon.Entities.Events.Items
{
    public class Loot
    {
        public Loot(string name, int value)
        {
            Name = name;
            Value = value;
        }
        public Loot(string name, int value, int dropChance)
            : this(name, value)
        {
            DropChance = dropChance;
        }

        public string Name { get; }
        public int Value { get; }
        public int DropChance { get; }
    }
}
