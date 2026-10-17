using DAL;
using Microsoft.AspNetCore.Mvc;

namespace API.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftController : ControllerBase
    {
        private readonly ShiftDal _shiftDal;

        public ShiftController(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection")!;
            _shiftDal = new ShiftDal(connectionString);
        }

        [HttpGet]
        public IActionResult GetAllShifts()
        {
            try
            {
                var shifts = _shiftDal.GetAllShifts();
                return Ok(shifts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "שגיאה בשליפת המשמרות", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetShiftById(int id)
        {
            try
            {
                var shift = _shiftDal.GetShiftById(id);
                if (shift == null)
                {
                    return NotFound(new { message = $"משמרת עם מזהה {id} לא נמצאה" });
                }
                return Ok(shift);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "שגיאה בשליפת המשמרת", error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult AddShift([FromBody] Shift shift)
        {
            if (shift == null)
            {
                return BadRequest(new { message = "נתוני המשמרת אינם תקינים" });
            }

            try
            {
                bool success = _shiftDal.AddShift(shift);
                if (success)
                {
                    return Ok(new { message = "המשמרת נוצרה בהצלחה!" });
                }
                return BadRequest(new { message = "יצירת המשמרת נכשלה" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "שגיאה בהוספת המשמרת", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateShift(int id, [FromBody] Shift shift)
        {
            if (shift == null || shift.ShiftID != id)
            {
                return BadRequest(new { message = "נתוני המשמרת אינם תואמים" });
            }

            try
            {
                bool success = _shiftDal.UpdateShift(shift);
                if (success)
                {
                    return Ok(new { message = "פרטי המשמרת עודכנו בהצלחה!" });
                }
                return NotFound(new { message = "המשמרת לא נמצאה לעדכון" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "שגיאה בעדכון המשמרת", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteShift(int id)
        {
            try
            {
                bool success = _shiftDal.DeleteShift(id);
                if (success)
                {
                    return Ok(new { message = "המשמרת הועברה למצב לא-פעיל בהצלחה!" });
                }
                return NotFound(new { message = "המשמרת לא נמצאה" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "שגיאה בביטול המשמרת", error = ex.Message });
            }
        }
    }
}