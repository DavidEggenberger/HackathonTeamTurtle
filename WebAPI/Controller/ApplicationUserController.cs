using DTOs;
using Infrastructure.Persistance;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private ApplicationDbContext applicationDbContext;
        public ApplicationUserController(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<ApplicationUserDTO>>> GetAllActiveUsers()
        {
            return Ok(applicationDbContext.Users.Where(user => user.IsOnline && !user.PrivateProfile).Select(user => new ApplicationUserDTO 
            { 
                IsOnline = user.IsOnline,
                Motto = user.Motto
            }));
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<List<ApplicationUserDTO>>> GetAllActiveUsersPerLearningPath(Guid id)
        {
            return Ok(applicationDbContext.Users.Where(user => user.IsOnline && !user.PrivateProfile && user.CurrentlyLearningId == id).Select(user => new ApplicationUserDTO
            {
                UserName = user.UserName,
                IsOnline = user.IsOnline,
                Motto = user.Motto
            }));
        }
    }
}
