using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Scripting;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


//indicator ����
public class LocationManager : MonoBehaviour
{

    [SerializeField] GameObject indicator;
    [SerializeField] GameObject buildingPrefab;
    GameObject building;

    ARRaycastManager raycastManager;
    
    void Start()
    {
        indicator.SetActive(false);
        raycastManager = GetComponent<ARRaycastManager>();

        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        
    }

    void Update()
    {
        DetectGround();

        TouchGround();
    }

    //��ũ�� ��ġ�� Building ���� �Ǵ� �̵�
	private void TouchGround()
	{
        //�ٴ��� Ȯ�ε� ���¿���
        if (indicator.activeInHierarchy)
        {
            //��ġ�� ������
            if (Input.touchCount > 0)
            {
                //ù��° ��ġ�� �����´�.
                Touch touch = Input.GetTouch(0);


                //��ġ ���۽�
                if (touch.phase == TouchPhase.Began)
                {
                    if (building == null)
                    {
                        building = Instantiate(buildingPrefab, indicator.transform.position, indicator.transform.rotation);
                    }
                    else
                    {
                        building.transform.position = indicator.transform.position;
                        building.transform.rotation = indicator.transform.rotation;


					}
                }
            }
        }
	}

	//��ũ�� �߾��������� ���̹߻��Ͽ� �ٴ� Ȯ��
	public void DetectGround()
    {
		indicator.SetActive(true);
		Vector2 screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

        List<ARRaycastHit> hitInfo = new List<ARRaycastHit>();

        //���̸� ���� ���� ����� �ٴ��̶��
        if (raycastManager.Raycast(screenCenter, hitInfo, TrackableType.Planes))
        {
            indicator.SetActive(true);
            indicator.transform.position = hitInfo[0].pose.position;
            indicator.transform.rotation = hitInfo[0].pose.rotation;
        }
        else
        {
            indicator.SetActive(false);
        }
    }
}
