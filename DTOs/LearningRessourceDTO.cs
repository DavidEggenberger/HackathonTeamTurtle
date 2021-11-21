using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DTOs
{
    public class LearningRessourceDTO
    {
        public string Name { get; set; }
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Link is required")]
        public string LinkURI { get; set; }
        public bool IsVideo { get; set; }
        public List<QuizQuestionDTO> QuizQuestions { get; set; }
    }
}