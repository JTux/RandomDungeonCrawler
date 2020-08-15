using ConsoleDungeon.Entities.Rooms;
using ConsoleDungeon.Entities.Rooms.Chambers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleDungeon.Entities
{
    public class Dungeon
    {
        private static readonly Random Random = new Random();

        public List<Room> Rooms { get; set; } = new List<Room>();
        private List<Room> Chambers => Rooms.Where(r => r is Chamber).ToList();
        private HashSet<RoomCoords> OccupiedCoords => Rooms.Select(r => r.Coords).ToHashSet();

        public static Dungeon Generate()
        {
            var dungeon = new Dungeon();

            dungeon.CreateInitialChamber();

            var maxBranchCount = Random.Next(3, 7);

            for (int i = 0; i < maxBranchCount; i++)
                dungeon.GenerateBranch();

            return dungeon.OccupiedCoords.Count != dungeon.Rooms.Count ? Generate() : dungeon;
        }

        private void CreateInitialChamber() => Rooms.Add(new StartRoom());

        private void GenerateBranch()
        {
            Room chamber;

            do chamber = GetRandomChamber();
            while (chamber.TotalEmptySides <= 0);

            GenerateHall(chamber);
            while (!GenerateChamber())
            {
                if (Rooms.FirstOrDefault(r => r is Hallway && r.TotalEmptySides == 3) is null)
                    break;
            }
        }

        private Room GetRandomChamber()
        {
            var availableChambers = Chambers.Where(c => c.TotalEmptySides > 0).ToList();
            return availableChambers[Random.Next(0, availableChambers.Count)];
        }

        private void GenerateHall(Room chamber)
        {
            var currentRoom = chamber;
            var branchLength = Random.Next(3, 7);
            for (int i = 0; i < branchLength; i++)
            {
                var nextRoom = GetNextHall(currentRoom);
                Rooms.Add(nextRoom);
                currentRoom = nextRoom;
            }
        }

        private Room GetNextHall(Room startingRoom)
        {
            while (true)
            {
                var adjacentRooms = startingRoom.AdjacentRooms;
                var directionalId = Random.Next(0, 4);

                var newRoom = adjacentRooms[directionalId];
                if (newRoom is null)
                {
                    var newCoords = GetCoords(startingRoom, directionalId);

                    newRoom = new Hallway
                    {
                        Coords = newCoords
                    };

                    if (OccupiedCoords.Contains(newCoords))
                    {
                        if (GetOccupiedSideCount(startingRoom) > 3)
                        {
                            var neighborCell = Rooms.FirstOrDefault(r => r.Coords.Equals(GetCoords(startingRoom, directionalId)));
                            ConnectRoom(neighborCell, startingRoom, directionalId);

                            return newRoom;
                        }

                        continue;
                    }

                    ConnectRoom(startingRoom, newRoom, directionalId);

                    return newRoom;
                }
            }
        }

        private int GetOccupiedSideCount(Room newRoom)
        {
            var occupiedSideCount = 0;

            for (int i = 0; i < 4; i++)
                if (OccupiedCoords.Contains(GetCoords(newRoom, i)))
                    occupiedSideCount++;

            return occupiedSideCount;
        }

        private RoomCoords GetCoords(Room startingRoom, int directionalId)
        {
            return new RoomCoords
            {
                X = startingRoom.Coords.X + (directionalId switch
                {
                    1 => 1,
                    3 => -1,
                    _ => 0
                }),
                Y = startingRoom.Coords.Y + (directionalId switch
                {
                    0 => 1,
                    2 => -1,
                    _ => 0
                })
            };
        }

        private void ConnectRoom(Room startingRoom, Room newRoom, int directionalId)
        {
            switch (directionalId)
            {
                case 0:
                    startingRoom.AdjacentNorth = newRoom;
                    newRoom.AdjacentSouth = startingRoom;
                    break;
                case 1:
                    startingRoom.AdjacentEast = newRoom;
                    newRoom.AdjacentWest = startingRoom;
                    break;
                case 2:
                    startingRoom.AdjacentSouth = newRoom;
                    newRoom.AdjacentNorth = startingRoom;
                    break;
                case 3:
                    startingRoom.AdjacentWest = newRoom;
                    newRoom.AdjacentEast = startingRoom;
                    break;
            }
        }

        private bool GenerateChamber()
        {
            var endOfHall = Rooms
                .FirstOrDefault(r => r is Hallway && r.TotalEmptySides == 3);

            if (endOfHall is null)
                return false;

            var adjacentRooms = endOfHall.AdjacentRooms;

            var directionalId = Random.Next(0, 4);

            if (adjacentRooms[directionalId] is null)
            {
                var newCoords = GetCoords(endOfHall, directionalId);
                if (!OccupiedCoords.Contains(newCoords))
                {
                    adjacentRooms[directionalId] = new Chamber()
                    {
                        Coords = newCoords
                    };

                    var targetRoom = adjacentRooms[directionalId];

                    ConnectRoom(endOfHall, targetRoom, directionalId);
                    Rooms.Add(targetRoom);

                    return true;
                }
                else if (GetOccupiedSideCount(endOfHall) > 3)
                {
                    var neighborCell = Rooms.FirstOrDefault(r => r.Coords.Equals(newCoords));
                    ConnectRoom(endOfHall, neighborCell, directionalId);

                    var newChamber = new Chamber
                    {
                        AdjacentNorth = endOfHall.AdjacentNorth,
                        AdjacentEast = endOfHall.AdjacentEast,
                        AdjacentSouth = endOfHall.AdjacentSouth,
                        AdjacentWest = endOfHall.AdjacentWest,
                        Coords = endOfHall.Coords
                    };

                    var hallIndex = Rooms.IndexOf(endOfHall);

                    Rooms[hallIndex] = newChamber;

                    return true;
                }
            }

            return false;
        }

        private Dictionary<int, Room> GetRoomDictionary(Room room) =>
            new Dictionary<int, Room>
                {
                    { 0, room.AdjacentNorth },
                    { 1, room.AdjacentEast },
                    { 2, room.AdjacentSouth },
                    { 3, room.AdjacentWest }
                };
    }
}
