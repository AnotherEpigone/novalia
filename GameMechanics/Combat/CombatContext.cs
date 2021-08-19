using SadRogue.Primitives;

namespace Novalia.GameMechanics.Combat
{
    public record CombatContext(
        Point Attacker,
        Point Defender)
    {
    }
}
