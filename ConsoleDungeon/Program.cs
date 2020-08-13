using ConsoleDungeon.Entities;
using ConsoleDungeon.Resources;
using System;

namespace ConsoleDungeon
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 10; i++)
            {
                var dungeon = new Dungeon();
                dungeon.GenerateRooms();

                PrintMap(dungeon);
                Console.WriteLine();
            }

            Console.ReadLine();
        }

        static void PrintMap(Dungeon dungeon)
        {
            foreach (var room in dungeon.Rooms)
            {
                Console.WriteLine(room.Coords);
            }
        }

    }
}
