using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.LearningRessources
{
    public class BlogExtern : LearningRessource
    {
        public string Title { get; set; }
        public string Uri { get; set; }
        public string Summary { get; set; }
    }
}
