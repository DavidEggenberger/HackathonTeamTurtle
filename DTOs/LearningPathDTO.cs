using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs
{
    public class LearningPathDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public TimeSpan EstimatedCompletionTime { get; set; }
        public int EnrolledUsersCount { get; set; }
        public List<LearningPathPillarDTO> LearningPathPillarDTOs { get; set; }
    }
}
