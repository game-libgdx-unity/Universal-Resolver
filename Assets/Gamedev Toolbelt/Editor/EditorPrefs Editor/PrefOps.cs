using UnityEditor;

namespace com.immortalhydra.gdtb.epeditor
{
    public static class PrefOps
    {
        public static void AddPref(Pref aPref)
        {
            WindowMain.Prefs.Add(aPref);
            IO.WritePrefsToFile();
            EditorWindow.GetWindow(typeof(WindowMain)).Repaint();
        }

        public static void AddPref(string aKey, bool aValue)
        {
            WindowMain.Prefs.Add(new Pref(PrefType.BOOL, aKey, aValue.ToString()));
            IO.WritePrefsToFile();
            EditorWindow.GetWindow(typeof(WindowMain)).Repaint();
        }

        public static void AddPref(string aKey, int aValue)
        {
            WindowMain.Prefs.Add(new Pref(PrefType.INT, aKey, aValue.ToString()));
            IO.WritePrefsToFile();
            EditorWindow.GetWindow(typeof(WindowMain)).Repaint();
        }

        public static void AddPref(string aKey, float aValue)
        {
            WindowMain.Prefs.Add(new Pref(PrefType.FLOAT, aKey, aValue.ToString()));
            IO.WritePrefsToFile();
            EditorWindow.GetWindow(typeof(WindowMain)).Repaint();
        }

        public static void AddPref(string aKey, string aValue)
        {
            WindowMain.Prefs.Add(new Pref(PrefType.STRING, aKey, aValue));
            IO.WritePrefsToFile();
            EditorWindow.GetWindow(typeof(WindowMain)).Repaint();
        }

        /// Add existing EditorPref to list of prefs.
        /// If the EditorPref doesn't exist, due to the fact that these Unity functions never return errors,
        /// a pref will be added with a default value anyway.
        public static void GetPref(string aKey, PrefType aType)
        {
            string val = "";
            switch (aType)
            {
                case PrefType.BOOL:
                    val = EditorPrefs.GetBool(aKey).ToString();
                    break;
                case PrefType.INT:
                    val = EditorPrefs.GetInt(aKey).ToString();
                    break;
                case PrefType.FLOAT:
                    val = EditorPrefs.GetFloat(aKey).ToString();
                    break;
                case PrefType.STRING:
                    val = EditorPrefs.GetString(aKey);
                    break;
            }
            WindowMain.Prefs.Add(new Pref(aType, aKey, val));
            IO.WritePrefsToFile();
            EditorWindow.GetWindow(typeof(WindowMain)).Repaint();
        }

        /// Remove pref from list of prefs (NOT EditorPrefs).
        public static void RemovePref(string aKey)
        {
            // Iterate backwards by index, a foreach will throw an InvalidOperationException.
            for (var i = WindowMain.Prefs.Count - 1; i >= 0; i--)
            {
                if (WindowMain.Prefs[i].Key == aKey)
                {
                    WindowMain.Prefs.Remove(WindowMain.Prefs[i]);
                }
            }
            IO.WritePrefsToFile();
            EditorWindow.GetWindow(typeof(WindowMain)).Repaint();
        }

        /// Remove pref from list of prefs (NOT EditorPrefs).
        public static void RemovePref(Pref aPref)
        {
            WindowMain.Prefs.Remove(aPref);
            IO.WritePrefsToFile();
            EditorWindow.GetWindow(typeof(WindowMain)).Repaint();
        }

        /// Re-populate the list of prefs from the ones in bak.gdtb.
        public static void RefreshPrefs()
        {
            var storedPrefs = IO.LoadStoredPrefs();
            if (storedPrefs.Count >= WindowMain.Prefs.Count)
            {
                WindowMain.Prefs.Clear();
                WindowMain.Prefs.AddRange(storedPrefs);
            }
        }
    }
}