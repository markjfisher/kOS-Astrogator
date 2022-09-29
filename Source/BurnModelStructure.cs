using Astrogator;
using kOS.Safe.Encapsulation;
using kOS.Safe.Encapsulation.Suffixes;
using kOS.Safe.Utilities;
using kOS.Suffixed;

namespace kOS.AddOns.kOSAstrogator
{
    /// <summary>
    /// A structure wrapper for BurnModel to allow kOS to interact with.
    /// </summary>
    [KOSNomenclature("BurnModel")]
    public class BurnModelStructure : Structure
    {
        private readonly BurnModel model;
        private readonly SharedObjects shared;

        private BurnModelStructure(BurnModel model, SharedObjects shared)
        {
            this.model = model;
            this.shared = shared;
            InitializeSuffixes();
        }

        /// <summary>
        /// Creates a wrapper for BurnModel
        /// </summary>
        /// <param name="model">The BurnModel to wrap</param>
        /// <param name="shared">The shared object</param>
        /// <returns>A BurnModel Structure usable in kOS</returns>
        public static BurnModelStructure Create(BurnModel model, SharedObjects shared)
        {
            BurnModelStructure burnModelStructure = new BurnModelStructure(model, shared);
            return burnModelStructure;
        }

        private void InitializeSuffixes()
        {
            AddSuffix("ATTIME", new Suffix<ScalarDoubleValue>(() => model.atTime));
            AddSuffix("PROGRADE", new Suffix<ScalarDoubleValue>(() => model.prograde));
            AddSuffix("NORMAL", new Suffix<ScalarDoubleValue>(() => model.normal));
            AddSuffix("RADIAL", new Suffix<ScalarDoubleValue>(() => model.radial));
            AddSuffix("TOTALDV", new Suffix<ScalarDoubleValue>(() => model.totalDeltaV));
            AddSuffix("TONODE", new NoArgsSuffix<Node>(CreateNode, "Generate a visible maneuver using this object's parameters"));
            AddSuffix("DURATION", new NoArgsSuffix<ScalarDoubleValue>(CalculateDuration, "Calculate the burn time for a given vessel and delta V amount"));
        }

        private Node CreateNode()
        {
            // Immediately release the node generated in the underlying model, we probably won't want it hanging around.
            Node node = Node.FromExisting(shared.Vessel, model.ToActiveManeuver(), shared);
            model.NodeDeleted();
            return node;
        }

        private ScalarDoubleValue CalculateDuration()
        {
            // kOS doesn't do NaN/null/infinity, so convert to "special" values.
            double? duration = model.Duration(shared.Vessel.VesselDeltaV);
            if (!duration.HasValue) return -1.0;                      // Not Ready
            if (duration == double.NaN) return -2.0;                  // Can't burn at all
            if (duration == double.PositiveInfinity) return -3.0;     // Not enough Fuel
            return duration.Value;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            if (model == null) return "BurnModel(null)";
            return string.Format("BurnModel(pro: {0}, norm: {1}, rad: {2}, dv: {3}, dur: {4}, at: {5})",
                model.prograde,
                model.normal,
                model.radial,
                model.totalDeltaV,
                CalculateDuration().GetDoubleValue(),
                model.atTime
            );

        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            BurnModelStructure bms = (BurnModelStructure)obj;
            return bms.model.atTime == model.atTime &&
                bms.model.prograde == model.prograde &&
                bms.model.normal == model.normal &&
                bms.model.radial == model.radial &&
                bms.model.totalDeltaV == model.totalDeltaV;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
