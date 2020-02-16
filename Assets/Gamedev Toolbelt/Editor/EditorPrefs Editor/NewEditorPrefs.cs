using UnityEditor;
using com.immortalhydra.gdtb.epeditor;

public static class NewEditorPrefs
{
    #region bools

    /// Sets the value of the preference identified by key.
    public static void SetBool(string aKey, bool aValue)
    {
        EditorPrefs.SetBool(aKey, aValue);
        var duplicate = CheckDuplicate(aKey, PrefType.BOOL);
        if (duplicate != null)
        {
            duplicate.Value = aValue.ToString();
        }
        else
        {
            PrefOps.AddPref(aKey, aValue);
        }
    }

    /// Returns the value corresponding to key in the preference file if it exists (false otherwise).
    public static bool GetBool(string aKey, bool aDefaultValue = false)
    {
        return EditorPrefs.GetBool(aKey, aDefaultValue);
    }

    #endregion

    #region ints

    /// Sets the value of the preference identified by key as an integer.
    public static void SetInt(string aKey, int aValue)
    {
        EditorPrefs.SetInt(aKey, aValue);
        var duplicate = CheckDuplicate(aKey, PrefType.INT);
        if (duplicate != null)
        {
            duplicate.Value = aValue.ToString();
        }
        else
        {
            PrefOps.AddPref(aKey, aValue);
        }
    }

    /// Returns the value corresponding to key in the preference file if it exists (0 otherwise).
    public static int GetInt(string aKey, int aDefaultValue = 0)
    {
        return EditorPrefs.GetInt(aKey, aDefaultValue);
    }

    #endregion

    #region floats

    /// Sets the value of the preference identified by key.
    public static void SetFloat(string aKey, float aValue)
    {
        EditorPrefs.SetFloat(aKey, aValue);
        var duplicate = CheckDuplicate(aKey, PrefType.FLOAT);
        if (duplicate != null)
        {
            duplicate.Value = aValue.ToString();
        }
        else
        {
            PrefOps.AddPref(aKey, aValue);
        }
    }

    /// Returns the value corresponding to key in the preference file if it exists (0.0f otherwise).
    public static float GetFloat(string aKey, float aDefaultValue = 0.0f)
    {
        return EditorPrefs.GetFloat(aKey, aDefaultValue);
    }

    #endregion

    #region strings

    /// Sets the value of the preference identified by key.
    public static void SetString(string aKey, string aValue)
    {
        EditorPrefs.SetString(aKey, aValue);
        var duplicate = CheckDuplicate(aKey, PrefType.STRING);
        if (duplicate != null)
        {
            duplicate.Value = aValue;
        }
        else
        {
            PrefOps.AddPref(aKey, aValue);
        }
    }

    /// Returns the value corresponding to key in the preference file if it exists ("" otherwise).
    public static string GetString(string aKey, string aDefaultValue = "")
    {
        return EditorPrefs.GetString(aKey, aDefaultValue);
    }

    #endregion

    /// Removes all keys and values from EditorPrefs and EPEditor. Use with caution.
    public static void DeleteAll()
    {
        EditorPrefs.DeleteAll();
        WindowMain.Prefs.Clear();
        IO.ClearStoredPrefs();
    }


    /// Removes key and its corresponding value from EditorPrefs and EPEditor.
    public static void DeleteKey(string aKey)
    {
        EditorPrefs.DeleteKey(aKey);
        PrefOps.RemovePref(aKey);
    }


    /// Returns true if key exists in the preferences.
    public static bool HasKey(string aKey)
    {
        return EditorPrefs.HasKey(aKey);
    }


    /// Returns true if a pref is found in WindowMain.Prefs with the same key and type.
    private static Pref CheckDuplicate(string aKey, PrefType aType)
    {
        foreach (var pref in WindowMain.Prefs)
        {
            if (pref.Key == aKey && pref.Type == aType)
            {
                return pref;
            }
        }
        return null;
    }
}