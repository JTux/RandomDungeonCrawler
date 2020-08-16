using ConsoleDungeon.Entities;
using ConsoleDungeon.Entities.Rooms;
using ConsoleDungeon.Entities.Rooms.Chambers;
using ConsoleDungeon.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleDungeon
{
    class Program
    {
        static void Main(string[] args)
        {
            var dungeon = Dungeon.Generate();
            HandleMovement(dungeon);


            Console.ReadLine();
        }

        static void PrintMap(Dungeon dungeon, RoomCoords userPos)
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
                List<string> verticalConnections = new List<string>();

                for (int x = farLeft; x <= farRight; x++)
                {
                    var allRooms = orderedRooms.Where(r => r.Coords.X == x && r.Coords.Y == y).ToList();
                    var room = allRooms.FirstOrDefault();
                    if (allRooms.Count > 1)
                        Console.Write(allRooms.Count);
                    else if (room is null)
                    {
                        Console.Write(" ");
                        verticalConnections.Add("  ");
                    }
                    else
                    {
                        if (room.Coords.Equals(userPos))
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write("U");
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
                        else if (room is Chamber)
                        {
                            Console.ForegroundColor = room is StartRoom ? ConsoleColor.Red : ConsoleColor.Blue;
                            Console.Write("C");
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
                        else if (room is Hallway)
                        {
                            Console.Write("H");
                        }

                        verticalConnections.Add(room.AdjacentSouth != null ? "| " : "  ");
                    }
                    // Connect Halls
                    Console.Write(room != null && room.AdjacentEast != null ? "-" : " ");


                }
                Console.WriteLine();

                foreach (var connection in verticalConnections)
                {
                    Console.Write(connection);
                }

                Console.WriteLine();
            }
            Console.WriteLine();
        }

        static void HandleMovement(Dungeon dungeon)
        {
            void WriteOptions(Room room, List<Direction> directions)
            {
                Console.Write($"You can move: ");
                foreach (var direction in directions)
                    Console.Write(direction + " ");
            }

            var room = dungeon.Rooms.FirstOrDefault(r => r is StartRoom);
            RoomCoords userPos = new RoomCoords(0, 0);
            while (true)
            {
                PrintMap(dungeon, userPos);

                List<Direction> directions = new List<Direction>();
                if (room.AdjacentNorth != null)
                    directions.Add(Direction.North);
                if (room.AdjacentEast != null)
                    directions.Add(Direction.East);
                if (room.AdjacentSouth != null)
                    directions.Add(Direction.South);
                if (room.AdjacentWest != null)
                    directions.Add(Direction.West);

                WriteOptions(room, directions);
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.W:
                    case ConsoleKey.UpArrow:
                        if (directions.Contains(Direction.North) && room.AdjacentNorth != null)
                        {
                            room = room.AdjacentNorth;
                            userPos.Y++;
                        }
                        break;
                    case ConsoleKey.A:
                    case ConsoleKey.LeftArrow:
                        if (directions.Contains(Direction.West) && room.AdjacentWest != null)
                        {
                            room = room.AdjacentWest;
                            userPos.X--;
                        }
                        break;
                    case ConsoleKey.S:
                    case ConsoleKey.DownArrow:
                        if (directions.Contains(Direction.South) && room.AdjacentSouth != null)
                        {
                            room = room.AdjacentSouth;
                            userPos.Y--;
                        }
                        break;
                    case ConsoleKey.D:
                    case ConsoleKey.RightArrow:
                        if (directions.Contains(Direction.East) && room.AdjacentEast != null)
                        {
                            room = room.AdjacentEast;
                            userPos.X++;
                        }
                        break;
                    default:
                        break;
                }
                Console.Clear();
            }
        }

    }
    enum Direction { North, East, South, West }
}
