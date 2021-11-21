using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DTOs
{
    public class LearningPathDTO
    {
        public string UserId { get; set; }
        public Guid Id { get; set; }
        [MinLength(3, ErrorMessage = "Minimum Length 3 characters")]
        public string Name { get; set; }
        [Range(1, 99, ErrorMessage = "Must be between 1-99hrs")]
        public int EstimatedCompletionTimeInHrs { get; set; }
        public int EnrolledUsersCount { get; set; }
        public string Genre { get; set; }
        public bool RetrievingUserEnrolled { get; set; }
        public List<LearningRessourceDTO> LearningRessourceDTOs { get; set; }
    }
}
