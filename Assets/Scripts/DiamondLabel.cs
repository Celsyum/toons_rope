using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DiamondLabel : MonoBehaviour
{
    public void OnLabelTweenEnded()
    {
        gameObject.SetActive(false);
    }

    public void OnLabelTweenUpdate(float value)
    {
        gameObject.GetComponent<TextMeshPro>().alpha = value;
    }
}
