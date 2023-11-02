using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//오브젝트가 켜져 있을 때, 터치하면서 움직인다면 물체를 회전 시키고 싶다.
public class ObjectController : MonoBehaviour
{
    [SerializeField] float rotSpeed = 0.1f;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            

            //첫번쨰 터치가 입력이 움직이는 중이라면
            if (touch.phase == TouchPhase.Moved)
            {
                Vector2 touchDelta = touch.deltaPosition;

           
                transform.Rotate(transform.up, touchDelta.x * -1f * rotSpeed);


            }
        }
    }
}
