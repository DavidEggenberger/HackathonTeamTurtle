using Blazor.Diagrams.Core.Models;
using DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebClient.Diagrams.Nodes
{
    public class LearningPathStartNode : NodeModel
    {
        public LearningRessourceDTO LearningRessourceDTO { get; set; }
        public LearningPathStartNode(Blazor.Diagrams.Core.Geometry.Point position = null) : base(position)
        {
            AddPort(PortAlignment.Bottom);
        }
    }
}
