using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem {
    
    public static void Save(int level, float masterVolume, float sfxVolume, float musicVolume, float ambientVolume,
        float uiVolume) {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/hideData";

        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData(level, masterVolume, sfxVolume, musicVolume, ambientVolume,
            uiVolume);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SaveData Load() {
        string path = Application.persistentDataPath + "/hideData";
        if(File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            return data;
        }
        else {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

}
