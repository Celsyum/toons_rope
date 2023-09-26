using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public GameData gameData;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        this.LoadLevelData();
        GameData game_data = this.LoadState();
        if (game_data != null)
        {
            this.gameData = game_data;
        }
        else
        {
            this.gameData = new GameData(this);
        }   
    }


    private void LoadLevelData()
    {
        Debug.Log(Application.persistentDataPath);
        string levelData = File.ReadAllText(Path.Combine(Application.persistentDataPath, "level_data.json"));
        ConfigData.CONFIG_DATA = JsonUtility.FromJson<ConfigData>(levelData);
    }

    public static void SaveState(GameManager manager)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string fullPath = Path.Combine(Application.persistentDataPath, "ropes.save");
        FileStream stream = new FileStream(fullPath, FileMode.Create);

        GameData data = new GameData(manager);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    private GameData LoadState()
    {
        string fullPath = Path.Combine(Application.persistentDataPath, "ropes.save");
        if (File.Exists(fullPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(fullPath, FileMode.Open);

            GameData data = formatter.Deserialize(stream) as GameData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.Log("File not found, continue new");
            return null;
        }
    }
}
