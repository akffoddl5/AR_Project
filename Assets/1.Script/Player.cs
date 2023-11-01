using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject cameraOffset;
    [SerializeField] TextMeshProUGUI posText;
    [SerializeField] GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {
        //Screen.SetResolution(720, 1280, false);
    }

    // Update is called once per frame
    void Update()
    {
        //Screen.SetResolution(720, 1280, false);
        cameraOffset.transform.position = transform.localPosition;
        //cameraOffset.transform.rotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
        transform.localPosition = Vector3.zero;
        //transform.localRotation = Quaternion.Euler(0, 0, 0);
        posText.text = cameraOffset.transform.position + " " + transform.localPosition + "\n¿ÀÂ¡¾î : " + enemy.transform.position;
    }
}
