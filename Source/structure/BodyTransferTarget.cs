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
    [KOSNomenclature("AstrogatorBodyTransferTarget")]
    public class BodyTransferTarget : TransferTarget
    {
        /// <summary>
        /// Create a TransferTarget representing a BodyTarget
        /// </summary>
        /// <param name="target"></param>
        /// <param name="shared"></param>
        public BodyTransferTarget(ITargetable target, SharedObjects shared) : base(target, shared) {}

        /// <summary>
        /// Creates a BodyTransferTarget
        /// </summary>
        /// <param name="target">The target to wrap</param>
        /// <param name="shared">Game shared data</param>
        /// <returns></returns>
        public static BodyTransferTarget Create(ITargetable target, SharedObjects shared) => new BodyTransferTarget(target, shared);

        /// <summary>
        /// Initializes the suffix for a BodyTransferTarget
        /// </summary>
        protected override void InitializeSuffixes()
        {
            AddSuffix("name", new Suffix<StringValue>(() => target.GetName()));
            AddSuffix("displayName", new Suffix<StringValue>(() => target.GetDisplayName()));
            AddSuffix("isVessel", new Suffix<BooleanValue>(() => BooleanValue.False));
            AddSuffix("body", new Suffix<BodyTarget>(() =>
            {
                return BodyTarget.CreateOrGetExisting(target.GetName(), shared);
            }));
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            if (target == null) return "BodyTransferTarget(none)";
            return string.Format("BodyTransferTarget(\"{0}\")", target.GetName());
        }

    }
}
