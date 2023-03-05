using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using UnityEngine;

namespace Moein.TimeSystem
{
    public class TimeRecorderFileHandler : MonoBehaviour
    {
        [SerializeField] public bool debug = true;
        private static TimeRecorderFileHandler instance;
        private readonly static string ASSETS_PATH = "Assets/Resources/";
        private readonly static string MAIN_DIRECTORY = "TimelineFiles/";
        private readonly static string RECORD_FILE_EXTENSION = ".bytes";

        /// <summary>
        /// save list<T> as a text file in resources folder
        /// </summary>
        public static void Save<T>(string subDirectory, string filename, List<T> list)
        {
            string directory = ASSETS_PATH + MAIN_DIRECTORY + subDirectory + "/";
            if (Directory.Exists(directory) == false) Directory.CreateDirectory(directory);
            string path = directory + filename + RECORD_FILE_EXTENSION;
            try
            {
                FileStream fs = new FileStream(path, FileMode.Create);
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, list);
                fs.Close();
#if UNITY_EDITOR
                if (instance.debug) Debug.Log($"File Saved Successfully. {path}");
#endif
            }
            catch (Exception)
            {
#if UNITY_EDITOR
                if (instance.debug) Debug.LogError($"File Saved Failed. {path}");
#endif
            }
        }

        /// <summary>
        /// load a text file in resources folder as list<T> 
        /// </summary>
        public static List<T> Load<T>(string subDirectory, string fileName)
        {
            string path = MAIN_DIRECTORY + subDirectory + "/" + fileName; // + RECORD_FILE_EXTENSION;
            try
            {
                TextAsset binaryFile = (TextAsset)Resources.Load(path);
                Stream s = new MemoryStream(binaryFile.bytes);
                BinaryFormatter formatter = new BinaryFormatter();

#if UNITY_EDITOR
                if (instance.debug) Debug.Log($"File Loaded Successfully. {path}");
#endif

                return (List<T>)formatter.Deserialize(s);
            }
            catch (Exception e)
            {
#if UNITY_EDITOR
                if (instance.debug) Debug.LogError($"File Loaded Failed. {path}, {e.Message}");
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