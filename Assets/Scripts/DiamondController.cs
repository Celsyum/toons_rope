using System.Collections;
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
    public GameObject label;
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

    public void OnCorrectPressed()
    {
        background.sprite = Sprite.Create(texturePath, new Rect(0, 0, texturePath.width, texturePath.height), new Vector2(0.5f, 0.5f), 100f);
        //tween label alpha to 0
        iTween.ValueTo(label, iTween.Hash("from", 1f, "to", 0f, "time", 1f, "onupdate", "OnLabelTweenUpdate", "oncomplete", "OnLabelTweenEnded"));
        
        StartCoroutine(DelayedColliderDisable());
    }

    IEnumerator DelayedColliderDisable()
    {
        yield return new WaitForSeconds(0.2f);
        background.GetComponent<SphereCollider>().enabled = false;
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
