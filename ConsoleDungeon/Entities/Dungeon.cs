using ConsoleDungeon.Entities.Rooms;
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

        public void GenerateRooms()
        {
            var firstChamber = new Chamber()
            {
                Coords = new RoomCoord { X = 0, Y = 0 },
                Loot = null,
                Encounter = null
            };

            Rooms.Add(firstChamber);

            GenerateBranch();
        }

        private void GenerateBranch()
        {
            Room chamber;

            do chamber = GetRandomChamber();
            while (chamber.AdjacentRooms.Where(r => r is null).Count() <= 0);

            GenerateHall(chamber);
        }

        private Room GetRandomChamber() => Chambers[Random.Next(0, Chambers.Count)];

        private void GenerateHall(Room chamber)
        {
            var currentRoom = chamber;
            for (int i = 0; i < 7; i++)
            {
                var nextRoom = SelectAdjacentRoom(currentRoom);
                Rooms.Add(nextRoom);
                currentRoom = nextRoom;
            }
        }

        // This is definitely doing too much
        private Room SelectAdjacentRoom(Room room)
        {
            while (true)
            {
                var rooms = new Dictionary<int, Room>
                {
                    { 0, room.AdjacentNorth },
                    { 1, room.AdjacentEast },
                    { 2, room.AdjacentSouth },
                    { 3, room.AdjacentWest }
                };
                var num = Random.Next(0, 4);
                var targetRoom = rooms[num];
                if (targetRoom is null)
                {
                    targetRoom = new Hallway();
                    switch (num)
                    {
                        case 0:
                            room.AdjacentNorth = targetRoom;
                            targetRoom.AdjacentSouth = room;
                            targetRoom.Coords = new RoomCoord { X = room.Coords.X, Y = room.Coords.Y + 1 };
                            return targetRoom;
                        case 1:
                            room.AdjacentEast = targetRoom;
                            targetRoom.AdjacentWest = room;
                            targetRoom.Coords = new RoomCoord { X = room.Coords.X + 1, Y = room.Coords.Y };
                            return targetRoom;
                        case 2:
                            room.AdjacentSouth = targetRoom;
                            targetRoom.AdjacentNorth = room;
                            targetRoom.Coords = new RoomCoord { X = room.Coords.X, Y = room.Coords.Y - 1 };
                            return targetRoom;
                        case 3:
                            room.AdjacentWest = targetRoom;
                            targetRoom.AdjacentEast = room;
                            targetRoom.Coords = new RoomCoord { X = room.Coords.X - 1, Y = room.Coords.Y };
                            return targetRoom;
                    }
                }
            }
        }

        private void GenerateChamber()
        {

        }
    }
}
