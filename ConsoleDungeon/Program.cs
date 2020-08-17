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
            Console.CursorVisible = false;
            
            //var customDungeon = GetCustomDungeon();
            //HandleMovement(customDungeon);

            var randomizedDungeon = Dungeon.Generate(3, 7);
            HandleMovement(randomizedDungeon);

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
                        // Print Chamber
                        PrintRoomLabel(room.Coords.Equals(userPos) ? 'U' : room.GetType().Name[0]);
                        verticalConnections.Add(room.AdjacentSouth != null ? "| " : "  ");
                    }

                    // Connect Halls
                    Console.Write(room != null && room.AdjacentEast != null ? "-" : " ");
                }
                Console.WriteLine();

                // Print Vertical Connections
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
            var room = dungeon.Rooms.FirstOrDefault(r => r is StartRoom);
            RoomCoords userPos = new RoomCoords(0, 0);

            PrintMap(dungeon, userPos);
            int cursorTop = Console.CursorTop;
            while (true)
            {
                var previousPos = userPos;

                List<Direction> directions = new List<Direction>();
                if (room.AdjacentNorth != null)
                    directions.Add(Direction.North);
                if (room.AdjacentEast != null)
                    directions.Add(Direction.East);
                if (room.AdjacentSouth != null)
                    directions.Add(Direction.South);
                if (room.AdjacentWest != null)
                    directions.Add(Direction.West);

                PrintMoveOptions(directions, cursorTop);
                EvaluatePlayerMove(ref room, ref userPos, directions);
                UpdatePlayerPosition(dungeon, userPos, previousPos);
            }
        }

        static void UpdatePlayerPosition(Dungeon dungeon, RoomCoords currentPos, RoomCoords previousPos)
        {
            var orderedRooms = dungeon.Rooms.OrderBy(r => r.Coords.X).ThenBy(r => r.Coords.Y).ToList();
            var xValues = orderedRooms.Select(r => r.Coords.X).OrderBy(x => x).ToHashSet().ToList();
            var yValues = orderedRooms.Select(r => r.Coords.Y).OrderByDescending(y => y).ToHashSet().ToList();

            var roomAtPreviousPos = dungeon.Rooms.FirstOrDefault(r => r.Coords.Equals(previousPos));
            UpdateChamber(previousPos, xValues, yValues, roomAtPreviousPos.GetType().Name[0]);

            UpdateChamber(currentPos, xValues, yValues, 'U');
        }

        static void UpdateChamber(RoomCoords previousPos, List<int> xValues, List<int> yValues, char label)
        {
            var left = xValues.IndexOf(previousPos.X);
            Console.CursorLeft = left * 2;
            var top = yValues.IndexOf(previousPos.Y);
            Console.CursorTop = top * 2;

            PrintRoomLabel(label);
        }

        static void PrintRoomLabel(char label)
        {
            switch (label)
            {
                case 'B':
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write("!");
                    break;
                case 'S':
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("C");
                    break;
                case 'C':
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("C");
                    break;
                case 'U':
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("U");
                    break;
                case 'H':
                    Console.Write("H");
                    break;
            }
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        static void PrintMoveOptions(List<Direction> directions, int cursorTop)
        {
            Console.CursorLeft = 0;
            Console.CursorTop = cursorTop;

            Console.Write($"You can move: ");
            foreach (var direction in directions)
                Console.Write(direction + " ");

            Console.Write(new string(' ', 36 - Console.CursorLeft));
        }

        static void EvaluatePlayerMove(ref Room room, ref RoomCoords userPos, List<Direction> directions)
        {
            var moveDirection = GetMoveDirection(directions);
            switch (moveDirection)
            {
                case Direction.North:
                    if (room.AdjacentNorth != null)
                    {
                        room = room.AdjacentNorth;
                        userPos.Y++;
                    }
                    break;
                case Direction.West:
                    if (room.AdjacentWest != null)
                    {
                        room = room.AdjacentWest;
                        userPos.X--;
                    }
                    break;
                case Direction.South:
                    if (room.AdjacentSouth != null)
                    {
                        room = room.AdjacentSouth;
                        userPos.Y--;
                    }
                    break;
                case Direction.East:
                    if (room.AdjacentEast != null)
                    {
                        room = room.AdjacentEast;
                        userPos.X++;
                    }
                    break;
                default:
                    break;
            }
        }

        static Direction GetMoveDirection(List<Direction> directions)
        {
            while (true)
            {
                var moveDirection = Console.ReadKey(true).Key;
                Direction direction = Direction.North;
                switch (moveDirection)
                {
                    case ConsoleKey.W:
                    case ConsoleKey.UpArrow:
                        direction = Direction.North;
                        break;
                    case ConsoleKey.A:
                    case ConsoleKey.LeftArrow:
                        direction = Direction.West;
                        break;
                    case ConsoleKey.S:
                    case ConsoleKey.DownArrow:
                        direction = Direction.South;
                        break;
                    case ConsoleKey.D:
                    case ConsoleKey.RightArrow:
                        direction = Direction.East;
                        break;
                }

                if (directions.Contains(direction))
                {
                    return direction;
                }
            }
        }

        static Dungeon GetCustomDungeon()
        {
            var roomN = new BossRoom() { Coords = new RoomCoords(0, 1) };
            var roomNE = new Chamber() { Coords = new RoomCoords(1, 1) };
            var roomW = new Chamber() { Coords = new RoomCoords(-1, 0) };
            var roomC = new StartRoom() { Coords = new RoomCoords(0, 0) };
            var roomE = new Chamber() { Coords = new RoomCoords(1, 0) };
            var roomSW = new Chamber() { Coords = new RoomCoords(-1, -1) };
            var roomS = new Chamber() { Coords = new RoomCoords(0, -1) };
            var roomSE = new Chamber() { Coords = new RoomCoords(1, -1) };
            var roomNW = new Chamber() { Coords = new RoomCoords(-1, 1) };

            roomNW.AdjacentEast = roomN;
            roomNW.AdjacentSouth = roomW;

            roomN.AdjacentWest = roomNW;
            roomN.AdjacentEast = roomNE;

            roomNE.AdjacentWest = roomN;
            roomNE.AdjacentSouth = roomE;

            roomE.AdjacentNorth = roomNE;
            roomE.AdjacentWest = roomC;

            roomC.AdjacentEast = roomE;
            roomC.AdjacentWest = roomW;
            roomC.AdjacentSouth = roomS;

            roomS.AdjacentNorth = roomC;
            roomS.AdjacentEast = roomSE;
            roomS.AdjacentWest = roomSW;

            roomSE.AdjacentWest = roomS;

            roomW.AdjacentNorth = roomNW;
            roomW.AdjacentEast = roomC;
            roomW.AdjacentSouth = roomSW;

            roomSW.AdjacentNorth = roomW;
            roomSW.AdjacentEast = roomS;

            return new Dungeon()
            {
                Rooms = new List<Room>
                {
                    roomNW, roomN, roomNE, roomW, roomC, roomE, roomSW, roomS, roomSE
                }
            };
        }
    }

    enum Direction { North, East, South, West }
}
