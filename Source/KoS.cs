using kOS.Safe.Utilities;
using kOS.Safe.Encapsulation;
using kOS.Safe.Encapsulation.Suffixes;
using kOS.Suffixed;
using Astrogator;
using System.Reflection;
using System.Linq;
using UnityEngine;
using System;

namespace kOS.AddOns.kOSAstrogator {
    /// <summary>
    /// kOS integration for Astrogator
    /// </summary>
    [kOSAddon("ASTROGATOR")]
    [KOSNomenclature("AstrogatorAddon")]
    public class KOSAstrogator : Addon
    {
        private static readonly Version modVersion = Assembly.GetExecutingAssembly().GetName().Version;

        /// <summary>
        /// The class initializer
        /// </summary>
        /// <param name="shared">The shared objects</param>
        public KOSAstrogator(SharedObjects shared) : base(shared)
        {
            InitializeSuffixes();
        }

        /// <inheritdoc/>
        public override BooleanValue Available()
        {
            return IsModInstalled("Astrogator");
        }

        private void InitializeSuffixes()
        {
            // info
            AddSuffix("help", new NoArgsVoidSuffix(PrintHelp));
            AddSuffix("version", new NoArgsSuffix<StringValue>(GetVersion));

            // creating transfers and info
            AddSuffix("create", new VarArgsSuffix<Node, Structure>(CreateTransfer));
            AddSuffix("calculateBurns", new OneArgsSuffix<ListValue, BodyTarget>(CalculateBurns));

            // physics
            AddSuffix("deltaVToOrbit", new OneArgsSuffix<ScalarDoubleValue, BodyTarget>(KADeltaVToOrbit));
            AddSuffix("speedAtPeriapsis", new ThreeArgsSuffix<ScalarDoubleValue, BodyTarget, ScalarValue, ScalarValue>(KASpeedAtPeriapsis));
            AddSuffix("speedAtApoapsis", new ThreeArgsSuffix<ScalarDoubleValue, BodyTarget, ScalarValue, ScalarValue>(KASpeedAtApoapsis));
            AddSuffix("shipSpeedAtPeriapsis", new NoArgsSuffix<ScalarDoubleValue>(KAShipSpeedAtPeriapsis));
            AddSuffix("shipSpeedAtApoapsis", new NoArgsSuffix<ScalarDoubleValue>(KAShipSpeedAtApoapsis));
        }

        #region suffix_functions
        private void PrintHelp()
        {
            // TODO: split cmd into sub sections, like "addons:astrogator:info:help" if possible?
            // info, tf, phys, ...
            shared.Screen.Print("--------------------------------------------");
            shared.Screen.Print("kOS-Astrogator Help: addons:astrogator:<cmd>");
            shared.Screen.Print("----- Information -----");
            shared.Screen.Print("help:    this help message");
            shared.Screen.Print("version: return mod and Astrogator version string");
            shared.Screen.Print("----- Transfers -----");
            shared.Screen.Print("create(dest, [genPlaneChange])");
            shared.Screen.Print("  create nodes to get to dest body");
            shared.Screen.Print("    dest: (BodyTarget, reqd) destination body, e.g. Mun");
            shared.Screen.Print("    genPlaneChange: (bool, def: true) also generate node for plane burn change if needed");
            shared.Screen.Print("calculateBurns(dest)");
            shared.Screen.Print("  returns list of BurnModels for ejection/plane changes");
            shared.Screen.Print("    dest: (BodyTarget, reqd) destination body, e.g. Mun");
            shared.Screen.Print("----- Physics -----");
            shared.Screen.Print("deltaVToOrbit(body)");
            shared.Screen.Print("  deltav needed for the vessel to get to orbit around");
            shared.Screen.Print("  the given body");
            shared.Screen.Print("    body: (BodyTarget) the target body to get orbit around");
            shared.Screen.Print("shipSpeedAtPeriapsis()");
            shared.Screen.Print("  speed of ship in m/s around its current body at periapsis");
            shared.Screen.Print("shipSpeedAtApoapsis()");
            shared.Screen.Print("  speed of ship in m/s around its current body at apoapsis");
            shared.Screen.Print("speedAtPeriapsis(body, apoapsis, periapsis)");
            shared.Screen.Print("  general version for any body with given apo/peri.");
            shared.Screen.Print("speedAtPeriapsis(body, apoapsis, periapsis");
            shared.Screen.Print("  general version for any body with given apo/peri.");
            shared.Screen.Print("");
        }

        private StringValue GetVersion()
        {
            return string.Format("kOS-Astrogator: v{0}.{1}.{2}, Astrogator: {3}", modVersion.Major, modVersion.Minor, modVersion.Build, Astrogator.ViewTools.versionString);
        }

