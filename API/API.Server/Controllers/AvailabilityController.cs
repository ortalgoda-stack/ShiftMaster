using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.AspNetCore.Mvc;
using ShiftMaster.DAL;

namespace ShiftMaster.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AvailabilityController : ControllerBase
    {
        private readonly AvailabilityDal _dal;

        public AvailabilityController(IConfiguration configuration)
        {
            string connStr = configuration.GetConnectionString("DefaultConnection")!;
            _dal = new AvailabilityDal(connStr);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var result = _dal.GetAllAvailabilities();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "שגיאה בשליפת הזמינויות", error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] Availability availability)
        {
            try
            {
                bool success = _dal.AddAvailability(availability);
                if (success)
                    return Ok(new { message = "הזמינות נשמרה בהצלחה" });
                return BadRequest(new { message = "נכשל בשמירת הזמינות" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "שגיאה בהוספת זמינות", error = ex.Message });
            }
        }
    }
}