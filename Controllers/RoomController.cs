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
        public IActionResult Create([FromForm] int userId)
        {
            var room = _roomManager.CreateRoom(userId);
            return Ok(new { roomCode = room.RoomCode });
        }

        [HttpPost("join")]
        public IActionResult Join([FromForm] int userId, [FromForm] string roomCode)
        {
            if (_roomManager.TryJoinRoom(roomCode, userId, out var room))
            {
                return Ok(new { roomCode = room.RoomCode });
            }
            return NotFound(new { message = "방이 존재하지 않습니다." });
        }
    }
}