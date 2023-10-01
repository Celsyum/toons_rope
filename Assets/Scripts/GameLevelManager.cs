using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLevelManager : MonoBehaviour
{
    List<GameObject> points = new List<GameObject>();

    public GameObject pinkButton;
    public RopeManager ropeManager;
    public RectTransform safeArea;

    public GameObject backButton;
    public Image background;

    private int lastPressed = -1;

    private int score = 3;

    // Start is called before the first frame update
    void Start()
    {
        if (ConfigData.CONFIG_DATA != null)
        {
            this.PrepareLevel();
        }
        else
        {
            GameManager.Instance.onLevelDataLoaded.AddListener(this.PrepareLevel);        
        }

        background.material.SetFloat("_Force", 1f);
    }

    void PrepareLevel()
    {
        Rect safeRect = CalculateSafeRectForView();

        List<string> levelData = ConfigData.CONFIG_DATA.levels[GameManager.Instance.levelToLoad].level_data;
        for (int i = 0; i < levelData.Count-1; i+=2)
        {
            GameObject point = Instantiate(pinkButton, transform);
            point.name = "Diamond"+ (i/2);
            DiamondController diamondC = point.GetComponent<DiamondController>();
            if (i == 0)
            {
                ropeManager.firstDiamond = diamondC;
            }
            diamondC.onPressed.AddListener(this.OnPointClicked);
            diamondC.queue = points.Count;
            if (levelData.Count > 20) diamondC.make_centered = true;
            point.transform.position = GetPointPosition(float.Parse(levelData[i]), float.Parse(levelData[i + 1]), i, safeRect);
            points.Add(point);       
       }

       ropeManager.onAllRopeAnimationsDone.AddListener(this.OnAllRopeAnimationsDone);
    }

    void OnAllRopeAnimationsDone()
    {
        Debug.Log("All rope animations done");
        backButton.GetComponent<OffScreenTweener>().StartAnimation();
        // save game data
        // check if gamedat.completion[levelToLoad] exists
        if (GameManager.Instance.gameData.completion.Count > GameManager.Instance.levelToLoad)
        {
            if (GameManager.Instance.gameData.completion[GameManager.Instance.levelToLoad] < score)
                GameManager.Instance.gameData.completion[GameManager.Instance.levelToLoad] = score < 0 ? 0 : score;
        } else GameManager.Instance.gameData.completion.Add(score < 0 ? 0 : score);
        if (score == 3 && GameManager.Instance.gameData.current_level <= GameManager.Instance.levelToLoad && GameManager.Instance.gameData.current_level < ConfigData.CONFIG_DATA.levels.Count-1) GameManager.Instance.gameData.current_level = GameManager.Instance.levelToLoad + 1;
        GameManager.SaveState(GameManager.Instance);
    }

    
    public void LoadScene(string name)
    {         
        SceneManager.LoadScene(name);
     }

    void OnPointClicked(DiamondController diamond)
    { 
        if (lastPressed + 1 == diamond.queue)
        {
            ropeManager.AddController(diamond);
            
            diamond.OnCorrectPressed();
            lastPressed++;
            AudioManager.instance.PlaySFX("click");

            if (lastPressed == points.Count - 1)
            {
                Debug.Log("Level completed");
                AudioManager.instance.PlaySFX("levelEnd");
                AudioManager.instance.musicSource.Stop();
                ropeManager.lastDiamond = true;
            }
        }
        else
        {
            score--;
            Debug.Log("Wrong point");
        }
    }

    Rect CalculateSafeRectForView()
    {
        Vector3[] safeAreaCorners = new Vector3[4];
        safeArea.GetWorldCorners(safeAreaCorners);
        Vector3[] safeAreaCornersParent = new Vector3[4];
        safeArea.parent.GetComponent<RectTransform>().GetWorldCorners(safeAreaCornersParent);

        float AreaParentHeight = safeAreaCornersParent[1].y - safeAreaCornersParent[0].y;
        float AreaParentWidth = safeAreaCornersParent[2].x - safeAreaCornersParent[1].x;

        float AreaHeight = safeAreaCorners[1].y - safeAreaCorners[0].y;
        float AreaWidth = safeAreaCorners[2].x - safeAreaCorners[1].x;

        float parentPositionX = safeAreaCornersParent[0].x;
        float parentPositionY = safeAreaCornersParent[0].y;

        float safeAreaPositionX = safeAreaCorners[0].x;
        float safeAreaPositionY = safeAreaCorners[0].y;

        float leftMargin = (safeAreaPositionX - parentPositionX) / AreaParentWidth;
        float topMargin = (safeAreaPositionY - parentPositionY) / AreaParentHeight;

        float safeAspectRatio = AreaWidth / AreaHeight;
        float ScreenRatio = (float)Screen.width / Screen.height;
        float screenHeight = Camera.main.orthographicSize * 2f;
        float screenWidth = screenHeight * ScreenRatio;

        float safeScreenHeight = Camera.main.orthographicSize * 2f - topMargin * 4f * Camera.main.orthographicSize;
        float safeScreenWidth = safeScreenHeight * safeAspectRatio;

        float leftSafeScreen = -screenWidth /2f + screenWidth * leftMargin;
        float topSafeScreen = Camera.main.orthographicSize * (1f-topMargin);


        return new Rect(leftSafeScreen, topSafeScreen, safeScreenWidth, safeScreenHeight);

    }

    Vector3 GetPointPosition(float posX, float posY, int i, Rect safeRect)
    {
        //use safe area

        float aspectRatio = (float)safeRect.width / safeRect.height;
        float screenHalfHeight = safeRect.height/2f;
        
        float screenHalfWidth = screenHalfHeight * aspectRatio;

        float spacerX = screenHalfWidth * 2f / 1000f;
        float spacerY = screenHalfHeight * 2f / 1000f;

        return new Vector3(safeRect.x + spacerX * posX, safeRect.y - spacerY * posY, i / 100f);
    }

    public void OnResizedWindow()
    {

    }
}
