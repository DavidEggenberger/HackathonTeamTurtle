﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazor.Diagrams.Core.Models;

namespace WebClient.Diagrams.Ports
{
    public class ColumnPort : PortModel
    {
        public ColumnPort(NodeModel parent, PortAlignment alignment = PortAlignment.Bottom)
            : base(parent, alignment, null, null)
        {

        }

        public override bool CanAttachTo(PortModel port)
        {
            // Avoid attaching to self port/node
            if (!base.CanAttachTo(port))
                return false;

            return true;
        }
    }
}
