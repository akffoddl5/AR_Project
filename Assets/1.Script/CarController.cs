using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
    bool hold;



    public void GoFoward()
    {
        transform.localPosition += transform.forward;
	}

    public void GoBackward()
    {
        transform.localPosition -= transform.forward;
    }

    public void TurnRight()
    {
        transform.Rotate(0, 20, 0);
        transform.localPosition += transform.forward;
    }

    public void TurnLeft()
    {
		transform.Rotate(0, -20, 0);
		transform.localPosition += transform.forward;
	}

    


}
