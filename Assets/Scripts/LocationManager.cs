using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Scripting;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


//indicator 관리
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

    //스크린 텇치로 Building 생성 또는 이동
	private void TouchGround()
	{
        //바닥이 확인된 상태에서
        if (indicator.activeInHierarchy)
        {
            //터치가 됐을떄
            if (Input.touchCount > 0)
            {
                //첫번째 터치를 가져온다.
                Touch touch = Input.GetTouch(0);


                //터치 시작시
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

	//스크린 중앙지점에서 레이발사하여 바닥 확인
	public void DetectGround()
    {
		indicator.SetActive(true);
		Vector2 screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

        List<ARRaycastHit> hitInfo = new List<ARRaycastHit>();

        //레이를 쏴서 추적 대상이 바닥이라면
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
