public static class GameplayBalance
{
    public static class Actions
    {
        // Bed (по SRS: экран темнеет на 5 секунд)
        public const float BedDuration = 5f;

        // Кулдаун кровати не обязателен, т.к. доступность ограничена временем.
        // Оставим 0, чтобы не мешал.
        public const float BedCooldown = 0f;

        // Fridge
        public const float FridgeDuration = 2f;
        public const float FridgeCooldown = 4f;

        // Window
        public const float WindowDuration = 1.5f;
        public const float WindowCooldown = 3f;
    }
}
