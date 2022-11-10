using kOS.Safe.Utilities;
using kOS.Safe.Encapsulation;
using kOS.Safe.Encapsulation.Suffixes;
using kOS.Suffixed;
using Astrogator;

namespace kOS.AddOns.kOSAstrogator
{
    using transfer;
    using info;
    using kOS.AddOns.kOSAstrogator.structure;

    /// <summary>
    /// kOS integration for Astrogator
    /// </summary>
    [kOSAddon("ASTROGATOR")]
    [KOSNomenclature("AstrogatorAddon")]
    public class KOSAstrogatorAddon : Addon
    {
        /// <summary>
        /// The Addon's name.
        /// </summary>
        public const string Name = "kOS-Astrogator";

        /// <summary>
        /// The class initializer
        /// </summary>
        /// <param name="shared">The shared objects</param>
        public KOSAstrogatorAddon(SharedObjects shared) : base(shared)
        {
            InitializeSuffixes();
        }

        /// <inheritdoc/>
        public override BooleanValue Available() => Util.IsModInstalled("Astrogator");

        private void InitializeSuffixes()
        {
            // info
            AddSuffix("help", new NoArgsVoidSuffix(() => Help.PrintHelp(shared)));
            AddSuffix("version", new NoArgsSuffix<StringValue>(() => Version.GetVersion()));

            // creating transfers
            AddSuffix("create", new OptionalArgsSuffix<Node>(CreateTransfer, new Safe.Encapsulation.Structure[] { null, BooleanValue.True }));
            AddSuffix("calculateBurns", new OneArgsSuffix<ListValue, BodyTarget>((dest) => Transfers.CalculateBurns(shared, dest)));
            AddSuffix("astrogation", new NoArgsSuffix<AstrogationModelStructure>(() => Astrogation.CreateAstrogationModel(shared)));

            // physics
            AddSuffix("deltaVToOrbit", new OneArgsSuffix<ScalarDoubleValue, BodyTarget>((body) => PhysicsTools.DeltaVToOrbit(body.Body)));
            AddSuffix("speedAtPeriapsis", new ThreeArgsSuffix<ScalarDoubleValue, BodyTarget, ScalarValue, ScalarValue>((body, apopapsis, periapsis) => PhysicsTools.SpeedAtPeriapsis(body.Body, apopapsis.GetDoubleValue(), periapsis.GetDoubleValue())));
            AddSuffix("speedAtApoapsis", new ThreeArgsSuffix<ScalarDoubleValue, BodyTarget, ScalarValue, ScalarValue>((body, apopapsis, periapsis) => PhysicsTools.SpeedAtApoapsis(body.Body, apopapsis.GetDoubleValue(), periapsis.GetDoubleValue())));
            AddSuffix("shipSpeedAtPeriapsis", new NoArgsSuffix<ScalarDoubleValue>(() => PhysicsTools.SpeedAtPeriapsis(shared.Vessel.mainBody, shared.Vessel.orbit.ApR, shared.Vessel.orbit.PeR)));
            AddSuffix("shipSpeedAtApoapsis", new NoArgsSuffix<ScalarDoubleValue>(() => PhysicsTools.SpeedAtApoapsis(shared.Vessel.mainBody, shared.Vessel.orbit.ApR, shared.Vessel.orbit.PeR)));

            // AN/DN calcs
            AddSuffix("timeOfAN", new TwoArgsSuffix<ScalarDoubleValue, BodyTarget, BodyTarget>((a, b) => a.Orbit.TimeOfAscendingNode(b.Orbit, Planetarium.GetUniversalTime())));
            AddSuffix("timeOfDN", new TwoArgsSuffix<ScalarDoubleValue, BodyTarget, BodyTarget>((a, b) => a.Orbit.TimeOfDescendingNode(b.Orbit, Planetarium.GetUniversalTime())));
            AddSuffix("timeOfShipAN", new NoArgsSuffix<ScalarDoubleValue>(() => shared.Vessel.orbit.TimeOfAscendingNode(shared.Vessel.orbit.referenceBody.orbit, Planetarium.GetUniversalTime())));
            AddSuffix("timeOfShipDN", new NoArgsSuffix<ScalarDoubleValue>(() => shared.Vessel.orbit.TimeOfDescendingNode(shared.Vessel.orbit.referenceBody.orbit, Planetarium.GetUniversalTime())));

        }

        #region suffix_functions
        private Node CreateTransfer(params Safe.Encapsulation.Structure[] args) => Transfers.CreateManeuverNodes(shared, args);
        #endregion

    }
}