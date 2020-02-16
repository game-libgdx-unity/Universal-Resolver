using System.Collections.Generic;
using System.IO;
using System;

namespace com.immortalhydra.gdtb.epeditor
{
    public static class IO
    {
        /// Write Prefs in memory to the backup file.
        public static void WritePrefsToFile()
        {
            var tempFile = Path.GetTempFileName();
            var bakFile = GetFirstInstanceOfFolder("EditorPrefs Editor") + "/bak.gdtb";

            var writer = new StreamWriter(tempFile, false);
            try
            {
                foreach (var pref in WindowMain.Prefs)
                {
                    var type = pref.Type.ToString();
                    var key = pref.Key;
                    var line = type + "|" + key;
                    writer.WriteLine(line);
                }
                writer.Close();
            }
            catch (Exception)
            {
                writer.Dispose();
            }

            if (File.Exists(bakFile))
            {
                File.Delete(bakFile);
            }
            File.Move(tempFile, bakFile);
        }

        /// Load Prefs from the backup.
        public static List<Pref> LoadStoredPrefs()
        {
            var backedPrefs = new List<Pref>();

            var bakFile = GetFirstInstanceOfFolder("EditorPrefs Editor") + "/bak.gdtb";

            if (File.Exists(bakFile))
            {
                // Parse the document for exclusions.
                var reader = new StreamReader(bakFile);
                try
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.StartsWith("#") || String.IsNullOrEmpty(line) || line == " ")
                            // If the line is a comment, is empty, or is a single space, ignore them.
                        {
                            continue;
                        }
                        else
                        {
                            var pref = ParsePref(line);
                            if (pref != null)
                            {
                                backedPrefs.Add(pref);
                            }
                        }
                    }
                    reader.Close();
                }
                catch (Exception)
                {
                    reader.Dispose();
                }
            }
            return backedPrefs;
        }

        /// Empty the bak file.
        public static void ClearStoredPrefs()
        {
            var bakFile = GetFirstInstanceOfFolder("EditorPrefs Editor") + "/bak.gdtb";
            if (File.Exists(bakFile))
            {
                System.IO.File.WriteAllText(bakFile, string.Empty);
            }
        }

        /// Overwrite the shortcut code in WindowMain.
        public static void OverwriteShortcut(string aShortcut)
        {
            var tempFile = Path.GetTempFileName();
            var file = GetFilePath("Gamedev Toolbelt/Editor/EditorPrefs Editor/WindowMain.cs");

            var writer = new StreamWriter(tempFile, false);
            var reader = new StreamReader(file);

            try
            {
                var line = "";
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains("[MenuItem"))
                    {
                        writer.WriteLine("        [MenuItem(" + '"' +
                                         "Window/Gamedev Toolbelt/EditorPrefs Editor/Open EditorPrefs Editor " +
                                         aShortcut + '"' + ", false, 1)]");
                    }
                    else
                    {
                        writer.WriteLine(line);
                    }
                }
                reader.Close();
                writer.Close();

                // Overwrite the old file with the temp file.
                File.Delete(file);
                File.Move(tempFile, file);
                UnityEditor.AssetDatabase.ImportAsset(file);
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log(ex.Message);
                UnityEngine.Debug.Log(ex.Data);
                UnityEngine.Debug.Log(ex.StackTrace);
                reader.Dispose();
                writer.Dispose();
            }
        }




        /// Return the first instance of the given folder.
        /// This is a non-recursive, breadth-first search.
        private static string GetFirstInstanceOfFolder(string aFolderName)
        {
            var projectDirectoryPath = Directory.GetCurrentDirectory();
            var projectDirectoryInfo = new DirectoryInfo(projectDirectoryPath);
            var listOfAssetsDirs = projectDirectoryInfo.GetDirectories("Assets");
            var assetsDir = "";

            // 1. Get the Assets dir.
            foreach (var dir in listOfAssetsDirs)
            {
                if (dir.FullName.EndsWith("\\Assets"))
                {
                    assetsDir = dir.FullName;
                }
            }
            var path = assetsDir;

            var q = new Queue<string>();
            q.Enqueue(path);
            var absolutePath = "";

            // 2. While there's still folders in the queue:
            while (q.Count > 0)
            {
                path = q.Dequeue();
                // 2a. Get sub directories of the current folder and add them to the queue.
                try
                {
                    foreach (string subDir in Directory.GetDirectories(path))
                    {
                        q.Enqueue(subDir);
                    }
                }
                catch (Exception)
                {
                } // If there are problems, they're not folders, so ignore them.

                // Get the folder we were looking for in the complete list.
                string[] folders = null;
                try
                {
                    folders = Directory.GetDirectories(path);
                }
                catch (Exception)
                {
                }

                if (folders != null)
                {
                    foreach (string folder in folders)
                    {
                        if (folder.EndsWith(aFolderName))
                        {
                            absolutePath = folder;
                            break;
                        }
                    }
                }
            }
            // Get the path of the folder we're interested in.
            var relativePath = absolutePath.Remove(0, projectDirectoryPath.Length + 1);
            return relativePath;
        }

        /// Get the path of a file based on the ending provided.
        private static string GetFilePath(string aPathEnd)
        {
            var assetsPaths = UnityEditor.AssetDatabase.GetAllAssetPaths();
            var filePath = "";
            foreach (var path in assetsPaths)
            {
                if (path.EndsWith(aPathEnd))
                {
                    filePath = path;
                    break;
                }
            }
            return filePath;
        }

        /// Parse a line in the backup file.
        private static Pref ParsePref(string aString)
        {
            var parts = aString.Split('|');

            // Get the key.
            var key = parts[1].Replace("(U+007C)", "|");

            // If the key doesn't exist, no sense in adding it.
            if (!NewEditorPrefs.HasKey(key))
            {
                return null;
            }

            // Get the type.
            var typeString = parts[0];
            PrefType type;

            Pref pref;
            switch (typeString)
            {
                case "BOOL":
                    type = PrefType.BOOL;
                    var boolValue = NewEditorPrefs.GetBool(key, false);
                    pref = new Pref(type, key, boolValue.ToString());
                    break;
                case "INT":
                    type = PrefType.INT;
                    var intValue = NewEditorPrefs.GetInt(key, 0);
                    pref = new Pref(type, key, intValue.ToString());
                    break;
                case "FLOAT":
                    type = PrefType.FLOAT;
                    var floatValue = NewEditorPrefs.GetFloat(key, 0.0f);
                    pref = new Pref(type, key, floatValue.ToString());
                    break;
                case "STRING":
                default:
                    type = PrefType.STRING;
                    var stringValue = NewEditorPrefs.GetString(key, "");
                    pref = new Pref(type, key, stringValue);
                    break;
            }
            return pref;
        }
    }
}