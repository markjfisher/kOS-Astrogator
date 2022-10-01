using Astrogator;
using kOS.AddOns.kOSAstrogator.structure;
using kOS.Safe.Encapsulation;
using kOS.Suffixed;


namespace kOS.AddOns.kOSAstrogator.transfer
{
    /// <summary>
    /// 
    /// </summary>
    public static class Transfers
    {
        /// <summary>
        /// Invokes Astrogator's CreateManeuvers method to produce a body transfer, if possible.
        /// </summary>
        /// <param name="shared"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static Node CreateManeuverNodes(SharedObjects shared, Structure[] args)
        {
            BodyTarget dest = (BodyTarget)args[0];
            bool paramGeneratePlaneChangeBurns = (BooleanValue)args[1];

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shared"></param>
        /// <param name="dest"></param>
        /// <returns></returns>
        public static ListValue CalculateBurns(SharedObjects shared, BodyTarget dest)
        {
            ListValue burns = new ListValue();
            TransferModel model = new TransferModel(shared.Vessel, dest.Target);
            model.CalculateEjectionBurn();
            model.CalculatePlaneChangeBurn();
            burns.Add(BurnModelStructure.Create(model.ejectionBurn, shared));
            if (model.planeChangeBurn != null) burns.Add(BurnModelStructure.Create(model.planeChangeBurn, shared));
            return burns;
        }
    }
}
