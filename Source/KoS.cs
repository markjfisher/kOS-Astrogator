using kOS.Safe.Utilities;
using kOS.Safe.Encapsulation;
using kOS.Safe.Encapsulation.Suffixes;
using kOS.Suffixed;
using Astrogator;
using System.Reflection;
using System.Linq;

namespace kOS.AddOns.kOSAstrogator {
    using static ViewTools;

    /// <summary>
    /// kOS integration for Astrogator
    /// </summary>
    [kOSAddon("ASTROGATOR")]
    [KOSNomenclature("AstrogatorAddon")]
    public class KOSAstrogator : Addon
    {
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
            AddSuffix("help", new NoArgsVoidSuffix(PrintHelp));
            AddSuffix("version", new NoArgsSuffix<StringValue>(GetAstrogatorVersion));
            AddSuffix("create", new VarArgsSuffix<Node, Structure>(CreateTransfer));
            AddSuffix("calculateBurns", new OneArgsSuffix<ListValue, BodyTarget>(CalculateBurns));
        }

        #region suffix_functions
        private void PrintHelp()
        {
            shared.Screen.Print("kOS-Astrogator Help: addons:astrogator:<cmd>");
            shared.Screen.Print("          help: this help message");
            shared.Screen.Print("       version: return Astrogator version string");
            shared.Screen.Print("        create: [dest, genPlaneChange] create nodes to get to dest body");
            shared.Screen.Print("                dest is any body (required),");
            shared.Screen.Print("                genPlaneChange generates additional node (def: true)");
            shared.Screen.Print("calculateBurns: [dest] Calc and return List of BurnModels for ejection/plane changes.");
        }

        private StringValue GetAstrogatorVersion()
        {
            return versionString;
        }

        private Node CreateTransfer(params Structure[] args)
        {
            // This is an exception
            // throw new kOS.Safe.Exceptions.KOSException("invalid resource type");

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