using kOS.AddOns;
using kOS.Safe.Utilities;
using kOS.Safe.Encapsulation;
using kOS.Safe.Encapsulation.Suffixes;
using UnityEngine;
using System;
using kOS.Suffixed;
using kOS;
using Astrogator;

namespace AstrogatorKOS {
    using static ViewTools;

    /// <summary>
    /// kOS integration for Astrogator
    /// </summary>
    [kOSAddon("ASTROGATOR")]
    [KOSNomenclature("ASTROGATORAddon")]
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

        private void InitializeSuffixes()
        {
            AddSuffix("version", suffixToAdd: new NoArgsSuffix<StringValue>(PrintAstrogatorVersion));
        }

        private StringValue PrintAstrogatorVersion()
        {
            return versionString;
        }

        /// <inheritdoc/>
        public override BooleanValue Available()
        {
            return true;
        }
    }
}