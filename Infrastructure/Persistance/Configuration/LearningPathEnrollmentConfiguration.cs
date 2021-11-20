using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistance.Configuration
{
    public class LearningPathEnrollmentConfiguration : IEntityTypeConfiguration<LearningPathEnrollment>
    {
        public void Configure(EntityTypeBuilder<LearningPathEnrollment> builder)
        {
            builder.HasKey(learningPath => new
            {
                learningPath.ApplicationUserId,
                learningPath.LearningPathId
            });
        }
    }
}
