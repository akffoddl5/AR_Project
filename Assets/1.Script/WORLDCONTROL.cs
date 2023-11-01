using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WORLDCONTROL : MonoBehaviour
{
    public GameObject world;
    public Text log1;
    public Text log2;
    public GameObject obj1;
    public GameObject obj2;

    void Start()
    {
        
    }

	void Update()
	{
		log1.text = world.transform.position + "  1,   " + transform.localPosition + "  2 , " + obj1.transform.position + " 3  , ";
		log2.text = obj2.transform.position + " 5 , " + transform.position + " 6 ,  " + transform.localPosition;
		if (transform.localPosition != Vector3.zero)
		{
			world.transform.position -= transform.localPosition;
			transform.localPosition = Vector3.zero;
			transform.parent.localPosition = Vector3.zero;
			transform.parent.parent.position = Vector3.zero;
			transform.parent.localPosition = Vector3.zero;
			transform.localPosition = Vector3.zero;
		}


	}

}
