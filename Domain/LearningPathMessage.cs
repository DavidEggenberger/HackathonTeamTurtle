using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class LearningPathMessage
    {
        public Guid Id { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public Guid LearningPathId { get; set; }
        public LearningPath LearningPath { get; set; }
        public string Message { get; set; }
    }
}
