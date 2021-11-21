using System.Collections.Generic;

namespace DTOs
{
    public class LearningRessourceDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string LinkURI { get; set; }
        public bool IsVideo { get; set; }
        public List<QuizQuestionDTO> QuizQuestions { get; set; }
    }
}