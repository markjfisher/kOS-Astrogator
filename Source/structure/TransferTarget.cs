using kOS.Safe.Encapsulation;
using kOS.Safe.Utilities;

namespace kOS.AddOns.kOSAstrogator.structure
{
    /// <summary>
    /// TransferTarget is base class for BodyTransferTarget and VesselTransferTarget
    /// </summary>
    [KOSNomenclature("AstrogatorTransferTarget")]
    public abstract class TransferTarget : Structure
    {
        /// <summary>
        /// Adds suffixes to the instance type.
        /// </summary>
        protected abstract void InitializeSuffixes();

        /// <summary>
        /// The target to wrap
        /// </summary>
        public readonly ITargetable target;
        /// <summary>
        /// The shared objects given to us.
        /// </summary>
        protected readonly SharedObjects shared;

        /// <summary>
        /// Base initialiser for transfer targets.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="shared"></param>
        protected TransferTarget(ITargetable target, SharedObjects shared)
        {
            this.target = target;
            this.shared = shared;
            InitializeSuffixes();
        }
    }
}
