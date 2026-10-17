using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.AspNetCore.Mvc;
using ShiftMaster.DAL;

namespace ShiftMaster.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly RoleDal _dal;

        public RoleController(IConfiguration configuration)
        {
            string connStr = configuration.GetConnectionString("DefaultConnection")!;
            _dal = new RoleDal(connStr);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var result = _dal.GetAllRoles();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "שגיאה בשליפת התפקידים", error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] Role role)
        {
            try
            {
                bool success = _dal.AddRole(role);
                if (success)
                    return Ok(new { message = "התפקיד נוצר בהצלחה" });
                return BadRequest(new { message = "נכשל ביצירת התפקיד" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "שגיאה בהוספת התפקיד", error = ex.Message });
            }
        }
    }
}