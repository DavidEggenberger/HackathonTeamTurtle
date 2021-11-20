﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class LearningPath
    {
        public Guid Id { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public TimeSpan EstimatedCompletionTime { get; set; }
        public string Name { get; set; }
        public List<LearningPathPillar> Pillars { get; set; }
    }
}