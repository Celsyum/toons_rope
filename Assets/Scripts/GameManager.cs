using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        this.LoadGameData();
        Debug.Log(ConfigData.CONFIG_DATA.levels.Count);
    }


    private void LoadGameData()
    {
        string levelData = File.ReadAllText(Path.Combine(Application.persistentDataPath, "level_data.json"));
        //Config.CONFIG_DATA = JsonUtility.FromJson<Config.ConfigData>(configData);
        ConfigData.CONFIG_DATA = JsonUtility.FromJson<ConfigData>(levelData);
        
    }
}
