using kOS.AddOns;
using kOS.Safe.Utilities;
using kOS.Safe.Encapsulation;
using kOS.Safe.Encapsulation.Suffixes;
using kOS.Suffixed;
using kOS;
using Astrogator;
using System.Reflection;
using System.Linq;

namespace AstrogatorKOS {
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
            AddSuffix("version", suffixToAdd: new NoArgsSuffix<StringValue>(PrintAstrogatorVersion));
            AddSuffix("create", suffixToAdd: new TwoArgsSuffix<StringValue, BodyTarget, BodyTarget>(CreateTransfer));
        }

        #region suffix_functions
        private StringValue PrintAstrogatorVersion()
        {
            return versionString;
        }

        private StringValue CreateTransfer(BodyTarget src, BodyTarget dest)
        {
            TransferModel model = new TransferModel(src.Target, dest.Target);
            model.CalculateEjectionBurn();
            model.CalculatePlaneChangeBurn();
            model.CreateManeuvers();
            return model.ToString();
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