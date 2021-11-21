using Blazor.Diagrams.Core.Models;
using DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebClient.Diagrams.Nodes
{
    public class LearningRessourceNodeModel : NodeModel
    {
        public LearningRessourceNodeModel(Blazor.Diagrams.Core.Geometry.Point position) : base(position)
        {

        }
        public LearningRessourceDTO LearningRessourceDTO { get; set; }
    }
}
