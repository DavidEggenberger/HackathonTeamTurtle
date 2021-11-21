using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs
{
    public class MessageDTO
    {
        public Guid LearningPathId { get; set; }
        public string SenderId { get; set; }
        public string Message { get; set; }
        public string UserName { get; set; }
    }
}
