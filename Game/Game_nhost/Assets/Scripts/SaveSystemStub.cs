using UnityEngine;

public static class SaveSystemStub
{
    private const string KEY_HAS_SAVE = "has_save";

    public static bool HasSave() => PlayerPrefs.GetInt(KEY_HAS_SAVE, 0) == 1;

    public static void MarkHasSave(bool value)
    {
        PlayerPrefs.SetInt(KEY_HAS_SAVE, value ? 1 : 0);
    }

    public static void ClearSave()
    {
        PlayerPrefs.SetInt(KEY_HAS_SAVE, 0);
    }
}
