using DAL;
using Microsoft.AspNetCore.Mvc;

namespace API.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class userController : ControllerBase
    {
        private readonly userDal _userDal;

        public userController(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection")!;
            _userDal = new userDal(connectionString);
        }

        // GET: api/user
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            try
            {
                var users = _userDal.GetAllUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "שגיאה בשליפת המשתמשים", error = ex.Message });
            }
        }

        // GET: api/user/5
        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            try
            {
                var user = _userDal.GetUserById(id);
                if (user == null)
                {
                    return NotFound(new { message = $"משתמש עם מזהה {id} לא נמצא" });
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "שגיאה בשליפת המשתמש", error = ex.Message });
            }
        }

        // POST: api/user
        [HttpPost]
        public IActionResult AddUser([FromBody] DAL.User user)
        {
            if (user == null)
            {
                return BadRequest(new { message = "נתוני המשתמש אינם תקינים" });
            }

            try
            {
                bool success = _userDal.AddUser(user);
                if (success)
                {
                    return Ok(new { message = "המשתמש נוצר בהצלחה!" });
                }
                return BadRequest(new { message = "יצירת המשתמש נכשלה" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "שגיאה בהוספת המשתמש", error = ex.Message });
            }
        }

        // PUT: api/user/5
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] DAL.User user)
        {
            if (user == null || user.UserID != id)
            {
                return BadRequest(new { message = "נתוני המשתמש או המזהה אינם תואמים" });
            }

            try
            {
                bool success = _userDal.UpdateUser(user);
                if (success)
                {
                    return Ok(new { message = "פרטי המשתמש עודכנו בהצלחה!" });
                }
                return NotFound(new { message = "המשתמש לא נמצא לעדכון" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "שגיאה בעדכון המשתמש", error = ex.Message });
            }
        }

        // DELETE: api/user/5
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                bool success = _userDal.DeleteUser(id);
                if (success)
                {
                    return Ok(new { message = "המשתמש הועבר למצב לא-פעיל בהצלחה!" });
                }
                return NotFound(new { message = "המשתמש לא נמצא" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "שגיאה בשינוי סטטוס המשתמש", error = ex.Message });
            }
        }
    }
}