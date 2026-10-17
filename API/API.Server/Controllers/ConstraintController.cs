using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.AspNetCore.Mvc;
using ShiftMaster.DAL;

namespace ShiftMaster.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConstraintController : ControllerBase
    {
        private readonly ConstraintDal _dal;

        public ConstraintController(IConfiguration configuration)
        {
            string connStr = configuration.GetConnectionString("DefaultConnection")!;
            _dal = new ConstraintDal(connStr);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var result = _dal.GetAllConstraints();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "שגיאה בשליפת האילוצים", error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] ConstraintModel constraint)
        {
            try
            {
                bool success = _dal.AddConstraint(constraint);
                if (success)
                    return Ok(new { message = "האילוץ נוצר בהצלחה" });
                return BadRequest(new { message = "נכשל ביצירת האילוץ" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "שגיאה בהוספת האילוץ", error = ex.Message });
            }
        }
    }
}