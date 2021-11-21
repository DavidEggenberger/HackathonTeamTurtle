using Domain;
using DTOs;
using Infrastructure.Persistance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class LearningPathController : ControllerBase
    {
        private UserManager<ApplicationUser> userManager;
        private ApplicationDbContext applicationDbContext;
        public LearningPathController(UserManager<ApplicationUser> userManager, ApplicationDbContext applicationDbContext)
        {
            this.userManager = userManager;
            this.applicationDbContext = applicationDbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<LearningPathDTO>>> GetAllLearningPaths()
        {
            return Ok(applicationDbContext.LearningPaths.Select(path => new LearningPathDTO
            {
                Name = path.Name,
                Id = path.Id,
                EstimatedCompletionTimeInHrs = path.EstimatedCompletionTimeInHrs
            }).ToList());
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateLearningPath(LearningPathDTO learningPathDTO)
        {
            ApplicationUser applicationUser = await userManager.FindByIdAsync(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            applicationDbContext.LearningPaths.Add(new LearningPath 
            {
                ApplicationUser = applicationUser,
                Name = learningPathDTO.Name,
                EstimatedCompletionTimeInHrs = learningPathDTO.EstimatedCompletionTimeInHrs
            });
            await applicationDbContext.SaveChangesAsync();
            return Ok();
        }

        [Authorize]
        [HttpPost("{learningPathId}")]
        public async Task<ActionResult> EnrollInLearningPath(Guid learningPathId)
        {
            ApplicationUser applicationUser = applicationDbContext.Users.Single(user => user.Id == HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            LearningPath learningPath = applicationDbContext.LearningPaths.First(path => path.Id == learningPathId);
            applicationUser.EnrolledLearningPaths.Add(new LearningPathEnrollment
            {
                PillarLevel = 1,
                LearningRessourceLevel = 1,
                ApplicationUser = applicationUser,
                LearningPath = learningPath
            });
            await applicationDbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
