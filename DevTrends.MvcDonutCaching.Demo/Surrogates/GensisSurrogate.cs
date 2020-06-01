using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DevTrends.MvcDonutCaching.Demo.Surrogates
{
    public class GensisSurrogate
    {
        [ProtoMember(1)]
        public string AssemblyQualifiedName { get; set; }
        // protobuf-net wants an implicit or explicit operator between the types
        public static implicit operator Type(GensisSurrogate value)
        {
            return value == null ? null : Type.GetType(value.AssemblyQualifiedName);
        }
        public static implicit operator GensisSurrogate(Type value)
        {
            return value == null ? null : new GensisSurrogate
            {
                AssemblyQualifiedName = value.AssemblyQualifiedName
            };
        }
    }
}
