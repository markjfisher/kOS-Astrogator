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
            AddSuffix("calculateBurns", new OneArgsSuffix<ListValue, BodyTarget>(CalculateBurns));
            AddSuffix("astrogation", new NoArgsSuffix<AstrogationModelStructure>(() => Astrogation.CreateAstrogationModel(shared)));

            // physics
            AddSuffix("deltaVToOrbit", new OneArgsSuffix<ScalarDoubleValue, BodyTarget>(DeltaVToOrbit));
            AddSuffix("speedAtPeriapsis", new ThreeArgsSuffix<ScalarDoubleValue, BodyTarget, ScalarValue, ScalarValue>(SpeedAtPeriapsis));
            AddSuffix("speedAtApoapsis", new ThreeArgsSuffix<ScalarDoubleValue, BodyTarget, ScalarValue, ScalarValue>(SpeedAtApoapsis));
            AddSuffix("shipSpeedAtPeriapsis", new NoArgsSuffix<ScalarDoubleValue>(ShipSpeedAtPeriapsis));
            AddSuffix("shipSpeedAtApoapsis", new NoArgsSuffix<ScalarDoubleValue>(ShipSpeedAtApoapsis));

        }

        #region suffix_functions
        // I don't know how to move these directly into the above, or make the above call different classes. Haven't learned enough about delegates yet.

        private Node CreateTransfer(params Safe.Encapsulation.Structure[] args) => Transfers.CreateManeuverNodes(shared, args);
        private ListValue CalculateBurns(BodyTarget dest) => Transfers.CalculateBurns(shared, dest);
        private ScalarDoubleValue DeltaVToOrbit(BodyTarget body) => PhysicsTools.DeltaVToOrbit(body.Body);
        private ScalarDoubleValue SpeedAtPeriapsis(BodyTarget body, ScalarValue apopapsis, ScalarValue periapsis) => PhysicsTools.SpeedAtPeriapsis(body.Body, apopapsis.GetDoubleValue(), periapsis.GetDoubleValue());
        private ScalarDoubleValue SpeedAtApoapsis(BodyTarget body, ScalarValue apopapsis, ScalarValue periapsis) => PhysicsTools.SpeedAtApoapsis(body.Body, apopapsis.GetDoubleValue(), periapsis.GetDoubleValue());
        private ScalarDoubleValue ShipSpeedAtPeriapsis() => PhysicsTools.SpeedAtPeriapsis(shared.Vessel.mainBody, shared.Vessel.orbit.ApR, shared.Vessel.orbit.PeR);
        private ScalarDoubleValue ShipSpeedAtApoapsis() => PhysicsTools.SpeedAtApoapsis(shared.Vessel.mainBody, shared.Vessel.orbit.ApR, shared.Vessel.orbit.PeR);
        #endregion

        #region internal_function
        #endregion
    }
}