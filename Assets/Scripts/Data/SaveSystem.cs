using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string SavePath => Path.Combine(Application.persistentDataPath, "gamedata.json");

    public static void Save(GameDataSO data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);
    }

    public static void Load(GameDataSO data)
    {
        if (File.Exists(SavePath))
        {
            string json = File.ReadAllText(SavePath);
            JsonUtility.FromJsonOverwrite(json, data);
        }
        else
        {
            Debug.Log("Сохранений нет, используем пустой GameDataSO");
        }
    }
}
