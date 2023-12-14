![image](https://github.com/akffoddl5/AR_Project/assets/44525847/979d67db-f83f-4691-8332-0561efd34dd8)


## **프로젝트 소개**

- **유니티 AR코어를 이용해 AR 다마고치 구현**
- **날씨 API로 해당 지역의 2시간전 날씨를 게임에 표현(맑음, 비, 눈)**
- **현재 시간에 따라 낮, 저녁, 밤 표현**
- **구글 플레이스토어에 출시하기 위해 여러가지 세팅(위치정보 허용, 카메라 허용)**
- **배고픔 게이지가 존재하며 일정시간 마다 줄어들고 배고프면 배고픔 표시**

## 시연영상

https://youtu.be/35OBUrr-rSg?si=Li7mNqUh0ivCY3LO

## **목차**

1. **AR 세팅**
    
    **1-1. 컴포넌트 세팅**
    
    **1-2. 프로젝트 세팅**
    
2. **날씨 API**
    
    **2-1. GPS 정보 취득**
    
    **2-2. 위치 정보 허용 패널 호출**
    
    **2-3. 날씨 정보 취득**
    
    **2-3-1. GPS 데이터 전처리**
    
    **2-3-2. API 호출**
    
3. **상호작용**
    
    **3-1. 쓰다듬기**
    
    **3-2. 밥 먹기**
    
4. **출시 세팅**
    
    **4-1. 앱 아이콘 세팅**
    
    **4-2. Splash 세팅**
    
    **4-3. 해상도 세팅**
    

### **1. AR세팅**

**1-1. 컴포넌트 세팅**

- XR 오리진 생성 및 컴포넌트 Add

![image](https://github.com/akffoddl5/AR_Project/assets/44525847/4adcb6e0-ef3f-4faf-aeb5-c302786143a1)


- AR세션 세팅

![image](https://github.com/akffoddl5/AR_Project/assets/44525847/6ddd2c6a-136a-4a2a-bb02-1957389664fa)


**1-2. 프로젝트 세팅**

![image](https://github.com/akffoddl5/AR_Project/assets/44525847/b5f5788d-8635-4388-a11d-468b4d6a7322)


### 2**. 날씨 API**

**2-1. GPS 정보 취득**

```csharp
IEnumerator IGPS_Detect()
 {
     while (true)
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
                 //log.text = " API 규격 좌표 : " + encoded_pos.x + " , " + encoded_pos.y ;
                 //Debug.Log( " API 규격 좌표 : " + encoded_pos.x + " , " + encoded_pos.y);
                 StartCoroutine(GetWeather(encoded_pos.x, encoded_pos.y));

                 yield return new WaitForSeconds(10f);
             }
         }

         yield return new WaitForSeconds(3f);
		}
	}
```

**2-2. 위치 정보 허용 패널 호출**

```csharp
if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
{
	Permission.RequestUserPermission(Permission.FineLocation);
}
```

**2-3. 날씨 정보 취득**

**2-3-1. GPS 데이터 전처리**

- GPS 위도 경도 데이터를 API규격으로 변환

```csharp
//위도경도를 API규격으로 변환
WgsToBaseStationCoord pos_encoder = new WgsToBaseStationCoord();
LatXLonY encoded_pos = pos_encoder.dfs_xy_conv(current_Lat, current_Long);

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
```

**2-3-2. API 호출**

```csharp
StartCoroutine(GetWeather(encoded_pos.x, encoded_pos.y));

public IEnumerator GetWeather(double nx, double ny)
{
    //log.text = DateTime.Now.ToString(("yyyy")) + " " + DateTime.Now.ToString(("MM")) + " " + DateTime.Now.ToString(("dd"));
    //log.text += DateTime.Now.ToString(("HH")) + " " + DateTime.Now.ToString(("mm")) + " " + DateTime.Now.ToString(("ss"));

    //현재 시간에서 한 시간 전
    DateTime dateTime = DateTime.Now.AddHours(-1);

    //log.text += nx + "  , " + ny;
    HttpClient client = new HttpClient();
    string url = "http://apis.data.go.kr/1360000/VilageFcstInfoService_2.0/getUltraSrtNcst"; // URL
    url += "?ServiceKey=" + "0kf1KQ3urov%2FXPmHtfhp3hqbmo85Xl7oUlu3njLQF%2Bp%2BAmixPIRc4TadB7ixtDkMplrwmzpy1oKR6d6cxkfKSA%3D%3D"; // Service Key
    url += "&pageNo=1";
    url += "&numOfRows=1000";
    url += "&dataType=JSON";
    url += "&base_date=" + dateTime.ToString(("yyyyMMdd"));//
    url += "&base_time="/* +DateTime.Now.ToString(("HHmm"))*/ + dateTime.ToString("D2") + "00";
    url += "&nx=" + nx;
    url += "&ny=" + ny;

	
	var request = (HttpWebRequest)WebRequest.Create(url);
    request.Method = "GET";
    request.Timeout = 1000;

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
        //날씨정보
        if (item.category == "PTY")
        {
            if (item.obsrValue == "0")
            {
                weather_type = WEATHER.SUNNY;
            }
            else if (item.obsrValue == "1" || item.obsrValue == "5" || item.obsrValue == "6")
            {
                weather_type = WEATHER.RAINNY;
            }
            else if (item.obsrValue == "2" || item.obsrValue == "3" || item.obsrValue == "7")
            {

                weather_type = WEATHER.SNOWY;
            }
        }

        //기온 정보
        if (item.category == "T1H")
        {
            temperature = item.obsrValue;
        }

        //풍속 정보
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

        log.text = "온도 : " + temperature + " 풍속 : " + wind_force + "  날씨 : " + weather_type;
    }

    yield return null;
}
```

### 3**. 상호작용**

**3-1. 쓰다듬기**

- 터치 입력의 델타x값으로 캐릭터의 회전 방향을 정해 애니메이션 플레이

```csharp
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
```

**3-2. 밥 주기**

- 밥 그릇을 터치하여 밥을 먹으면 배고픔 게이지 완화
- 밥 먹을 시 애니메이션 재생

### 4**. 출시 세팅**

**4-1. 앱 아이콘 세팅**
![image](https://github.com/akffoddl5/AR_Project/assets/44525847/598cd8c5-c0af-43d1-873c-1dda9a890cf9)


**4-2. Splash 세팅**

![image](https://github.com/akffoddl5/AR_Project/assets/44525847/88eccb37-9c46-4f43-bf0d-b35473da55e5)
