using System;

namespace Quantum
{

    using Photon.Deterministic;

    public class KccSettings : AssetObject
    {
        public FP Radius = FP._0_50;
        public Int32 MaxContacts = 2;
        public FP AllowedPenetration = FP._0_10;
        public FP CorrectionSpeed = FP._10 / 30;
        public FP Acceleration = FP._10 / 30;
        public Boolean Debug = false;
        public FP Brake = FP._0_33;
    }

}