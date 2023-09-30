using UnityEngine;

public class OffScreenTweener : MonoBehaviour
{
    public bool autoplay = false;
    public bool animationIn = true;
    public Vector3 outDirection = Vector3.zero;

    private Vector3 _originalPosition;
    private Vector3 _originalWorldPosition;
    private Vector3 _directionWorlPosition;

    private RectTransform rectTransform;
    private bool animating = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        _originalPosition = rectTransform.anchoredPosition;
        _originalWorldPosition = rectTransform.position;
        _directionWorlPosition = WorldPositionForDirection();

        if (animationIn)
        {
            rectTransform.anchoredPosition = GetPositionForDirection();
        }
    }

    Vector3 GetPositionForDirection()
    {
        Vector3 position = _originalPosition;
        _originalPosition.x += outDirection.x;
        _originalPosition.y += outDirection.y;

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
            iTween.MoveTo(gameObject, iTween.Hash("position", _originalWorldPosition, "time", 1f, "easetype", iTween.EaseType.easeOutBack));
        }
        else
        {
            iTween.MoveTo(gameObject, iTween.Hash("position", _directionWorlPosition, "time", 1f, "easetype", iTween.EaseType.easeInBack));
        }
        animating = true;
    }

    Vector3 WorldPositionForDirection()
    {
        Vector3 tempPos = rectTransform.anchoredPosition;
        rectTransform.anchoredPosition = GetPositionForDirection();
        Vector3 tempPos2 = rectTransform.position;

        rectTransform.anchoredPosition = tempPos;
        return tempPos2;
    }

    // Update is called once per frame
    void Update()
    {       
        if (!animating) return;
        if (animationIn)
        {
            if (Vector3.Distance(transform.position, _originalWorldPosition) < 0.01f)
            {
                enabled = false;
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, _directionWorlPosition) < 0.01f)
            {
                enabled = false;
            }
        }
        
    }
}
