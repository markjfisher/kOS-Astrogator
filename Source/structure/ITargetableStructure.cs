using Astrogator;
using kOS.Safe.Encapsulation;
using kOS.Safe.Encapsulation.Suffixes;
using kOS.Safe.Utilities;
using kOS.Suffixed;

namespace kOS.AddOns.kOSAstrogator.structure
{
    /// <summary>
    /// A structure wrapper for ITargetable
    /// </summary>
    [KOSNomenclature("AstrogatorITargetable")]
    public class ITargetableStructure : Structure
    {
        /// <summary>
        /// The target to wrap
        /// </summary>
        public readonly ITargetable target;
        private readonly SharedObjects shared;

        private ITargetableStructure(ITargetable target, SharedObjects shared)
        {
            this.target = target;
            this.shared = shared;
            InitializeSuffixes();
        }

        /// <summary>
        /// Creates a wrapper around ITargetable
        /// </summary>
        /// <param name="target">The target to wrap</param>
        /// <param name="shared">Game shared data</param>
        /// <returns></returns>
        public static ITargetableStructure Create(ITargetable target, SharedObjects shared) => new ITargetableStructure(target, shared);

        private void InitializeSuffixes()
        {
            AddSuffix("GetObtVelocity", new Suffix<Vector>(() => new Vector(target.GetObtVelocity())));
            AddSuffix("GetSrfVelocity", new Suffix<Vector>(() => new Vector(target.GetSrfVelocity())));
            AddSuffix("GetFwdVector", new Suffix<Vector>(() => new Vector(target.GetFwdVector())));
            AddSuffix("GetName", new Suffix<StringValue>(() => target.GetName()));
            AddSuffix("GetDisplayName", new Suffix<StringValue>(() => target.GetDisplayName()));
            AddSuffix("GetOrbit", new Suffix<OrbitInfo>(() => new OrbitInfo(target.GetOrbit(), shared)));
            AddSuffix("GetActiveTargetable", new Suffix<BooleanValue>(() => target.GetActiveTargetable()));
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            if (target == null) return "ITargetable(null)";
            return string.Format("ITargetable(name: {0}, orbit: {1})", target.GetName(), new OrbitInfo(target.GetOrbit(), shared));
        }
    }
}
