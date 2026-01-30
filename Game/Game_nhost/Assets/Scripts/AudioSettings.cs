using UnityEngine;

public static class AudioSettings
{
    private const string KEY_MASTER = "vol_master";
    private const string KEY_SFX = "vol_sfx";
    private const string KEY_MUSIC = "vol_music";

    public static float Master { get; private set; }
    public static float Sfx { get; private set; }
    public static float Music { get; private set; }

    public static void Load()
    {
        Master = PlayerPrefs.GetFloat(KEY_MASTER, 1f);
        Sfx = PlayerPrefs.GetFloat(KEY_SFX, 1f);
        Music = PlayerPrefs.GetFloat(KEY_MUSIC, 1f);
        Apply();
    }

    public static void Apply()
    {
        // пока без микшера — просто общая громкость
        AudioListener.volume = Mathf.Clamp01(Master);
    }

    public static void SetMaster(float v)
    {
        Master = Mathf.Clamp01(v);
        PlayerPrefs.SetFloat(KEY_MASTER, Master);
        Apply();
    }

    public static void SetSfx(float v)
    {
        Sfx = Mathf.Clamp01(v);
        PlayerPrefs.SetFloat(KEY_SFX, Sfx);
    }

    public static void SetMusic(float v)
    {
        Music = Mathf.Clamp01(v);
        PlayerPrefs.SetFloat(KEY_MUSIC, Music);
    }
}
