using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject cameraOffset;
    [SerializeField] TextMeshProUGUI posText;

    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(720, 1280, false);
    }

    // Update is called once per frame
    void Update()
    {
        Screen.SetResolution(720, 1280, false);
        cameraOffset.transform.position = transform.localPosition;
        transform.position = Vector3.zero;
        posText.text = cameraOffset.transform.position + " " + transform.position;
    }
}
