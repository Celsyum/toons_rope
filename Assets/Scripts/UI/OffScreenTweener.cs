using UnityEngine;

public class OffScreenTweener : MonoBehaviour
{
    public bool autoplay = false;
    public bool animationIn = true;
    public Vector2 outDirection = Vector2.zero;
    public RectTransform area;

    private Vector3 _originalPosition;

    void Awake()
    {
        _originalPosition = transform.localPosition;
        if (animationIn)
        {                  
            //Debug.Log(area.rect);
            Debug.Log(area.parent.localScale);
            Debug.Log(transform.localScale);
            Debug.Log((transform as RectTransform).rect.width * transform.localScale.x);
            
            transform.localPosition = GetPositionForDirection();
        }
    }

    Vector3 GetPositionForDirection()
    {
        Rect parentRect = area.parent.GetComponent<RectTransform>().rect;
        Vector3 position = _originalPosition;
        if (outDirection.x < 0)
        {
            position.x = parentRect.xMin - ((transform as RectTransform).rect.width * transform.localScale.x) - 3f;
        }
        else if (outDirection.x > 0)
        {
            position.x = parentRect.xMax + ((transform as RectTransform).rect.width * transform.localScale.x) + 3f;
        }

        if (outDirection.y < 0)
        {
            position.y = area.rect.yMin - ((transform as RectTransform).rect.height * transform.localScale.y) - 3f;
        }
        else if (outDirection.y > 0)
        {
            position.y = area.rect.yMax + ((transform as RectTransform).rect.height * transform.localScale.y) + 3f;
        }

        return position;

    }

    // Start is called before the first frame update
    void Start()
    {
        if (autoplay)
        {
            StartAnimation();
        }
    }

    public void StartAnimation()
    {
        if (animationIn)
        {
            iTween.MoveTo(gameObject, iTween.Hash("position", _originalPosition, "isLocal", true , "time", 1f, "easetype", iTween.EaseType.easeOutBack));
        }
        else
        {
            iTween.MoveTo(gameObject, iTween.Hash("position", GetPositionForDirection(), "isLocal", true, "time", 1f, "easetype", iTween.EaseType.easeInBack));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (animationIn)
        {
            if (transform.localPosition == _originalPosition)
            {
                enabled = false;
            }
        }
        else
        {
            if (transform.localPosition == GetPositionForDirection())
            {
                enabled = false;
            }
        }
        
    }
}
