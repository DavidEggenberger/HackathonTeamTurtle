using Domain;
using DTOs;
using Infrastructure.Persistance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            ApplicationUser applicationUser = await userManager.FindByIdAsync(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            return Ok(applicationDbContext.LearningPaths.Include(s => s.EnrolledUsers).Include(s => s.LearningRessources).ThenInclude(s => s.QuizQuestions).Select(path => new LearningPathDTO
            {
                Genre = path.Genre,
                Name = path.Name,
                Id = path.Id,
                RetrievingUserEnrolled = applicationUser != null ? path.EnrolledUsers.Any(x => x.ApplicationUserId == applicationUser.Id) : false,
                UserId = path.ApplicationUserId,
                EstimatedCompletionTimeInHrs = path.EstimatedCompletionTimeInHrs,
                EnrolledUsersCount = path.EnrolledUsers.Count,
                LearningRessourceDTOs = path.LearningRessources.Select(ressource => new LearningRessourceDTO
                {
                    Description = ressource.Description,
                    IsVideo = ressource.IsVideo,
                    LinkURI = ressource.URI,
                    Name = ressource.Name
                }).ToList()
            }).ToList());
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<LearningPathDTO>> GetAllLearningPathById(Guid id)
        {
            ApplicationUser applicationUser = await userManager.FindByIdAsync(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            return Ok(applicationDbContext.LearningPaths.Include(s => s.EnrolledUsers).Include(s => s.LearningRessources).ThenInclude(s => s.QuizQuestions).Where(s => s.Id == id).Select(path => new LearningPathDTO
            {
                Genre = path.Genre,
                Name = path.Name,
                Id = path.Id,
                RetrievingUserEnrolled = applicationUser != null ? path.EnrolledUsers.Any(x => x.ApplicationUserId == applicationUser.Id) : false,
                UserId = path.ApplicationUserId,
                EstimatedCompletionTimeInHrs = path.EstimatedCompletionTimeInHrs,
                EnrolledUsersCount = path.EnrolledUsers.Count,
                LearningRessourceDTOs = path.LearningRessources.Select(ressource => new LearningRessourceDTO
                {
                    Description = ressource.Description,
                    IsVideo = ressource.IsVideo,
                    LinkURI = ressource.URI,
                    Name = ressource.Name
                }).ToList()
            }).First());
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateLearningPath(LearningPathDTO learningPathDTO)
        {
            ApplicationUser applicationUser = await userManager.FindByIdAsync(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            applicationDbContext.LearningPaths.Add(new LearningPath 
            {
                Genre = learningPathDTO.Genre,
                ApplicationUser = applicationUser,
                Name = learningPathDTO.Name,
                EnrolledUsers = new List<LearningPathEnrollment>
                {
                    new LearningPathEnrollment
                    {
                        ApplicationUser = applicationUser
                    }
                },
                EstimatedCompletionTimeInHrs = learningPathDTO.EstimatedCompletionTimeInHrs,
                LearningRessources = learningPathDTO.LearningRessourceDTOs.Select(ressource => new LearningRessource
                {
                    Description = ressource.Description,
                    IsVideo = ressource.IsVideo,
                    Name = ressource.Name,
                    URI = ressource.LinkURI,
                    QuizQuestions = ressource.QuizQuestions?.Select(s => new QuizQuestion { Question = s.Question, CorrectAnswer = s.Answert}).ToList()
                }).ToList()
            });
            await applicationDbContext.SaveChangesAsync();
            return Ok();
        }

        [Authorize]
        [HttpPost("{learningPathId}")]
        public async Task<ActionResult> EnrollInLearningPath(Guid learningPathId)
        {
            ApplicationUser applicationUser = applicationDbContext.Users.Include(s => s.EnrolledLearningPaths).Single(user => user.Id == HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            LearningPath learningPath = applicationDbContext.LearningPaths.First(path => path.Id == learningPathId);
            applicationUser.EnrolledLearningPaths.Add(new LearningPathEnrollment
            {
                PillarLevel = 1,
                LearningRessourceLevel = 1,
                ApplicationUser = applicationUser,
                LearningPath = learningPath,
            });
            await applicationDbContext.SaveChangesAsync();
            return Ok();
        }

        [Authorize]
        [HttpPost("unenroll/{learningPathId}")]
        public async Task<ActionResult> UnEnrollInLearningPath(Guid learningPathId)
        {
            ApplicationUser applicationUser = applicationDbContext.Users.Include(s => s.EnrolledLearningPaths).Single(user => user.Id == HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            LearningPath learningPath = applicationDbContext.LearningPaths.First(path => path.Id == learningPathId);
            LearningPathEnrollment lp = applicationUser.EnrolledLearningPaths.Where(s => s.LearningPathId == learningPath.Id).First();
            applicationUser.EnrolledLearningPaths.Remove(lp);
            await applicationDbContext.SaveChangesAsync();
            return Ok();
        }

        [Authorize]
        [HttpDelete("{learningPathId:guid}")]
        public async Task<ActionResult> DeleteLearningPath(Guid learningPathId)
        {
            ApplicationUser applicationUser = applicationDbContext.Users.Single(user => user.Id == HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            LearningPath learningPath = applicationDbContext.LearningPaths.First(path => path.Id == learningPathId);
            if(learningPath.ApplicationUserId == applicationUser.Id)
            {
                applicationDbContext.Remove(learningPath);
                await applicationDbContext.SaveChangesAsync();
            }
            return Ok();
        }
    }
}
