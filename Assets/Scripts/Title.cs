using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Title : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
      Physics.gravity = new Vector3(0, 9.81f, 0);  
    }

    private void Update() {
      if(Input.GetMouseButtonDown(0)) { 
        Cursor.lockState = CursorLockMode.None;
        Cursor.lockState = CursorLockMode.Confined;
      }
    }
}
