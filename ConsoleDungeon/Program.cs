using ConsoleDungeon.Entities;
using ConsoleDungeon.Entities.Rooms;
using ConsoleDungeon.Entities.Rooms.Chambers;
using ConsoleDungeon.Resources;
using System;
using System.Linq;

namespace ConsoleDungeon
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                for (int i = 0; i < 10; i++)
                {
                    var dungeon = Dungeon.Generate();
                    PrintMap(dungeon);
                }

                Console.ReadLine();
            }
        }

        static void PrintMap(Dungeon dungeon)
        {
            var orderedRooms = dungeon.Rooms.OrderBy(r => r.Coords.X).ThenBy(r => r.Coords.Y).ToList();
            var xValues = orderedRooms.Select(r => r.Coords.X).OrderBy(x => x).ToList();
            var yValues = orderedRooms.Select(r => r.Coords.Y).OrderBy(y => y).ToList();

            var farLeft = xValues.First();
            var farRight = xValues.Last();
            var farUp = yValues.Last();
            var farDown = yValues.First();
            for (int y = farUp; y >= farDown; y--)
            {
                for (int x = farLeft; x <= farRight; x++)
                {
                    var allRooms = orderedRooms.Where(r => r.Coords.X == x && r.Coords.Y == y).ToList();
                    var room = allRooms.FirstOrDefault();
                    if(allRooms.Count > 1)
                        Console.Write(allRooms.Count);
                    else if (room is Chamber)
                    {
                        Console.ForegroundColor = room is StartRoom ? ConsoleColor.Red : ConsoleColor.Blue;
                        Console.Write("C");
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                    else if(room is Hallway)
                        Console.Write("H");
                    else
                        Console.Write(" ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

    }
}
