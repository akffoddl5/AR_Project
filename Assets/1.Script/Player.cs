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

    }

    // Update is called once per frame
    void Update()
    {

		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);

			//첫번쨰 터치가 입력이 움직이는 중이라면
			if (touch.phase == TouchPhase.Moved)
			{
				Vector2 touchDelta = touch.deltaPosition;
                Vector2 screen_pos = touch.position;


                RaycastHit _hit;
                var a = Camera.main.ScreenPointToRay(screen_pos);
                var b = Physics.Raycast(a, out _hit, LayerMask.GetMask("Player"));

                if (b)
                {
                    if (touchDelta.x > 0)
                    {
                        _hit.transform.gameObject.GetComponent<Animator>().Play("Spin");
                    }
                    else
                    {
                        _hit.transform.gameObject.GetComponent<Animator>().Play("Spin_R");

                    }
                    

				}
                else
                {
                    //Debug.Log("못찾음");
                }

			}
		}
	}
}
