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

	public static double first_Lat; //���� ����
	public static double first_Long; //���� �浵
	public static double current_Lat; //���� ����
	public static double current_Long; //���� �浵


	private static bool gpsStarted = false;
	private static LocationInfo location;


	// Start is called before the first frame update
	void Start()
    {
		StartCoroutine(Start2());
    }

	private void OnTriggerEnter(Collider other)
	{
        Debug.Log("�¾Ѵµ�?");
        if (other.CompareTag("Enemy"))
        {
            count++;
            //log2.text = "�¾��ݾ�" + count.ToString();
		}
	}

	// Update is called once per frame
	void Update()
    {
        log.text = transform.position.ToString() + " " + GetComponent<XROrigin>().transform.position.ToString() + " " + Camera.main.transform.position + " " ;
        
    }

	IEnumerator Start2()
	{
		// ������ GPS ��������� ���� üũ
		if (!Input.location.isEnabledByUser)
		{
			Debug.Log("GPS is not enabled");
			log2.text = "GPS is not enabled";
			yield break;
		}

		//GPS ���� ����
		Input.location.Start();
		Debug.Log("Awaiting initialization");

		//Ȱ��ȭ�� �� ���� ���
		int maxWait = 20;
		while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
		{
			yield return new WaitForSeconds(1f);
			maxWait -= 1;
		}

		//20�� ������� Ȱ��ȭ �ߴ�
		if (maxWait < 1)
		{
			Debug.Log("Timed out");
			log2.text = "Timed out";
			yield break;
		}

		//���� ����
		if (Input.location.status == LocationServiceStatus.Failed)
		{
			log2.text = "Unable to determine device location";
			Debug.Log("Unable to determine device location");
			yield break;
		}
		else
		{
			//���� �㰡��, ���� ��ġ ���� �޾ƿ���
			location = Input.location.lastData;
			first_Lat = location.latitude * 1.0d;
			first_Long = location.longitude * 1.0d;
			gpsStarted = true;

			//���� ��ġ ����
			while (gpsStarted)
			{
				location = Input.location.lastData;
				current_Lat = location.latitude * 1.0d;
				current_Long = location.longitude * 1.0d;
				log2.text = current_Lat + " " + current_Long + "  << ���� ���� �浵";
				yield return new WaitForSeconds(1f);
			}
		}
	}

	//��ġ ���� ����
	public static void StopGPS()
	{
		if (Input.location.isEnabledByUser)
		{
			gpsStarted = false;
			Input.location.Stop();
		}
	}
}
