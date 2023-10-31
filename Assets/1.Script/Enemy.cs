using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{

    void Start()
    {
        
    }

    void Update()
    {
        transform.Translate(-transform.forward  * Time.deltaTime);
    }
}
