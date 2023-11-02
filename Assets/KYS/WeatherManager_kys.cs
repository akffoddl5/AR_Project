using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using UnityEngine;
using UnityEngine.UI;

public enum WEATHER
{
	SUNNY,
	RAINNY,
	SNOWY,
}




public class WeatherManager_kys : MonoBehaviour
{
	public static WeatherManager_kys instance;

	public static double first_Lat; //���� ����
	public static double first_Long; //���� �浵
	public static double current_Lat; //���� ����
	public static double current_Long; //���� �浵


	private static bool gpsStarted = false;
	private static LocationInfo location;
    public Text log;


	public WEATHER weather_type = WEATHER.SUNNY;
	public string temperature;
	public string wind_force;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(this);
		}
		else
		{
			Destroy(gameObject);
		}
	}


	void Start()
    {

		Debug.Log(DateTime.Now.ToString(("yyyy")) + " " + DateTime.Now.ToString(("MM")) + " " + DateTime.Now.ToString(("dd")));
		Debug.Log(DateTime.Now.ToString(("HH")) + " " + DateTime.Now.ToString(("mm")) + " " + DateTime.Now.ToString(("ss")));
		Debug.Log(DateTime.Now.ToString(("yyyyMMdd")));
		Debug.Log(DateTime.Now.ToString(("HHmm")));//

		StartCoroutine(IGPS_Detect());
		//GetWeather();
		//log.text = DateTime.Now.ToString(("yyyy")) + " " + DateTime.Now.ToString(("MM")) + " " + DateTime.Now.ToString(("dd"));
		//log.text += DateTime.Now.ToString(("HH")) + " " + DateTime.Now.ToString(("mm")) + " " + DateTime.Now.ToString(("ss"));

		//Debug.Log(DateTime.Now.ToString(("yyyy")) + " " + DateTime.Now.ToString(("MM")) + " " + DateTime.Now.ToString(("dd")));
		//Debug.Log(DateTime.Now.ToString(("HH")) + " " + DateTime.Now.ToString(("mm")) + " " + DateTime.Now.ToString(("ss")));
		
		HttpClient client = new HttpClient();
		string url = "http://apis.data.go.kr/1360000/VilageFcstInfoService_2.0/getUltraSrtNcst"; // URL
		url += "?ServiceKey=" + "0kf1KQ3urov%2FXPmHtfhp3hqbmo85Xl7oUlu3njLQF%2Bp%2BAmixPIRc4TadB7ixtDkMplrwmzpy1oKR6d6cxkfKSA%3D%3D"; // Service Key
		url += "&pageNo=1";
		url += "&numOfRows=1000";
		url += "&dataType=JSON";
		url += "&base_date=20231102";//
		url += "&base_time=0600";
		url += "&nx=61"  ;
		url += "&ny=126" ;

		var request = (HttpWebRequest)WebRequest.Create(url);
		request.Method = "GET";

		string results = string.Empty;
		HttpWebResponse response;
		using (response = request.GetResponse() as HttpWebResponse)
		{
			StreamReader reader = new StreamReader(response.GetResponseStream());
			results = reader.ReadToEnd();
		}

		//var a = JsonUtility.FromJson<JsonWeather>(results);

		Debug.Log(results);

		WeatherData weatherData = JsonUtility.FromJson<WeatherData>(results);

		foreach (var item in weatherData.response.body.items.item)
		{
			//Debug.Log("BaseDate: " + item.baseDate);
			//Debug.Log("BaseTime: " + item.baseTime);
			//Debug.Log("Category: " + item.category);
			//Debug.Log("Nx: " + item.nx);
			//Debug.Log("Ny: " + item.ny);
			//Debug.Log("ObsrValue: " + item.obsrValue);
			//Debug.Log();
		}






		/////



	}

    void Update()
    {
        
    }

    

    public IEnumerator GetWeather(double nx, double ny)
    {
		//log.text = DateTime.Now.ToString(("yyyy")) + " " + DateTime.Now.ToString(("MM")) + " " + DateTime.Now.ToString(("dd"));
		//log.text += DateTime.Now.ToString(("HH")) + " " + DateTime.Now.ToString(("mm")) + " " + DateTime.Now.ToString(("ss"));

		
        //log.text += nx + "  , " + ny;
		HttpClient client = new HttpClient();
        string url = "http://apis.data.go.kr/1360000/VilageFcstInfoService_2.0/getUltraSrtNcst"; // URL
        url += "?ServiceKey=" + "0kf1KQ3urov%2FXPmHtfhp3hqbmo85Xl7oUlu3njLQF%2Bp%2BAmixPIRc4TadB7ixtDkMplrwmzpy1oKR6d6cxkfKSA%3D%3D"; // Service Key
        url += "&pageNo=1";
        url += "&numOfRows=1000";
        url += "&dataType=JSON";
        url += "&base_date=" + DateTime.Now.ToString(("yyyyMMdd")) ;//
        url += "&base_time="/* +DateTime.Now.ToString(("HHmm"))*/ + "0000";
        url += "&nx=" + nx;
        url += "&ny=" + ny;

        var request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "GET";

        string results = string.Empty;
        HttpWebResponse response;
        using (response = request.GetResponse() as HttpWebResponse)
        {
            StreamReader reader = new StreamReader(response.GetResponseStream());
            results = reader.ReadToEnd();
        }

        //log.text += results;
		WeatherData weatherData = JsonUtility.FromJson<WeatherData>(results);

		foreach (var item in weatherData.response.body.items.item)
		{
			//��������
			if (item.category == "PTY")
			{
				if (item.obsrValue == "0")
				{
					weather_type = WEATHER.SUNNY;
				}
				else if (item.obsrValue == "1" || item.obsrValue == "5" || item.obsrValue == "6")
				{
					weather_type = WEATHER.RAINNY;
				} else if (item.obsrValue == "2" || item.obsrValue == "3" || item.obsrValue == "7") {
					
					weather_type = WEATHER.SNOWY;
				}
			}

			//��� ����
			if (item.category == "T1H")
			{
				temperature = item.obsrValue;
			}

			//ǳ�� ����
			if (item.category == "WSD")
			{
				wind_force = item.obsrValue;
			}
			//Debug.Log("BaseDate: " + item.baseDate);
			//Debug.Log("BaseTime: " + item.baseTime);
			//Debug.Log("Category: " + item.category);
			//Debug.Log("Nx: " + item.nx);
			//Debug.Log("Ny: " + item.ny);
			//Debug.Log("ObsrValue: " + item.obsrValue);
			//Debug.Log();

			log.text = "�µ� : " + temperature + " ǳ�� : " + wind_force + "  ���� : " + weather_type; 
		}

		yield return null;
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
				//log.text = " API �԰� ��ǥ : " + encoded_pos.x + " , " + encoded_pos.y ;
				//Debug.Log( " API �԰� ��ǥ : " + encoded_pos.x + " , " + encoded_pos.y);
                StartCoroutine(GetWeather(encoded_pos.x, encoded_pos.y));

				yield return new WaitForSeconds(10f);
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
[Serializable]
public class Item_data
{
	public string baseDate ;
	public string baseTime ;
	public string category ;
	public int nx;
	public int ny;
	public string obsrValue;
}

[Serializable]
public class Item
{
	public List<Item_data> item;
	
}
[Serializable]
public class Body
{
	public string dataType;
	public Item items;
	//public List<Item_data> items { get; set; }
	public int pageNo;
	public int numOfRows;
	public int totalCount;
}
[Serializable]
public class Header
{
	public string resultCode;
	public string resultMsg;
}

[Serializable]
public class Response
{
	public Header header;
	public Body body;
}

[Serializable]
public class WeatherData
{
	public Response response;
}

