using Astrogator;
using kOS.AddOns.kOSAstrogator.structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kOS.AddOns.kOSAstrogator.transfer
{
    /// <summary>
    /// Provides interactions with AstrogationModel
    /// </summary>
    public static class Astrogation
    {
        /// <summary>
        /// Create a wrapped AstrogationModel for kOS.
        /// </summary>
        public static AstrogationModelStructure CreateAstrogationModel(SharedObjects shared)
        {
            // Start from ship, could extend this to any ITargetable as an input
            AstrogationModel model = new AstrogationModel(shared.Vessel);
            return AstrogationModelStructure.Create(model, shared);
        }
    }
}
