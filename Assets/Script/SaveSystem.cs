using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;

public class SaveSystem : MonoBehaviour
{
    private static string latestSavePath = "";

    public static void CreateSave()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/" + DateTime.Now.ToString("hh-mm-ss") + ".sav";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, new SaveUnit());
        stream.Close();

        latestSavePath = path;

        Debug.Log(path);
    }

    public static void LoadSave()
    {
        if (File.Exists(latestSavePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(latestSavePath, FileMode.Open);

            SaveUnit data = formatter.Deserialize(stream) as SaveUnit;
            stream.Close();

            Vector3 playerPos = new Vector3(data.playerPosition[0], data.playerPosition[1], data.playerPosition[2]);
            Debug.Log(playerPos);
        }
        else
        {
            Debug.Log("Save Not Found");
        }
    }
}
