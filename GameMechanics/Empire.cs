using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Novalia.GameMechanics
{
    [DataContract]
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Empire
    {
        public Empire(EmpireTemplate template)
        {
            TemplateId = template.Id;
            Name = template.Name;
            Gold = 0;
            Id = Guid.NewGuid();
        }

        [DataMember]
        public string TemplateId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int Gold { get; set; }

        [DataMember]
        public Guid Id { get; set; }

        private string DebuggerDisplay => $"{nameof(Empire)}: {Name}";
    }
}
