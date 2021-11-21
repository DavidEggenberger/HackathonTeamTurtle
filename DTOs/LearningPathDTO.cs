using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs
{
    public class LearningPathDTO
    {
        public string UserId { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int EstimatedCompletionTimeInHrs { get; set; }
        public int EnrolledUsersCount { get; set; }
        public string Genre { get; set; }
        public bool RetrievingUserEnrolled { get; set; }
        public List<LearningRessourceDTO> LearningRessourceDTOs { get; set; }
    }
}
