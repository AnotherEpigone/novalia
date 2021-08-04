using System.Diagnostics;
using System.Runtime.Serialization;

namespace Novalia.GameMechanics
{
    [DataContract]
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Leader
    {
        public Leader(LeaderTemplate template)
        {
            Name = template.Name;
            TemplateId = template.Id;
        }

        [DataMember]
        public string TemplateId { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}
