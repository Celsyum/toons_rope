using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[System.Serializable]
public class ClickEvent : UnityEvent<DiamondController>
{
}

public class DiamondController : MonoBehaviour, IPointerClickHandler
{
    public SpriteRenderer background;
    public ClickEvent onPressed;
    public Texture2D texturePath;
    public TextMeshPro text;
    public GameObject label;
    [HideInInspector]
    public int _queue = 0;

    bool _make_centered = false;
    public bool make_centered
    {
        get
        {
            return _make_centered;
        }
        set
        {
            _make_centered = value;
            label.GetComponentInParent<LookAtCenter>().centered = _make_centered;
        }
    }

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
        background.GetComponent<BoxCollider>().enabled = false;
        StartCoroutine(DelayedColliderDisable());
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onPressed.Invoke(this);
    }

    IEnumerator DelayedColliderDisable()
    {
        yield return new WaitForSeconds(0.02f);
        
    }
}