        private Node CreateTransfer(params Structure[] args)
        {
            BodyTarget dest = (BodyTarget) args[0];
            bool paramGeneratePlaneChangeBurns = true;
            if (args.Length > 1) paramGeneratePlaneChangeBurns = (BooleanValue)args[1];

            // store old values
            bool autoTargetDestination = Settings.Instance.AutoTargetDestination;
            bool generatePlaneChangeBurns = Settings.Instance.GeneratePlaneChangeBurns;
            bool autoEditEjectionNode = Settings.Instance.AutoEditEjectionNode;
            bool autoEditPlaneChangeNode = Settings.Instance.AutoEditPlaneChangeNode;
            bool autoFocusDestination = Settings.Instance.AutoFocusDestination;
            bool autoSetSAS = Settings.Instance.AutoSetSAS;

            // set base flags off
            Settings.Instance.AutoTargetDestination = false;
            Settings.Instance.AutoEditEjectionNode = false;
            Settings.Instance.AutoEditPlaneChangeNode = false;
            Settings.Instance.AutoFocusDestination = false;
            Settings.Instance.AutoSetSAS = false;

            // take from config
            Settings.Instance.GeneratePlaneChangeBurns = paramGeneratePlaneChangeBurns;

            TransferModel model = new TransferModel(shared.Vessel, dest.Target);
            model.CalculateEjectionBurn();
            if (generatePlaneChangeBurns) model.CalculatePlaneChangeBurn();
            model.CreateManeuvers();

            // restore old values
            Settings.Instance.AutoTargetDestination = autoTargetDestination;
            Settings.Instance.GeneratePlaneChangeBurns = generatePlaneChangeBurns;
            Settings.Instance.AutoEditEjectionNode = autoEditEjectionNode;
            Settings.Instance.AutoEditPlaneChangeNode = autoEditPlaneChangeNode;
            Settings.Instance.AutoFocusDestination = autoFocusDestination;
            Settings.Instance.AutoSetSAS = autoSetSAS;

            if (model.ejectionBurn == null || model.ejectionBurn.node == null) return new Node(Planetarium.GetUniversalTime(), 0, 0, 0, shared);
            return Node.FromExisting(shared.Vessel, model.ejectionBurn.node, shared);
        }

        private ListValue CalculateBurns(BodyTarget dest)
        {
            ListValue burns = new ListValue();
            TransferModel model = new TransferModel(shared.Vessel, dest.Target);
            model.CalculateEjectionBurn();
            model.CalculatePlaneChangeBurn();
            burns.Add(BurnModelStructure.Create(model.ejectionBurn, shared));
            if (model.planeChangeBurn != null) burns.Add(BurnModelStructure.Create(model.planeChangeBurn, shared));
            return burns;
        }

        private ScalarDoubleValue KADeltaVToOrbit(BodyTarget body)
        {
            return PhysicsTools.DeltaVToOrbit(body.Body);
        }

        private ScalarDoubleValue KASpeedAtPeriapsis(BodyTarget body, ScalarValue apopapsis, ScalarValue periapsis)
        {
            return PhysicsTools.SpeedAtPeriapsis(body.Body, apopapsis.GetDoubleValue(), periapsis.GetDoubleValue());
        }

        private ScalarDoubleValue KASpeedAtApoapsis(BodyTarget body, ScalarValue apopapsis, ScalarValue periapsis)
        {
            return PhysicsTools.SpeedAtApoapsis(body.Body, apopapsis.GetDoubleValue(), periapsis.GetDoubleValue());
        }

        private ScalarDoubleValue KAShipSpeedAtPeriapsis()
        {
            return PhysicsTools.SpeedAtPeriapsis(shared.Vessel.mainBody, shared.Vessel.orbit.ApR, shared.Vessel.orbit.PeR);
        }
        private ScalarDoubleValue KAShipSpeedAtApoapsis()
        {
            return PhysicsTools.SpeedAtApoapsis(shared.Vessel.mainBody, shared.Vessel.orbit.ApR, shared.Vessel.orbit.PeR);
        }
        #endregion

        #region internal_function
        ///<summary>
        /// checks if the mod with "assemblyName" is loaded into KSP. Taken from KOS-Scansat
        ///</summary>
        internal static bool IsModInstalled(string assemblyName)
        {
          Assembly assembly = (from a in AssemblyLoader.loadedAssemblies
            where a.name.ToLower().Equals(assemblyName.ToLower())
            select a).FirstOrDefault().assembly;
          return assembly != null;
        }
        #endregion
    }
}