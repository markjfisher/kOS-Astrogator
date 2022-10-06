using Astrogator;
using kOS.Safe.Encapsulation;
using kOS.Safe.Encapsulation.Suffixes;
using kOS.Safe.Utilities;
using kOS.Suffixed;
using System;

namespace kOS.AddOns.kOSAstrogator.structure
{
    /// <summary>
    /// A structure wrapper for BurnModel to allow kOS to interact with.
    /// </summary>
    [KOSNomenclature("AstrogationBurnModel")]
    public class BurnModelStructure : Structure
    {
        private readonly BurnModel model;
        private readonly SharedObjects shared;

        private BurnModelStructure(BurnModel model, SharedObjects shared)
        {
            this.model = model ?? new BurnModel(0, 0);
            this.shared = shared;
            InitializeSuffixes();
        }

        /// <summary>
        /// Creates a wrapper for BurnModel
        /// </summary>
        /// <param name="model">The BurnModel to wrap</param>
        /// <param name="shared">The shared object</param>
        /// <returns>A BurnModel Structure usable in kOS</returns>
        public static BurnModelStructure Create(BurnModel model, SharedObjects shared) => new BurnModelStructure(model, shared);

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
            
            if (!duration.HasValue) return -1.0;                          // Not Ready
            if (Double.IsNaN(duration.Value)) return -2.0;                // Can't burn at all
            if (Double.IsPositiveInfinity(duration.Value)) return -3.0;   // Not enough Fuel
            return duration.Value;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            if (model == null) return "BurnModel(null)";
            return string.Format("BurnModel(attime: {0}, prograde: {1}, normal: {2}, radial: {3}, totaldv: {4}, duration: {5})",
                model.atTime,
                model.prograde,
                model.normal,
                model.radial,
                model.totalDeltaV,
                CalculateDuration().GetDoubleValue()
            );

        }
    }
}
