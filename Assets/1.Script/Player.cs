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
        ////Screen.SetResolution(720, 1280, false);
        //cameraOffset.transform.position = transform.localPosition;
        ////cameraOffset.transform.rotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
        //transform.localPosition = Vector3.zero;
        ////transform.localRotation = Quaternion.Euler(0, 0, 0);
        //posText.text = cameraOffset.transform.position + " " + transform.localPosition + "\n오징어 : " + enemy.transform.position;


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
                    Debug.Log("Player 찾음 !!!");
                    _hit.transform.gameObject.GetComponent<Animator>().Play("Spin");

				}
                else
                {
                    Debug.Log("못찾음");
                }

			}
		}
	}
}
