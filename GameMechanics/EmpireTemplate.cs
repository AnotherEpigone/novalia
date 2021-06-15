namespace Novalia.GameMechanics
{
    public class EmpireTemplate
    {
        public EmpireTemplate(
            string id,
            string name)
        {
            Id = id;
            Name = name;
        }

        public string Id { get; }
        public string Name { get; }
    }
}
