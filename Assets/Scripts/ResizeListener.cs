using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeListener : MonoBehaviour
{
    //public event callback
    [SerializeField]
    private UnityEngine.Events.UnityEvent onResize;

    // Start is called before the first frame update
    void OnRectTransformDimensionsChange()
    {
        onResize.Invoke();
    }
}
