using SadRogue.Primitives;
using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Novalia.GameMechanics
{
    [DataContract]
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Empire
    {
        public Empire()
        {
            Gold = 0;
            Id = Guid.NewGuid();
        }

        public Empire(EmpireTemplate template)
            : this()
        {
            TemplateId = template.Id;
            Name = template.Name;
            Leader = new Leader(template.DefaultLeader);
            Playable = template.Playable;
            Color = template.Color;
        }

        [DataMember]
        public string TemplateId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int Gold { get; set; }

        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public Leader Leader { get; set; }

        [DataMember]
        public bool Playable { get; set; }

        [DataMember]
        public Color Color { get; set; }

        private string DebuggerDisplay => $"{nameof(Empire)}: {Name}";
    }
}
