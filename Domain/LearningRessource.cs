using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class LearningRessource
    {
        public Guid Id { get; set; }
        public Guid LearningPathId { get; set; }
        public LearningPath LearningPath { get; set; }
        public string Name { get; set; }
        public bool IsVideo { get; set; }
        public string URI { get; set; }
        public string Description { get; set; }
        public List<QuizQuestion> QuizQuestions { get; set; }
    }
}
