namespace RPG.Core
{
    public enum AttackType
    {
        area, ranged, touch
    }

    public enum CasterClass
    {
        cleric, druid, palidine, ranger, sorceror, wizard
    }

    public enum CastingSpeed
    {
        instant, standard, full, other
    }

    public enum CharacterStat
    {
        attacks, baseSave
    }

    public enum Hand
    {
        left, right
    }

    public enum PlayerClass
    {
        barbarian, bard, cleric, druid, fighter, monk, palidine, ranger, rogue, sorceror, wizard
    }

    public enum ProgressionRate
    {
        fair, good, poor
    }

    public enum Range
    {
        close, far, given, medium, personal, touch
    }

    public enum SaveType
    {
        fortitude, reflex, will
    }

    public enum School
    {
        abjuration, conjuration, divination, enchantment, evokation, illusion, necromancy, transmutation, universal
    }

    public enum SpellLevel
    {
        cantrips, first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth
    }
}