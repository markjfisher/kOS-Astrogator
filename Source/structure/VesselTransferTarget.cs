using kOS.Safe.Encapsulation.Suffixes;
using kOS.Safe.Encapsulation;
using kOS.Suffixed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kOS.Safe.Exceptions;
using kOS.Safe.Utilities;

namespace kOS.AddOns.kOSAstrogator.structure
{
    /// <summary>
    /// A TransferTarget representing a BodyTarget
    /// </summary>
    [KOSNomenclature("AstrogatorVesselTransferTarget")]
    public class VesselTransferTarget : TransferTarget
    {
        /// <summary>
        /// Create a TransferTarget representing a VesselTarget
        /// </summary>
        /// <param name="target"></param>
        /// <param name="shared"></param>
        public VesselTransferTarget(ITargetable target, SharedObjects shared) : base(target, shared) { }

        /// <summary>
        /// Creates a VesselTransferTarget
        /// </summary>
        /// <param name="target">The target to wrap</param>
        /// <param name="shared">Game shared data</param>
        /// <returns></returns>
        public static VesselTransferTarget Create(ITargetable target, SharedObjects shared) => new VesselTransferTarget(target, shared);


        /// <summary>
        /// Initializes the suffix for a VesselTransferTarget
        /// </summary>
        protected override void InitializeSuffixes()
        {
            AddSuffix("name", new Suffix<StringValue>(() => target.GetName()));
            AddSuffix("displayName", new Suffix<StringValue>(() => target.GetDisplayName()));
            AddSuffix("isVessel", new Suffix<BooleanValue>(() => BooleanValue.True));
            AddSuffix("vessel", new Suffix<VesselTarget>(() =>
            {
                return VesselTarget.CreateOrGetExisting(target.GetVessel(), shared);
            }));
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            if (target == null) return "VesselTransferTarget(none)";
            return string.Format("VesselTransferTarget(\"{0}\")", target.GetName());
        }

    }
}
