namespace RPG.Core
{
    public enum AttackType
    {
        area, ranged, touch
    }

    public enum Attributes
    {
        strength, constitution, dexterity, intelligence, wisdom, charisma
    }

    public enum PlayerClass
    {
        barbarian, bard, cleric, druid, fighter, monk, palidine, ranger, rogue, sorceror, wizard
    }

    public enum CasterClass
    {
        cleric, druid, palidine, ranger, sorceror, wizard
    }

    public enum CastingSpeed
    {
        instant, standard, full, other
    }

    public enum ProgressionRate
    {
        good, fair, poor
    }

    public enum Hand
    {
        left, right
    }

    public enum Range
    {
        close, far, given, medium, personal, touch
    }

    public enum School
    {
        abjuration, conjuration, divination, enchantment, evokation, illusion, necromancy, transmutation, universal
    }

    public enum SpellLevel
    {
        cantrips, first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth
    }

    public enum Stat
    {
        attacks,
        maxHitPoints,
        skillPoints,
        spellsKnown,
        spellsPerDay
    }

    public enum XP
    {value, requirement}
}