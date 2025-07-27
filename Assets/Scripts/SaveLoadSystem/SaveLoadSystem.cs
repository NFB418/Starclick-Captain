using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveLoadSystem: MonoBehaviour
{
    private const string FileType = ".txt";

    private static int SaveCount = 0;
    public static void SaveData<T>(T data, string filename)
    {

        if (SaveCount % 5 == 0) Save(filename + "Backup");
        Save(filename);

        SaveCount++;

        void Save(string savename)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, data);
            string dataToSave = Convert.ToBase64String(stream.ToArray());
            PlayerPrefs.SetString(savename + FileType, dataToSave);
        }
    }

    public static T LoadData<T>(string filename)
    {
        bool backUpNeeded = false;
        T dataToReturn;

        Load(filename);
        if (backUpNeeded) Load(filename + "Backup");

        return dataToReturn;

        void Load(string savename)
        {
            string dataToLoad = PlayerPrefs.GetString(savename + FileType, "");
            if (!string.IsNullOrEmpty(dataToLoad))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                MemoryStream stream = new MemoryStream(Convert.FromBase64String(dataToLoad));

                try
                {
                    dataToReturn = (T)formatter.Deserialize(stream);
                }
                catch
                {
                    backUpNeeded = true;
                    dataToReturn = default;
                }
            }
            else 
            {
                dataToReturn = default;
            }
        }

    }

    public static void DeleteData(string filename)
    {
        if (PlayerPrefs.HasKey(filename + FileType)) { PlayerPrefs.DeleteKey(filename + FileType); }
        if (PlayerPrefs.HasKey(filename + "Backup" + FileType)) { PlayerPrefs.DeleteKey(filename + "Backup" + FileType); }
    }

    public static bool SaveExists(string filename)
    {
        // If one of these exists, returns true, else returns false.
        return (PlayerPrefs.HasKey(filename + FileType)) || (PlayerPrefs.HasKey(filename + "Backup" + FileType));
    }
}
