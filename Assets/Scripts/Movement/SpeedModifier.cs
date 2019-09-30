namespace RPG.Movement
{
    struct SpeedModifier
    {
        const float Amble = 0.75f;
        const float Hustle = 1.75f;
        const float Idle = 0f;
        const float Jog = 2.5f;
        const float Run = 3f;
        const float Sneak = 0.25f;
        const float Walk = 1f;

        public float GetSpeedModifier(string speed)
        {
            float speedMultiplier;

            if (speed == "amble") { speedMultiplier = Amble; }
            else if (speed == "hustle") { speedMultiplier = Hustle; }
            else if (speed == "jog") { speedMultiplier = Jog; }
            else if (speed == "run") { speedMultiplier = Run; }
            else if (speed == "sneak") { speedMultiplier = Sneak; }
            else if (speed == "walk") { speedMultiplier = Walk; }
            else { speedMultiplier = Idle; }

            return speedMultiplier;
        }
    }
}