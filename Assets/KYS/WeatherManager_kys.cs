using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using UnityEngine;
using UnityEngine.UI;

public class WeatherManager_kys : MonoBehaviour
{
	public static double first_Lat; //���� ����
	public static double first_Long; //���� �浵
	public static double current_Lat; //���� ����
	public static double current_Long; //���� �浵


	private static bool gpsStarted = false;
	private static LocationInfo location;

    

    public Text log;


	void Start()
    {
        StartCoroutine(IGPS_Detect());
        //GetWeather();
	}

    void Update()
    {
        
    }


    public void GetWeather(double nx, double ny)
    {
		HttpClient client = new HttpClient();
        string url = "http://apis.data.go.kr/1360000/VilageFcstInfoService_2.0/getUltraSrtNcst"; // URL
        url += "?ServiceKey=" + "0kf1KQ3urov%2FXPmHtfhp3hqbmo85Xl7oUlu3njLQF%2Bp%2BAmixPIRc4TadB7ixtDkMplrwmzpy1oKR6d6cxkfKSA%3D%3D"; // Service Key
        url += "&pageNo=1";
        url += "&numOfRows=1000";
        url += "&dataType=JSON";
        url += "&base_date=20231101";//
        url += "&base_time=1200";
        url += "&nx=55";
        url += "&ny=127";

        var request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "GET";

        string results = string.Empty;
        HttpWebResponse response;
        using (response = request.GetResponse() as HttpWebResponse)
        {
            StreamReader reader = new StreamReader(response.GetResponseStream());
            results = reader.ReadToEnd();
        }

        
        Debug.Log(results);
    }

	IEnumerator IGPS_Detect()
	{
		// ������ GPS ��������� ���� üũ//
		if (!Input.location.isEnabledByUser)
		{
            log.text = "GPS is not enabled";
			Debug.Log("GPS is not enabled");
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
			yield break;
		}

		//���� ����
		if (Input.location.status == LocationServiceStatus.Failed)
		{
			log.text = "Unable to determine device location";
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

                //�����浵�� API�԰����� ��ȯ
				WgsToBaseStationCoord pos_encoder = new WgsToBaseStationCoord();
				LatXLonY encoded_pos = pos_encoder.dfs_xy_conv(current_Lat, current_Long);
				log.text = " API �԰� ��ǥ : " + encoded_pos.x + " , " + encoded_pos.y ;
				Debug.Log( " API �԰� ��ǥ : " + encoded_pos.x + " , " + encoded_pos.y);
                GetWeather(encoded_pos.x, encoded_pos.y);

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


public struct lamc_parameter
{
    public double Re;          /* ����� �����ݰ� [ km ]      */
    public double grid;        /* ���ڰ���        [ km ]      */
    public double slat1;       /* ǥ������        [degree]    */
    public double slat2;       /* ǥ������        [degree]    */
    public double olon;        /* �������� �浵   [degree]    */
    public double olat;        /* �������� ����   [degree]    */
    public double xo;          /* �������� X��ǥ  [���ڰŸ�]  */
    public double yo;          /* �������� Y��ǥ  [���ڰŸ�]  */
};

public class WgsToBaseStationCoord
{
    lamc_parameter map;

    public WgsToBaseStationCoord()
    {
        map.Re = 6371.00877;         // �����ݰ�
        map.grid = 5.0;              // ���ڰ��� (km)
        map.slat1 = 30.0;            // ǥ������ 1
        map.slat2 = 60.0;            // ǥ������ 2
        map.olon = 126.0;            // ������ �浵
        map.olat = 38.0;             // ������ ����
        map.xo = 43;                 // ������ X��ǥ
        map.yo = 136;                // ������ Y��ǥ
    }

    public LatXLonY dfs_xy_conv(double _dLat, double _dLon)
    {
        double DEGARD = Math.PI / 180.0;
        //double RADDEG = 180.0 / Math.PI;

        double re = map.Re / map.grid;
        double slat1 = map.slat1 * DEGARD;
        double slat2 = map.slat2 * DEGARD;
        double olon = map.olon * DEGARD;
        double olat = map.olat * DEGARD;

        double sn = Math.Tan(Math.PI * 0.25 + slat2 * 0.5) / Math.Tan(Math.PI * 0.25 + slat1 * 0.5);
        sn = Math.Log(Math.Cos(slat1) / Math.Cos(slat2)) / Math.Log(sn);
        double sf = Math.Tan(Math.PI * 0.25 + slat1 * 0.5);
        sf = Math.Pow(sf, sn) * Math.Cos(slat1) / sn;
        double ro = Math.Tan(Math.PI * 0.25 + olat * 0.5);
        ro = re * sf / Math.Pow(ro, sn);

        LatXLonY rs = new LatXLonY();
        rs.lat = _dLat;
        rs.lon = _dLon;

        double ra = Math.Tan(Math.PI * 0.25 + _dLat * DEGARD * 0.5);
        ra = re * sf / Math.Pow(ra, sn);
        double theta = _dLon * DEGARD - olon;
        if (theta > Math.PI) theta -= 2.0 * Math.PI;
        if (theta < -Math.PI) theta += 2.0 * Math.PI;
        theta *= sn;
        rs.x = Math.Floor(ra * Math.Sin(theta) + map.xo + 0.5);
        rs.y = Math.Floor(ro - ra * Math.Cos(theta) + map.yo + 0.5);

        return rs;
    }
}

public class LatXLonY
{
    public double lat;
    public double lon;

    public double x;
    public double y;
}