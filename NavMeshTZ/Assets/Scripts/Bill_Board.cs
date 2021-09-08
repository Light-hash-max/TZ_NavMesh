using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bill_Board : MonoBehaviour
{
    private Transform cam;


    private void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }
    private void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
