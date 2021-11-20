using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class LearningPathPillar
    {
        public Guid Id { get; set; }
        public Guid LearningPathId { get; set; }
        public LearningPath LearningPath { get; set; }
        public string Name { get; set; }
        public List<LearningRessource> LearningRessources { get; set; }
    }
}
