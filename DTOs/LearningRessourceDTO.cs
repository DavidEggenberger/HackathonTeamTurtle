using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DTOs
{
    public class LearningRessourceDTO
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Link is required")]
        public string LinkURI { get; set; }
        public bool IsVideo { get; set; }
        public List<QuizQuestionDTO> QuizQuestions { get; set; }
    }
}