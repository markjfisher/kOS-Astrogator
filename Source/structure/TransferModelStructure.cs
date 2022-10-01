using Astrogator;
using kOS.Safe.Encapsulation;
using kOS.Safe.Encapsulation.Suffixes;
using kOS.Safe.Utilities;

namespace kOS.AddOns.kOSAstrogator.structure
{
    /// <summary>
    /// A structure wrapper for TransferModel
    /// </summary>
    [KOSNomenclature("AstrogatorTransferModel")]
    public class TransferModelStructure : Structure
    {
        private readonly TransferModel model;
        private readonly SharedObjects shared;

        private TransferModelStructure(TransferModel model, SharedObjects shared)
        {
            this.model = model;
            this.shared = shared;
            model.CalculateEjectionBurn();
            model.CalculatePlaneChangeBurn();

            InitializeSuffixes();
        }

        /// <summary>
        /// Creates a TransferModelStructure
        /// </summary>
        /// <param name="model">The model to wrap</param>
        /// <param name="shared">Game shared data</param>
        /// <returns></returns>
        public static TransferModelStructure Create(TransferModel model, SharedObjects shared) => new TransferModelStructure(model, shared);

        private void InitializeSuffixes()
        {
            AddSuffix("destination", new Suffix<ITargetableStructure>(() => ITargetableStructure.Create(model.destination, shared)));
            AddSuffix("transferDestination", new Suffix<ITargetableStructure>(() => ITargetableStructure.Create(model.transferDestination, shared)));
            AddSuffix("transferParent", new Suffix<CelestialBodyStructure>(() => CelestialBodyStructure.Create(model.transferParent, shared)));
            AddSuffix("retrogradeTransfer", new Suffix<BooleanValue>(() => model.retrogradeTransfer));
            AddSuffix("origin", new Suffix<ITargetableStructure>(() => ITargetableStructure.Create(model.origin, shared)));
            AddSuffix("ejectionBurn", new Suffix<BurnModelStructure>(() => BurnModelStructure.Create(model.ejectionBurn, shared)));
            AddSuffix("planeChangeburn", new Suffix<BurnModelStructure>(() => BurnModelStructure.Create(model.planeChangeBurn, shared)));
            AddSuffix("ejectionBurnDuration", new Suffix<ScalarValue>(() => model.ejectionBurnDuration ?? -1.0));

            AddSuffix("CalculateEjectionBurn", new NoArgsVoidSuffix(() => model.CalculateEjectionBurn()));
            AddSuffix("GetDuration", new NoArgsVoidSuffix(() => model.GetDuration()));
            AddSuffix("CalculatePlaneChangeBurn", new NoArgsVoidSuffix(() => model.CalculatePlaneChangeBurn()));
            AddSuffix("HaveEncounter", new Suffix<BooleanValue>(() => model.HaveEncounter()));
            AddSuffix("CheckIfNodesDisappeared", new NoArgsVoidSuffix(() => model.CheckIfNodesDisappeared()));
            AddSuffix("CreateManeuvers", new NoArgsVoidSuffix(() => { 
                model.CalculateEjectionBurn();
                model.CalculatePlaneChangeBurn();
                model.CreateManeuvers();
            }));
            AddSuffix("WarpToBurn", new NoArgsVoidSuffix(() => model.WarpToBurn()));
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            string s = string.Format("TransferModel(\n" +
                "          destination: {0}\n" +
                "  transferDestination: {1}\n" +
                "       transferParent: {2}\n" +
                "   retrogradeTransfer: {3}\n" +
                "               origin: {4}\n" +
                "         ejectionBurn: {5}\n" +
                "      planeChangeburn: {6}\n" +
                " ejectionBurnDuration: {7}\n" +
                ")",
                ITargetableStructure.Create(model.destination, shared),
                ITargetableStructure.Create(model.transferDestination, shared),
                CelestialBodyStructure.Create(model.transferParent, shared),
                model.retrogradeTransfer,
                ITargetableStructure.Create(model.origin, shared),
                BurnModelStructure.Create(model.ejectionBurn, shared),
                BurnModelStructure.Create(model.planeChangeBurn, shared),
                model.ejectionBurnDuration ?? -1.0
            );
            return s;
        }
    }
}
