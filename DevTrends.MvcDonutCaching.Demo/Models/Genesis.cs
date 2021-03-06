﻿using System;
using System.Runtime.Serialization;

namespace DevTrends.MvcDonutCaching.Demo.Models
{
    [Serializable]
    public class Genesis
    {
        [DataMember(Order = 1)]
        public string TierOneData { get; set; }
        [DataMember(Order = 2)]
        public string TierTwoData { get; set; }
        [DataMember(Order = 3)]
        public TierThree TierThreeData { get; set; }

        public Genesis()
        {
            TierOneData = $"initialized @ {DateTime.Now.ToString()}";
        }
    }

    [Serializable]
    public class TierThree
    {
        [DataMember(Order = 1)]
        public string Data { get; set; }

        public TierThree()
        {
            Data = $"initialized @ {DateTime.Now.ToString()}";
        }
    }
}
