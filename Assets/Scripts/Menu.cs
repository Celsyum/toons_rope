using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject levelSelector;
    public Camera mainCamera;

    private GameManager gameManager;

    private List<GameObject> selectors = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;    
     
        StartCoroutine(isDataLoaded());        
    }

    IEnumerator isDataLoaded()
    {
        if (ConfigData.CONFIG_DATA == null)
        {
            yield return new WaitForSeconds(0.2f);
            StartCoroutine(isDataLoaded());
        }
        else
        {
            this.PrepareMenu();
        }   
    }

    void PrepareMenu()
    {
        float aspectRatio = (float)Screen.width / Screen.height;
        float screenHalfHeight = Camera.main.orthographicSize;
        float screenHalfWidth = screenHalfHeight * aspectRatio;

        float spacerX = screenHalfWidth * 0.8f * 2f / 3f;
        float marginX = screenHalfWidth * 0.4f;
        float spacerY = screenHalfHeight * 0.8f * 2f / 3f;
        float marginY = screenHalfHeight * 0.4f;

        Vector3 topLeft = new Vector3(-screenHalfWidth, screenHalfHeight, 0);

        //ConfigData.CONFIG_DATA.levels
        for (int i = 0; i < ConfigData.CONFIG_DATA.levels.Count; i++)
        {
            List<string> level_data = ConfigData.CONFIG_DATA.levels[i].level_data;
            GameObject button = Instantiate(levelSelector, transform);
            LevelSelector levSelector = button.GetComponent<LevelSelector>();
            levSelector.points_count = gameManager.gameData.completion.Contains(i) ? gameManager.gameData.completion[i] : 0;
            levSelector.level = i;

            button.name = "Level" + (i + 1);
            levSelector.PrepareButton();

            // position buttons in a grid of 3x3 centered in the screen
            int x = i % 3;
            int y = i / 3;

            

            button.transform.localPosition = new Vector3(topLeft.x + marginX + x * spacerX, topLeft.y - marginY - y * spacerY);

            //listen to button press
            levSelector.onPressed.AddListener(() =>
            {
                SceneManager.LoadScene("Game");
            });

            selectors.Add(button);
        }
        
    }

    public void OnResizedWindow()
    {
        //realign buttons
        float aspectRatio = (float)Screen.width / Screen.height;
        float screenHalfHeight = Camera.main.orthographicSize;
        float screenHalfWidth = screenHalfHeight * aspectRatio;

        float spacerX = screenHalfWidth * 0.8f * 2f / 3f;
        float marginX = screenHalfWidth * 0.4f;
        float spacerY = screenHalfHeight * 0.8f * 2f / 3f;
        float marginY = screenHalfHeight * 0.4f;

        Vector3 topLeft = new Vector3(-screenHalfWidth, screenHalfHeight, 0);

        for (int i = 0; i < selectors.Count; i++)
        {
            int x = i % 3;
            int y = i / 3;

            selectors[i].transform.localPosition = new Vector3(topLeft.x + marginX + x * spacerX, topLeft.y - marginY - y * spacerY);
        }
    }
}
