using Domain;
using DTOs;
using Infrastructure.Persistance;
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
                EstimatedCompletionTime = path.EstimatedCompletionTime,
                LearningPathPillarDTOs = path.Pillars.Select(pillar => new LearningPathPillarDTO
                {
                    Name = pillar.Name
                }).ToList()
            }).ToList());
        }

        [HttpPost]
        public async Task<ActionResult> CreateLearningPath(LearningPathDTO learningPathDTO)
        {
            ApplicationUser applicationUser = await userManager.FindByIdAsync(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            applicationDbContext.LearningPaths.Add(new LearningPath 
            {
                ApplicationUser = applicationUser,
                Name = learningPathDTO.Name,
                EstimatedCompletionTime = learningPathDTO.EstimatedCompletionTime,
                Pillars = learningPathDTO.LearningPathPillarDTOs.Select(pillar => new LearningPathPillar
                {
                    Name = pillar.Name,
                    LearningRessources = pillar.LearningRessourcesDTOs.Select(ressource => new LearningRessource
                    {
                        Name = ressource.Name
                    }).ToList()
                }).ToList()
            });
            await applicationDbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
