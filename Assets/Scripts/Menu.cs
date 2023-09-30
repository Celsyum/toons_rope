using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject levelSelector;
    public GameObject navLeft;
    public GameObject navRight;

    private GameManager gameManager;
    private int currentPage = 0;
    private int totalPerPage = 6;

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
            currentPage = gameManager.gameData.current_level / totalPerPage;
            this.PrepareMenu(true);
        }   
    }

    void PrepareMenu(bool fromRight)
    {
        float aspectRatio = (float)Screen.width / Screen.height;
        float screenHalfHeight = Camera.main.orthographicSize;
        float screenHalfWidth = screenHalfHeight * aspectRatio;

        float spacerX = screenHalfWidth * 0.8f * 2f / 3f;
        float marginX = screenHalfWidth * 0.4f;
        float spacerY = screenHalfHeight * 0.8f * 2f / 3f;
        float marginY = screenHalfHeight * 0.4f;

        Vector3 topLeft = new Vector3(-screenHalfWidth, screenHalfHeight, 0);

        int items = 0;
        int i = 0;
        //add current page and total page
        for ( i = currentPage * totalPerPage; i < ConfigData.CONFIG_DATA.levels.Count; i++)
        {
            List<string> level_data = ConfigData.CONFIG_DATA.levels[i].level_data;
            GameObject button = Instantiate(levelSelector, transform);
            LevelSelector levSelector = button.GetComponent<LevelSelector>();
            levSelector.points_count = gameManager.gameData.completion.Count > i ? gameManager.gameData.completion[i] : 0;
            levSelector.level = i;
            if (gameManager.gameData.current_level >= i) levSelector.unlocked = true;
            button.name = "Level" + (i + 1);
            levSelector.PrepareButton();

            int x = i % 3;
            int y = items / 3;

            //tween button.transform.localPosition to  new Vector3(topLeft.x + marginX + x * spacerX, topLeft.y - marginY - y * spacerY);   from top
            Vector3 pos = new Vector3(topLeft.x + marginX + x * spacerX, topLeft.y - marginY - y * spacerY);
            pos.x += fromRight ? -30f : 30f;
            button.transform.localPosition = pos;
            iTween.MoveTo(button, iTween.Hash("position", new Vector3(topLeft.x + marginX + x * spacerX, topLeft.y - marginY - y * spacerY), "time", 1f, "delay", items * 0.1f, "easetype", iTween.EaseType.easeOutBack));

            levSelector.onPressed.AddListener(() =>
            {
                SceneManager.LoadScene("Game");
            });

            selectors.Add(button);

            items++;

            if (items >= totalPerPage)
            {
                break;
            }
        }

        if (i < ConfigData.CONFIG_DATA.levels.Count-1)
        {
            navRight.GetComponent<OffScreenTweener>().StartAnimation();
            navRight.SetActive(true);
        }
        else
        {
            navRight.SetActive(false);
        }
        if (currentPage > 0)
        {
            navLeft.GetComponent<OffScreenTweener>().StartAnimation();
            navLeft.SetActive(true);
        }
        else
        {
            navLeft.SetActive(false);
        }
        
    }

    public void OnNavLeft()
    {
        Debug.Log("OnNavLeft");
        currentPage--;
        this.OnNav(true);
    }

    public void OnNavRight()
    {
        Debug.Log("OnNavRight");
        currentPage++;
        this.OnNav(false);
    }

    void OnNav(bool moveRight)
    {
        //animate all buttons to the direction of movieRight with delay of each button
        for (int i = 0; i < selectors.Count; i++)
        {
            Vector3 pos = selectors[i].transform.localPosition;
            pos.x += moveRight ? 30f : -30f;
            iTween.MoveTo(selectors[i], iTween.Hash("position", pos, "time", 1f, "delay", i * 0.1f, "easetype", iTween.EaseType.easeOutBack, "oncomplete", "MenuAnimationsEnded", "oncompletetarget", gameObject, "oncompleteparams", moveRight));
        }
    }

    void MenuAnimationsEnded(bool moveRight)
    {
        for (int i = 0; i < selectors.Count; i++)
        {
            Destroy(selectors[i]);
        }
        selectors.Clear();

        this.PrepareMenu(moveRight);
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
