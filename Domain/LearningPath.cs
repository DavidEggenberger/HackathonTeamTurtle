using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class LearningPath
    {
        public Guid Id { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public int EstimatedCompletionTimeInHrs { get; set; }
        public string Name { get; set; }
        public List<LearningRessource> LearningRessources { get; set; }
        public List<LearningPathEnrollment> EnrolledUsers { get; set; }
        public List<LearningPathMessage> Messages { get; set; }
    }
}
