using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelManager : MonoBehaviour
{
    List<GameObject> points = new List<GameObject>();

    public GameObject pink_button;
    public GameObject blue_button;

    // Start is called before the first frame update
    void Start()
    {
        if (ConfigData.CONFIG_DATA != null)
        {
            this.PrepareLevel();
        }
        else
        {
            // just for testing from game scene
            GameManager.Instance.onLevelDataLoaded.AddListener(this.PrepareLevel);
            GameManager.Instance.LoadLevelData();
        }
    }

    void PrepareLevel()
    {
        //GameManager.Instance.level_to_load
        //GameManager.Instance.gameData

        //ConfigData.CONFIG_DATA.levels[GameManager.Instance.level_to_load].level_data

        for (int i = 0; i < ConfigData.CONFIG_DATA.levels[GameManager.Instance.levelToLoad].level_data.Count-1; i+=2)
        {
            GameObject point = Instantiate(pink_button, transform);
            point.transform.position = GetPointPosition(float.Parse(ConfigData.CONFIG_DATA.levels[GameManager.Instance.levelToLoad].level_data[i]), float.Parse(ConfigData.CONFIG_DATA.levels[GameManager.Instance.levelToLoad].level_data[i + 1]));
            points.Add(point);
            
        }
    }

    Vector2 GetPointPosition(float posX, float posY)
    {
        float aspectRatio = (float)Screen.width / Screen.height;
        float screenHalfHeight = Camera.main.orthographicSize;
        float screenHalfWidth = screenHalfHeight * aspectRatio;

        float spacerX = screenHalfWidth * 2f / 1000;
        float spacerY = screenHalfHeight * 2f / 1000;

        Vector3 topLeft = new Vector3(-screenHalfWidth, screenHalfHeight, 0);

        return new Vector2(topLeft.x + spacerX * posX, topLeft.y - spacerY * posY);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnResizedWindow()
    {

    }
}
