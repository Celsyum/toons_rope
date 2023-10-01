using System.Collections.Generic;
using UnityEngine;

public class RopeManager : MonoBehaviour
{
    [HideInInspector]
    public UnityEngine.Events.UnityEvent onAllRopeAnimationsDone;

    public float ropeAnimationSpeed = 1f;

    public GameObject ropePrefab;

    [HideInInspector]
    public DiamondController firstDiamond;

    [HideInInspector]
    public bool lastDiamond = false;

    public UnityEngine.UI.Image background;

    List<DiamondController> controllerList = new List<DiamondController>();
    List<Rope> ropeList = new List<Rope>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void AddRope()
    {
        GameObject rope = Instantiate(ropePrefab, transform);
        DiamondController diamond;
        rope.transform.position = controllerList[0].transform.position;
        Rope rope1 = rope.GetComponent<Rope>();
        rope1.onRopeAnimationDone.AddListener(this.OnRopeAnimationDone);
        if (controllerList.Count == 1) diamond = firstDiamond;
        else diamond = controllerList[1];
        rope1.Animate(controllerList[0].transform.position, diamond.transform.position);
        ropeList.Add(rope1);

        

    }

    void AnimateRipple()
    {
        background.material.SetFloat("_Force", 1f);
        Vector2 touchPos = Input.mousePosition;
        touchPos.x = touchPos.x / Screen.width;
        touchPos.y = 1f - touchPos.y / Screen.height;
        iTween.Stop(this.gameObject);
        background.material.SetVector("_TouchPos", touchPos);
        iTween.ValueTo(this.gameObject, iTween.Hash("from", 1f, "to", 0.85f, "time", 0.2f, "onupdate", "OnBackgroundTweenUpdate", "easetype", iTween.EaseType.easeOutCubic));
        iTween.ValueTo(this.gameObject, iTween.Hash("from", 0.85f, "to", 1f, "time", 0.5f, "delay", 0.3f, "onupdate", "OnBackgroundTweenUpdate", "oncomplete", "OnBackgroundTweenEnded", "easetype", iTween.EaseType.easeInSine));
    }

    void OnBackgroundTweenUpdate(float value)
    {
        background.material.SetFloat("_Force", value);
    }

    void OnBackgroundTweenEnded()
    {
        background.material.SetFloat("_Force", 1f);
    }

    public void AddController(DiamondController diamond)
    {
        AnimateRipple();
        controllerList.Add(diamond);
        if (controllerList.Count == 2)
        {
            AddRope();
        }
    }

    public void OnRopeAnimationDone()
    {
        controllerList.RemoveAt(0);
        if (controllerList.Count > 1)
        {
            AddRope();
        } else if (controllerList.Count == 1 && lastDiamond)
        {
            AddRope();
        }
        else if (controllerList.Count == 0)
        {
            onAllRopeAnimationsDone.Invoke();
        }
    }

}
