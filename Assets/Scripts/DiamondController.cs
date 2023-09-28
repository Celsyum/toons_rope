using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ClickEvent : UnityEvent<DiamondController>
{
}

public class DiamondController : MonoBehaviour
{
    public SpriteRenderer background;
    public ClickEvent onPressed;
    public Texture2D texturePath;
    public TextMeshPro text;
    [HideInInspector]
    public int _queue = 0;

    public int queue
    {
        get
        {
            return _queue;
        }
        set
        {
            _queue = value;
            text.text = (_queue+1).ToString();
        }
    }

    
    void Start()
    {
        //gameObject.GetComponent<SpriteRenderer>()
        //change sprite
        
    }

    public void OnCorrectPressed()
    {
        background.sprite = Sprite.Create(texturePath, new Rect(0, 0, texturePath.width, texturePath.height), new Vector2(0.5f, 0.5f), 100f);
    }

    void Update()
    {
        for (int i = 0; i < Input.touchCount; ++i)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {

                // Construct a ray from the current touch coordinates
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform == background.transform)
                    {
                        onPressed.Invoke(this);
                    }

                }


            }
        }
#if UNITY_EDITOR || !UNITY_ANDROID && !UNITY_IOS 
        //detect mouse click
        if (Input.GetMouseButtonDown(0))
        {
            // Construct a ray from the current mouse coordinates
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Create a particle if hit
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == background.transform)
                {
                    onPressed.Invoke(this);
                }
            }
        }
#endif
    }
}
