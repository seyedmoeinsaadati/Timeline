using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using UnityEngine;

namespace Moein.Timeline
{
    public class TimeRecorderFileHandler : MonoBehaviour
    {
        private static TimeRecorderFileHandler instance;
        public static string ASSETS_PATH = "Assets/Resources/";
        public static string MAIN_DIRECTORY = "TimeRecordFiles/";
        public static string RECORD_FILE_EXTENSION = ".tr";

        /// <summary>
        /// save list<T> as a text file in resources folder
        /// </summary>
        public static void Save<T>(string subDirectory, string filename, List<T> list)
        {
            string directory = ASSETS_PATH + MAIN_DIRECTORY + subDirectory + "/";
            filename += RECORD_FILE_EXTENSION;
            if (Directory.Exists(directory) == false)
                Directory.CreateDirectory(directory);

            string path = directory + filename;
            try
            {
                FileStream fs = new FileStream(path, FileMode.Create);
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, list);
                fs.Close();
#if UNITY_EDITOR
                Debug.Log($"File Saved Successfully. {path}");
#endif
            }
            catch (Exception)
            {
#if UNITY_EDITOR
                Debug.LogError($"File Saved Failed. {path}");
#endif
            }
        }

        /// <summary>
        /// load a text file in resources folder as list<T> 
        /// </summary>
        public static List<T> Load<T>(string subDirectory, string fileName)
        {
            string path = MAIN_DIRECTORY + subDirectory + "/" + fileName + RECORD_FILE_EXTENSION;
            try
            {
                TextAsset binaryFile = Resources.Load<TextAsset>(path);
                Stream s = new MemoryStream(binaryFile.bytes);
                BinaryFormatter formatter = new BinaryFormatter();

#if UNITY_EDITOR
                Debug.Log($"File Loaded Successfully. {path}");
#endif

                return (List<T>) formatter.Deserialize(s);
            }
            catch (Exception)
            {
#if UNITY_EDITOR
                Debug.Log($"File Loaded Failed. {path}");
#endif
                return new List<T>();
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void OnRuntimeInitialize()
        {
            instance = new GameObject(nameof(TimeRecorderFileHandler)).AddComponent<TimeRecorderFileHandler>();
            DontDestroyOnLoad(instance.gameObject);
        }
    }
}