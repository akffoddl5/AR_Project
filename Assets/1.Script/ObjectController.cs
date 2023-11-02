using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//������Ʈ�� ���� ���� ��, ��ġ�ϸ鼭 �����δٸ� ��ü�� ȸ�� ��Ű�� �ʹ�.
public class ObjectController : MonoBehaviour
{
    [SerializeField] float rotSpeed = 0.1f;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            

            //ù���� ��ġ�� �Է��� �����̴� ���̶��
            if (touch.phase == TouchPhase.Moved)
            {
                Vector2 touchDelta = touch.deltaPosition;

           
                transform.Rotate(transform.up, touchDelta.x * -1f * rotSpeed);


            }
        }
    }
}
