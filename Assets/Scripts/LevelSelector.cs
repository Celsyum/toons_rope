using TMPro;
using UnityEngine;

public class LevelSelector : MonoBehaviour
{
    public UnityEngine.Events.UnityEvent onPressed;

    public GameObject disabledShield;
    public GameObject pink_button;
    public GameObject blue_button;
    public TextMeshPro label;


    public GameObject[] points;
    

    //setter for level
    public int points_count;
    public int level;

    //setter for unlocked variable
    private bool _unlocked = false;
    public bool unlocked
    {
        get
        {
            return _unlocked;
        }
        set
        {
            foreach (var item in points)
            {
                item.SetActive(value);
            }
            disabledShield.SetActive(!value);

            _unlocked = value;
        }
    }


    



    public void PrepareButton()
    {
        label.text = (level + 1).ToString();
        if (!unlocked)
        {
            return;
        }
        int i = 0;
        for (i = 0; i < points_count; i++)
        {
            GameObject point = Instantiate(blue_button, points[i].transform);
            point.transform.position = points[i].transform.position;
        }
        
        for (int j = i; j < 3; j++)
        {
            GameObject point = Instantiate(pink_button, points[j].transform);
            point.transform.position = points[j].transform.position;
        }   
    }

    void Update()
    {
        if (!unlocked)
        {
            return;
        }
        for (int i = 0; i < Input.touchCount; ++i)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {

                // Construct a ray from the current touch coordinates
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform == this.transform)
                    {
                        GameManager.Instance.levelToLoad = level;
                        onPressed.Invoke();
                    }
                    
                }

                
            }
        }
#if UNITY_EDITOR || !UNITY_ANDROID && !UNITY_IOS 
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == this.transform)
                {
                    GameManager.Instance.levelToLoad = level;
                    onPressed.Invoke();
                }
            }
        }
#endif
    }

}
