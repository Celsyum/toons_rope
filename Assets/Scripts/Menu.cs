using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameObject levelSelector;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;    
        this.PrepareMenu();
        
    }

    void PrepareMenu()
    {
        //ConfigData.CONFIG_DATA.levels
        for (int i = 0; i < ConfigData.CONFIG_DATA.levels.Count; i++)
        {
            List<string> level_data = ConfigData.CONFIG_DATA.levels[i].level_data;
            GameObject button = Instantiate(levelSelector, transform);
            button.GetComponent<LevelSelector>().points_count = gameManager.gameData.completion.Contains(i) ? gameManager.gameData.completion[i] : 0;
            button.name = "Level " + (i + 1);
            button.GetComponent<LevelSelector>().PrepareButton();

            // position buttons in a grid of 3x3 centered in the screen
            int x = i % 3;
            int y = i / 3;
            //Screen.mainWindowPosition
            Camera.main.ScreenToWorldPoint(new Vector3(Screen.mainWindowPosition.x, Screen.mainWindowPosition.y, 0));

            float marginX = Screen.mainWindowDisplayInfo.width * 0.1f;
            float marginY = Screen.mainWindowDisplayInfo.height * 0.1f;
            float spacerX = Screen.mainWindowDisplayInfo.width * 0.8f / 3;
            float spacerY = Screen.mainWindowDisplayInfo.height * 0.8f / 3;
            

            button.transform.localPosition = new Vector3(Screen.mainWindowPosition.x + marginX + spacerX * x , Screen.mainWindowPosition.y - marginY - spacerY * y - Screen.height, 0);

        }   
    }

    void OnRectTransformDimensionsChange()
    {
        Debug.Log("OnRectTransformDimensionsChange");
    }
}
