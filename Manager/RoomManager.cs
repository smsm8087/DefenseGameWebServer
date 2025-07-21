using System;
using System.Collections.Generic;
using System.Linq;

namespace DefenseGameWebServer.Manager
{
    public class RoomSession
    {
        public string RoomCode;
        public int HostUserId;
        public List<int> Participants = new();
        public DateTime CreatedAt = DateTime.UtcNow;
    }

    public class RoomManager
    {
        private readonly Dictionary<string, RoomSession> _rooms = new();
        private readonly Random _rand = new();

        public RoomSession CreateRoom(int hostUserId)
        {
            string code;
            do
            {
                code = GenerateRoomCode();
            } while (_rooms.ContainsKey(code));

            var room = new RoomSession
            {
                RoomCode = code,
                HostUserId = hostUserId
            };
            room.Participants.Add(hostUserId);
            _rooms[code] = room;

            Console.WriteLine($"[Room Created] {code} by user {hostUserId}");
            return room;
        }

        public bool TryJoinRoom(string code, int userId, out RoomSession room)
        {
            if (_rooms.TryGetValue(code, out room))
            {
                room.Participants.Add(userId);
                Console.WriteLine($"[Room Join] user {userId} joined {code}");
                return true;
            }
            return false;
        }

        private string GenerateRoomCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 6)
                .Select(c => c[_rand.Next(c.Length)]).ToArray());
        }
    }
}