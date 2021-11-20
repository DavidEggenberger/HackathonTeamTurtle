﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsOnline { get; set; }
        public string Motto { get; set; }
        public List<LearningPath> CreatedLearningPaths { get; set; }
    }
}