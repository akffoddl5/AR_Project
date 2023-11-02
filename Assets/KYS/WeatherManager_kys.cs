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
	public static double first_Lat; //최초 위도
	public static double first_Long; //최초 경도
	public static double current_Lat; //현재 위도
	public static double current_Long; //현재 경도


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
		// 유저가 GPS 사용중인지 최초 체크//
		if (!Input.location.isEnabledByUser)
		{
            log.text = "GPS is not enabled";
			Debug.Log("GPS is not enabled");
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
			yield break;
		}

		//연결 실패
		if (Input.location.status == LocationServiceStatus.Failed)
		{
			log.text = "Unable to determine device location";
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

                //위도경도를 API규격으로 변환
				WgsToBaseStationCoord pos_encoder = new WgsToBaseStationCoord();
				LatXLonY encoded_pos = pos_encoder.dfs_xy_conv(current_Lat, current_Long);
				log.text = " API 규격 좌표 : " + encoded_pos.x + " , " + encoded_pos.y ;
				Debug.Log( " API 규격 좌표 : " + encoded_pos.x + " , " + encoded_pos.y);
                GetWeather(encoded_pos.x, encoded_pos.y);

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


public struct lamc_parameter
{
    public double Re;          /* 사용할 지구반경 [ km ]      */
    public double grid;        /* 격자간격        [ km ]      */
    public double slat1;       /* 표준위도        [degree]    */
    public double slat2;       /* 표준위도        [degree]    */
    public double olon;        /* 기준점의 경도   [degree]    */
    public double olat;        /* 기준점의 위도   [degree]    */
    public double xo;          /* 기준점의 X좌표  [격자거리]  */
    public double yo;          /* 기준점의 Y좌표  [격자거리]  */
};

public class WgsToBaseStationCoord
{
    lamc_parameter map;

    public WgsToBaseStationCoord()
    {
        map.Re = 6371.00877;         // 지도반경
        map.grid = 5.0;              // 격자간격 (km)
        map.slat1 = 30.0;            // 표준위도 1
        map.slat2 = 60.0;            // 표준위도 2
        map.olon = 126.0;            // 기준점 경도
        map.olat = 38.0;             // 기준점 위도
        map.xo = 43;                 // 기준점 X좌표
        map.yo = 136;                // 기준점 Y좌표
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