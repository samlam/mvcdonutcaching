﻿using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DevTrends.MvcDonutCaching
{
    [Serializable, ProtoContract, DataContract]
    public class PartialSettings
    {
        [DataMember(Order = 1), ProtoMember(1)]
        public string PartialViewName { get; set; }

        [DataMember(Order = 3), ProtoMember(3)]
        public object Model { get; set; }

        [DataMember(Order = 4), ProtoMember(4)]
        public string ModelTypeName { get; set; }

    }
}
