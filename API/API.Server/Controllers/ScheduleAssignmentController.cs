using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.AspNetCore.Mvc;
using ShiftMaster.DAL;

namespace ShiftMaster.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScheduleAssignmentController : ControllerBase
    {
        private readonly ScheduleAssignmentDal _dal;

        public ScheduleAssignmentController(IConfiguration configuration)
        {
            string connStr = configuration.GetConnectionString("DefaultConnection")!;
            _dal = new ScheduleAssignmentDal(connStr);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var result = _dal.GetAllAssignments();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "שגיאה בשליפת השיבוצים", error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] ScheduleAssignment assignment)
        {
            try
            {
                bool success = _dal.AddAssignment(assignment);
                if (success)
                    return Ok(new { message = "השיבוץ נוצר בהצלחה" });
                return BadRequest(new { message = "נכשל ביצירת השיבוץ" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "שגיאה בהוספת שיבוץ", error = ex.Message });
            }
        }
    }
}