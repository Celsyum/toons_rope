using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLevelManager : MonoBehaviour
{
    List<GameObject> points = new List<GameObject>();

    public GameObject pinkButton;
    public RopeManager ropeManager;
    public RectTransform safeArea;

    public GameObject backButton;

    private int lastPressed = -1;

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
            //GameManager.Instance.LoadLevelData();
        }
    }

    void PrepareLevel()
    {
        Rect safeRect = CalculateSafeRectForView();

       for (int i = 0; i < ConfigData.CONFIG_DATA.levels[GameManager.Instance.levelToLoad].level_data.Count-1; i+=2)
       {
           GameObject point = Instantiate(pinkButton, transform);
           DiamondController diamondC = point.GetComponent<DiamondController>();
            if (i == 0)
            {
                ropeManager.firstDiamond = diamondC;
            }
           diamondC.onPressed.AddListener(this.OnPointClicked);
           diamondC.queue = points.Count;
           point.transform.position = GetPointPosition(float.Parse(ConfigData.CONFIG_DATA.levels[GameManager.Instance.levelToLoad].level_data[i]), float.Parse(ConfigData.CONFIG_DATA.levels[GameManager.Instance.levelToLoad].level_data[i + 1]), i, safeRect);
           points.Add(point);       
       }

       ropeManager.onAllRopeAnimationsDone.AddListener(this.OnAllRopeAnimationsDone);
    }

    void OnAllRopeAnimationsDone()
    {
        Debug.Log("All rope animations done");
        backButton.GetComponent<OffScreenTweener>().StartAnimation();
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

            if (lastPressed == points.Count - 1)
            {
                Debug.Log("Level completed");
                ropeManager.lastDiamond = true;
            }
        }
        else
        {
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

        return new Vector3(safeRect.x + spacerX * posX, safeRect.y - spacerY * posY, i / 1000f);
    }

    public void OnResizedWindow()
    {

    }
}
