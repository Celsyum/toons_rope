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

    List<DiamondController> controllerList = new List<DiamondController>();
    List<Rope> ropeList = new List<Rope>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Time.deltaTime;     
        
    }

    //if controllerList.Count > 0 then add rope, position at first controller position and animate its height and direction to the second controller position
    // once animation is done, remove the first controller from the list and continue with the next one, leave previous rope in place

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

    public void AddController(DiamondController diamond)
    {
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
