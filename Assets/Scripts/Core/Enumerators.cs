namespace RPG.Core
{
    public enum AttackType
    {
        area,
        ranged,
        touch
    }

    public enum Attributes
    {
        strength,
        constitution,
        dexterity,
        intelligence,
        wisdom,
        charisma
    }

    public enum CharacterClass
    {
        barbarian,
        bard,
        cleric,
        druid,
        fighter,
        monk,
        palidine,
        ranger,
        rogue,
        sorceror,
        wizard
    }

    public enum CasterClass
    {
        cleric,
        druid,
        palidine,
        ranger,
        sorceror,
        wizard
    }

    public enum CastingSpeed
    {
        full,
        instant,
        standard,
        other
    }

    public enum Hand
    {
        left,
        right
    }

    public enum Range
    {
        close,
        far,
        given,
        medium,
        personal,
        touch
    }

    public enum School
    {
        abjuration,
        conjuration,
        divination,
        enchantment,
        evokation,
        illusion,
        necromancy,
        transmutation,
        universal
    }

    public enum Stat
    {
        experienceValue,
        health,
        numberOfAttacks
    }
}