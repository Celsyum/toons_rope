using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }

    [HideInInspector]
    public GameData gameData;

    [HideInInspector]
    public UnityEngine.Events.UnityEvent onLevelDataLoaded;

    public int levelToLoad = 0;

     private void Awake()
    {
        Debug.Log(Application.persistentDataPath);
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        this.LoadLevelData();

        DontDestroyOnLoad(gameObject); //not needed if you remove GameManager from other scenes
    }

    public void ResetLevel()
    {
        levelToLoad = 0;
        gameData.current_level = 0;
        gameData.completion = new List<int>();
        SaveState(GameManager.Instance);
    }

    public void LoadLevelData()
    {
        StartCoroutine(GetText(Path.Combine(Application.streamingAssetsPath, "level_data.json")));
    }

    IEnumerator GetText(string uri)
    {
        UnityWebRequest www = new UnityWebRequest(uri);
        www.downloadHandler = new DownloadHandlerBuffer();
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            ConfigData.CONFIG_DATA = JsonUtility.FromJson<ConfigData>(www.downloadHandler.text);

            GameData game_data = this.LoadState();
            if (game_data != null)
            {
                this.gameData = game_data;
            }
            else
            {
                this.gameData = new GameData(this);
            }

            onLevelDataLoaded.Invoke();
        }
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
