using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class PositionTeller : MonoBehaviour
{
    public Text log;
    public Text log2;

    int count = 0;

	public static double first_Lat; //최초 위도
	public static double first_Long; //최초 경도
	public static double current_Lat; //현재 위도
	public static double current_Long; //현재 경도


	private static bool gpsStarted = false;
	private static LocationInfo location;


	// Start is called before the first frame update
	void Start()
    {
		StartCoroutine(Start2());
    }

	private void OnTriggerEnter(Collider other)
	{
        Debug.Log("맞앗는데?");
        if (other.CompareTag("Enemy"))
        {
            count++;
            //log2.text = "맞았잖아" + count.ToString();
		}
	}

	// Update is called once per frame
	void Update()
    {
        log.text = transform.position.ToString() + " " + GetComponent<XROrigin>().transform.position.ToString() + " " + Camera.main.transform.position + " " ;
        
    }

	IEnumerator Start2()
	{
		// 유저가 GPS 사용중인지 최초 체크
		if (!Input.location.isEnabledByUser)
		{
			Debug.Log("GPS is not enabled");
			log2.text = "GPS is not enabled";
			yield break;
		}

		//GPS 서비스 시작
		Input.location.Start();
		Debug.Log("Awaiting initialization");

		//활성화될 때 까지 대기
		int maxWait = 20;
		while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
		{
			yield return new WaitForSeconds(1f);
			maxWait -= 1;
		}

		//20초 지날경우 활성화 중단
		if (maxWait < 1)
		{
			Debug.Log("Timed out");
			log2.text = "Timed out";
			yield break;
		}

		//연결 실패
		if (Input.location.status == LocationServiceStatus.Failed)
		{
			log2.text = "Unable to determine device location";
			Debug.Log("Unable to determine device location");
			yield break;
		}
		else
		{
			//접근 허가됨, 최초 위치 정보 받아오기
			location = Input.location.lastData;
			first_Lat = location.latitude * 1.0d;
			first_Long = location.longitude * 1.0d;
			gpsStarted = true;

			//현재 위치 갱신
			while (gpsStarted)
			{
				location = Input.location.lastData;
				current_Lat = location.latitude * 1.0d;
				current_Long = location.longitude * 1.0d;
				log2.text = current_Lat + " " + current_Long + "  << 여기 위도 경도";
				yield return new WaitForSeconds(1f);
			}
		}
	}

	//위치 서비스 종료
	public static void StopGPS()
	{
		if (Input.location.isEnabledByUser)
		{
			gpsStarted = false;
			Input.location.Stop();
		}
	}
}
