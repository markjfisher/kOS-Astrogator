using Astrogator;
using kOS.Safe.Encapsulation;
using kOS.Safe.Encapsulation.Suffixes;
using kOS.Safe.Utilities;
using kOS.Suffixed;

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

        private TransferTarget CreateTransferTarget(ITargetable target)
        {
            TransferTarget t;
            if (model.transferDestination.GetVessel() == null)
            {
                t = BodyTransferTarget.Create(target, shared);
            }
            else
            {
                t = VesselTransferTarget.Create(target, shared);
            }
            return t;
        }

        private void InitializeSuffixes()
        {
            AddSuffix("destination", new Suffix<TransferTarget>(() => CreateTransferTarget(model.destination)));
            AddSuffix("transferDestination", new Suffix<TransferTarget>(() => CreateTransferTarget(model.transferDestination)));
            AddSuffix("transferParent", new Suffix<BodyTarget>(() => BodyTarget.CreateOrGetExisting(model.transferParent, shared)));
            AddSuffix("retrogradeTransfer", new Suffix<BooleanValue>(() => model.retrogradeTransfer));
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
            return string.Format("TransferModel(\"{0}\")", model.destination.GetName());
        }
    }
}
