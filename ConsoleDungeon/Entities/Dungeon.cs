﻿using ConsoleDungeon.Entities.Rooms;
using ConsoleDungeon.Entities.Rooms.Chambers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleDungeon.Entities
{
    public class Dungeon
    {
        private const int MinBranchCount = 3;
        private const int MaxBranchCount = 7;
        private const int MinBranchLength = 4;
        private const int MaxBranchLength = 7;

        private static readonly Random Random = new Random();

        public List<Room> Rooms { get; set; } = new List<Room>();
        private List<Room> Chambers => Rooms.Where(r => r is Chamber).ToList();
        private HashSet<RoomCoords> OccupiedCoords => Rooms.Select(r => r.Coords).ToHashSet();

        public static Dungeon Generate()
        {
            var branchCount = Random.Next(MinBranchCount, MaxBranchCount + 1);

            var dungeon = new Dungeon();
            dungeon.CreateInitialChamber();

            for (int i = 0; i < branchCount; i++)
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
            var branchLength = Random.Next(MinBranchLength, MaxBranchLength + 1);
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

        private bool GenerateChamber()
        {
            var endOfHall = Rooms.FirstOrDefault(r => r is Hallway && r.TotalEmptySides == 3);

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
                    ConvertEndOfHall(endOfHall, directionalId, newCoords);
                    return true;
                }
            }

            return false;
        }

        private void ConvertEndOfHall(Room endOfHall, int directionalId, RoomCoords newCoords)
        {
            var neighborCell = Rooms.FirstOrDefault(r => r.Coords.Equals(newCoords));

            var newChamber = new Chamber
            {
                AdjacentNorth = endOfHall.AdjacentNorth,
                AdjacentEast = endOfHall.AdjacentEast,
                AdjacentSouth = endOfHall.AdjacentSouth,
                AdjacentWest = endOfHall.AdjacentWest,
                Coords = endOfHall.Coords
            };

            ConnectRoom(newChamber, neighborCell, directionalId);

            var hallIndex = Rooms.IndexOf(endOfHall);
            Rooms[hallIndex] = newChamber;
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

        private void PopulateRooms()
        {
            foreach(var room in Rooms)
            {
                switch (room)
                {
                    case Hallway hall:
                    case StartRoom startChamber:
                    default:
                        break;
                }
            }
        }
    }
}
