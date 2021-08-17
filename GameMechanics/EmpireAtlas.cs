using SadRogue.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Novalia.GameMechanics
{
    public static class EmpireAtlas
    {
        private static readonly Lazy<Dictionary<string, EmpireTemplate>> _byId;
        private static readonly Lazy<Dictionary<string, EmpireTemplate>> _byName;

        static EmpireAtlas()
        {
            _byId = new Lazy<Dictionary<string, EmpireTemplate>>(() => typeof(EmpireAtlas)
                .GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Where(p => p.PropertyType == typeof(EmpireTemplate))
                .Select(p => p.GetValue(null))
                .OfType<EmpireTemplate>()
                .ToDictionary(
                i => i.Id,
                i => i));
            _byName = new Lazy<Dictionary<string, EmpireTemplate>>(() => typeof(EmpireAtlas)
                .GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Where(p => p.PropertyType == typeof(EmpireTemplate))
                .Select(p => p.GetValue(null))
                .OfType<EmpireTemplate>()
                .ToDictionary(
                i => i.Name,
                i => i));
        }

        public static Dictionary<string, EmpireTemplate> ById => _byId.Value;
        public static Dictionary<string, EmpireTemplate> ByName => _byName.Value;

        public static EmpireTemplate Sudet => new EmpireTemplate(
            "EMPIRE_SUDET",
            "Kingdom of Sudet",
            LeaderAtlas.MagnanII,
            true,
            Color.Blue);

        public static EmpireTemplate Havenshire => new EmpireTemplate(
            "EMPIRE_HAVENSHIRE",
            "Havenshire",
            LeaderAtlas.RanulfGoldenale,
            true,
            Color.Gold);

        public static EmpireTemplate BlackhandDominion => new EmpireTemplate(
            "EMPIRE_BLACKHAND",
            "Blackhand Dominion",
            LeaderAtlas.Blackhand,
            false,
            Color.Black);

        public static EmpireTemplate Ayen => new EmpireTemplate(
            "EMPIRE_AYEN",
            "Ayen",
            LeaderAtlas.Ortakias,
            true,
            Color.Red);
    }
}
