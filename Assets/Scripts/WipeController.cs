using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WipeController : MonoBehaviour
{
    public bool startCenter;

    private void Start()
    {
        GetComponent<Animator>().SetBool(0, startCenter);
    }
}
