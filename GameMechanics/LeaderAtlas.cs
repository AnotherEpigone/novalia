using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Novalia.GameMechanics
{
    public static class LeaderAtlas
    {
        private static readonly Lazy<Dictionary<string, LeaderTemplate>> _byId;

        static LeaderAtlas()
        {
            _byId = new Lazy<Dictionary<string, LeaderTemplate>>(() => typeof(LeaderAtlas)
                .GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Where(p => p.PropertyType == typeof(LeaderTemplate))
                .Select(p => p.GetValue(null))
                .OfType<LeaderTemplate>()
                .ToDictionary(
                i => i.Id,
                i => i));
        }

        public static Dictionary<string, LeaderTemplate> ById => _byId.Value;

        public static LeaderTemplate MagnanII => new LeaderTemplate(
            "LEADER_MAGNAN_II",
            "Magnan II");

        public static LeaderTemplate RanulfGoldenale => new LeaderTemplate(
            "LEADER_RANULFGOLDENALE",
            "Ranulf Goldenale");

        public static LeaderTemplate Blackhand => new LeaderTemplate(
            "LEADER_BLACKHAND",
            "The Blackhand");

        public static LeaderTemplate Ortakias => new LeaderTemplate(
            "LEADER_ORTAKIAS",
            "Ortakias");
    }
}
