using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Novalia.GameMechanics
{
    public static class EmpireAtlas
    {
        private static readonly Lazy<Dictionary<string, EmpireTemplate>> _byId;

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
        }

        public static Dictionary<string, EmpireTemplate> ById => _byId.Value;

        public static EmpireTemplate Sudet => new EmpireTemplate(
            "EMPIRE_SUDET",
            "Kingdom of Sudet");

        public static EmpireTemplate Havenshire => new EmpireTemplate(
            "EMPIRE_HAVENSHIRE",
            "Havenshire");

        public static EmpireTemplate BlackhandDominion => new EmpireTemplate(
            "EMPIRE_BLACKHAND",
            "Blackhand Dominion");

        public static EmpireTemplate Ayen => new EmpireTemplate(
            "EMPIRE_AYEN",
            "Ayen");
    }
}
