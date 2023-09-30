using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * sets a random start time for the animation from the animator
 * */
public class RandomStartTime : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        Animator animator = GetComponent<Animator>();
        animator.Play("Idle", -1, Random.Range(0f, 1f));            
    }

}
