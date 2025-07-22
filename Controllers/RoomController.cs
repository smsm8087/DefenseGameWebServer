using DefenseGameWebServer.Manager;
using Microsoft.AspNetCore.Mvc;

namespace DefenseGameApiServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : ControllerBase
    {
        private static readonly RoomManager _roomManager = new(); // 싱글톤처럼 사용

        [HttpPost("create")]
        public IActionResult Create([FromForm] string userId)
        {
            var room = _roomManager.CreateRoom(userId);
            return Ok(new { roomCode = room.RoomCode, hostId = room.HostUserId });
        }

        [HttpPost("join")]
        public IActionResult Join([FromForm] string userId, [FromForm] string roomCode)
        {
            if (_roomManager.TryJoinRoom(roomCode, userId, out var room))
            {
                return Ok(new { roomCode = room.RoomCode, hostId = room.HostUserId });
            }
            return NotFound(new { message = "방이 존재하지 않습니다." });
        }
        [HttpPost("status")]
        public IActionResult GetRoomPaticipantCount([FromForm] string roomCode)
        {
            //게임시작 버튼 누른거임
            RoomSession room = _roomManager.GetRoom(roomCode);
            if(room == null)
            {
                return NotFound(new { message = "방이 존재하지 않습니다." });
            }

            List<string> participants = _roomManager.GetParticipants(roomCode);
            if (participants.Count <= 0)
            {
                return NotFound(new { message = "방이 존재하지 않습니다." });
            }

            // 게임 시작 상태로 변경
            room.isStarted = true; 
            return Ok(new { playerIds = participants });
        }
    }
}