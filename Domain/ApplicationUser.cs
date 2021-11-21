using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class ApplicationUser : IdentityUser
    {
        public LearningPath CurrentlyLearning { get; set; }
        public int TabsOpen { get; set; }
        public bool PrivateProfile { get; set; }
        public bool IsOnline { get; set; }
        public string Motto { get; set; }
        public List<LearningPath> CreatedLearningPaths { get; set; }
        public List<LearningPathEnrollment> EnrolledLearningPaths { get; set; }
        public List<LearningPathMessage> SentLearningPathMessages { get; set; }
    }
}
