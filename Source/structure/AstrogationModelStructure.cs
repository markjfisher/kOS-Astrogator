using Astrogator;
using kOS.Safe.Encapsulation;
using kOS.Safe.Encapsulation.Suffixes;
using kOS.Safe.Utilities;
using kOS.Suffixed;
using System.Linq;

namespace kOS.AddOns.kOSAstrogator.structure
{
    /// <summary>
    /// A structure wrapper for AstrogationModel
    /// </summary>
    [KOSNomenclature("AstrogationModel")]
    public class AstrogationModelStructure : Structure
    {
        private readonly AstrogationModel model;
        private readonly SharedObjects shared;

        private AstrogationModelStructure(AstrogationModel model, SharedObjects shared)
        {
            this.model = model;
            this.shared = shared;
            
            InitializeSuffixes();
        }

        /// <summary>
        /// Creates a wrapper around AstrogationModel
        /// </summary>
        /// <param name="model">The model to wrap</param>
        /// <param name="shared">Game shared data</param>
        /// <returns></returns>
        public static AstrogationModelStructure Create(AstrogationModel model, SharedObjects shared) => new AstrogationModelStructure(model, shared);

        private void InitializeSuffixes()
        {
            AddSuffix("origin", new Suffix<ITargetableStructure>(() => ITargetableStructure.Create(model.origin, shared)));
            AddSuffix("transfers", new NoArgsSuffix<Lexicon>(() => GetTransfers(), "Gets calculated Astrogator Transfers"));
            AddSuffix("ErrorCondition", new Suffix<BooleanValue>(() => model.ErrorCondition));
            AddSuffix("badInclination", new Suffix<BooleanValue>(() => model.badInclination));
            AddSuffix("retrogradeOrbit", new Suffix<BooleanValue>(() => model.retrogradeOrbit));
            AddSuffix("inbound", new Suffix<BooleanValue>(() => model.inbound));
            AddSuffix("hyperbolicOrbit", new Suffix<BooleanValue>(() => model.hyperbolicOrbit));

            AddSuffix("Reset", new OneArgsSuffix<ITargetableStructure>((o) => model.Reset(o.target)));
            AddSuffix("CheckIfNodesDisappeared", new NoArgsVoidSuffix(() => model.CheckIfNodesDisappeared()));
            AddSuffix("GetDurations", new NoArgsVoidSuffix(() => model.GetDurations()));
            AddSuffix("ActiveTransfer", new Suffix<TransferModelStructure>(() => TransferModelStructure.Create(model.ActiveTransfer, shared)));
            AddSuffix("ActiveEjectionBurn", new Suffix<BurnModelStructure>(() => BurnModelStructure.Create(model.ActiveEjectionBurn, shared)));
            AddSuffix("ActivePlaneChangeBurn", new Suffix<BurnModelStructure>(() => BurnModelStructure.Create(model.ActivePlaneChangeBurn, shared)));

        }

        private Lexicon GetTransfers()
        {
            Lexicon transfers = new Lexicon();
            foreach(TransferModel t in model.transfers)
            {
                transfers.Add(new StringValue(t.destination.GetName()), TransferModelStructure.Create(t, shared));
            }
            return transfers;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            if (model == null) return "AstrogationModel(null)";

            Lexicon l = GetTransfers();
            string transfersString = "Lex[" + string.Join(", ", l.Keys) + "]";
            string s = string.Format("AstrogationModel(origin: {0}, transfers: {1})",
                ITargetableStructure.Create(model.origin, shared),
                transfersString
            );

            return s;
        }
    }
}
