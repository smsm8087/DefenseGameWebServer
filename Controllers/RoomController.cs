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
            return Ok(new { roomCode = room.RoomCode });
        }

        [HttpPost("join")]
        public IActionResult Join([FromForm] string userId, [FromForm] string roomCode)
        {
            if (_roomManager.TryJoinRoom(roomCode, userId, out var room))
            {
                return Ok(new { roomCode = room.RoomCode });
            }
            return NotFound(new { message = "방이 존재하지 않습니다." });
        }
        [HttpPost("status")]
        public IActionResult GetRoomStatus([FromForm] string roomCode)
        {
            int participantCount = _roomManager.GetParticipantsCount(roomCode);
            if (participantCount < 0)
            {
                return NotFound(new { message = "방이 존재하지 않습니다." });
            }
            return Ok(new { playerCount = participantCount });
        }
    }
}