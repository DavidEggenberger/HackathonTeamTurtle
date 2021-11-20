using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class LearningPathEnrollment
    {
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public Guid LearningPathId { get; set; }
        public LearningPath LearningPath { get; set; }
        public bool CompletedLearningPath { get; set; }
        public int PillarLevel { get; set; }
        public int LearningRessourceLevel { get; set; }
    }
}
