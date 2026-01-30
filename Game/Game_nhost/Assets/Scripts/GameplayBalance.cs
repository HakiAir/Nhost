public static class GameplayBalance
{
    public static class Actions
    {
        // Bed
        public const float BedDuration = 5f;
        public const float BedCooldown = 0f;

        // Fridge
        public const float FridgeDuration = 2f;
        public const float FridgeCooldown = 4f;

        // Window
        public const float WindowDuration = 1.5f;
        public const float WindowCooldown = 3f;
    }

    public static class Stats
    {
        // Расход В ПРОЦЕНТАХ за 1 ВНУТРИИГРОВОЙ час
        public const float HungerDrainPerHour = 8f;
        public const float SleepDrainPerHour = 5f;
        public const float FocusDrainPerHour = 6f;
    }
}
