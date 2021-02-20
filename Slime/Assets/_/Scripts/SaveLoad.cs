using System;
using UnityEngine;

public static class SaveLoad
{
    public static void SaveBool(string key, bool value) => PlayerPrefs.SetString(key, value.ToString());
    public static void SaveInt(string key, int value) => PlayerPrefs.SetInt(key, value);
    public static void SaveFloat(string key, float value) => PlayerPrefs.SetFloat(key, value);
    public static void SaveString(string key, string value) => PlayerPrefs.SetString(key, value);

    public static bool LoadBool(string key, bool defaultValue = false)
    {
        if (PlayerPrefs.HasKey(key))
            return Convert.ToBoolean(PlayerPrefs.GetString(key));
        return defaultValue;
    }

    public static int LoadInt(string key, int defaultValue = 0)
    {
        if (PlayerPrefs.HasKey(key))
            return PlayerPrefs.GetInt(key);
        return defaultValue;
    }

    public static float LoadFloat(string key, float defaultValue = 0f)
    {
        if (PlayerPrefs.HasKey(key))
            return PlayerPrefs.GetFloat(key);
        return defaultValue;
    }

    public static string LoadString(string key, string defaultValue = null)
    {
        if (PlayerPrefs.HasKey(key))
            return PlayerPrefs.GetString(key);
        return defaultValue;
    }

    public static void SaveAll() => PlayerPrefs.Save();
}
