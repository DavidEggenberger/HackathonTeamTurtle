using System;
using System.Collections.Generic;

namespace DTOs
{
    public class LearningPathPillarDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<LearningRessourceDTO> LearningRessourcesDTOs { get; set; }
    }
}