using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Eat : MonoBehaviour
{
    Animator anim;
    [SerializeField] Slider satietyScroll;
    [SerializeField] float maxsatiety = 100f;
    [SerializeField] float satiety = 0; //������
    int timeH;
    int timeM;
    int timeDay;
    int timeMonth;
    int timeYear;

    DateTime nowTime;
    DateTime nowTime2;

    [SerializeField] Image timePanel;
    [SerializeField] GameObject loveHit;

    public bool isEat = false;

    void Start()
    {
        if (!PlayerPrefs.HasKey("satiety"))             //��ü ������ ���� 
            PlayerPrefs.SetFloat("satiety", maxsatiety);
        else
            satiety = PlayerPrefs.GetFloat("satiety");

        satietyScroll.value = satiety / maxsatiety; // UI ��������� ������

        anim = transform.GetComponent<Animator>();

        if (PlayerPrefs.HasKey("timeH"))
        {
            //������ ���� �ð� 
            timeH = PlayerPrefs.GetInt("timeH", timeH);
            timeM = PlayerPrefs.GetInt("timeM", timeM);
            timeDay = PlayerPrefs.GetInt("timeDay", timeDay);
            timeMonth = PlayerPrefs.GetInt("timeMonth", timeMonth);
            timeYear = PlayerPrefs.GetInt("timeYear", timeYear);

            Debug.Log($"������ ���� �ð���{timeYear}�� {timeMonth}�� {timeDay}�� {timeH}�� {timeM}�� �Դϴ�");
            GetCurrentDate();
        }

    }

    //�ð����� ���ϱ�
    public void GetCurrentDate()
    {
        DateTime noewDataTime = DateTime.Now;
        DateTime LastwDataTime = System.Convert.ToDateTime($"{timeYear}/{timeMonth}/{timeDay} {timeH}:{timeM}");
        System.TimeSpan timeCal = noewDataTime - LastwDataTime;
        float Minus = 0;
        if (timeCal.Hours >= 1)
        {
            satiety -= 60f * (float)timeCal.Hours;
            Minus += 60f * (float)timeCal.Hours;
        }
        if (timeCal.Minutes >= 1f)
        {
            satiety -= 1f * (float)timeCal.Minutes / 1f;
            Minus += 1f * (float)timeCal.Minutes / 1f;
        }
        if (satiety < 0f)
        {
            satiety = 0f;
        }
        Debug.Log(timeCal);
        Debug.Log($"�پ�� �������� {Minus}�̴�");
    }
    public void EatEatEat()
    {
        loveHit.SetActive(false);
        isEat = true;
        anim.Play("Eat");
        satiety = maxsatiety;
        loveHit.SetActive(true);
    }

    public void IsEat_False()
    {
        isEat = false;
    }

    private void Update()
    {
        nowTime2 = DateTime.Now;

        if (nowTime2.Hour < 6)
        {
            timePanel.color = new Color(timePanel.color.r, timePanel.color.g, timePanel.color.b, 200f / 255f);
        }
        else if (nowTime2.Hour < 18)
        {
            timePanel.color = new Color(timePanel.color.r, timePanel.color.g, timePanel.color.b, 0);
        }
        else
        {
            timePanel.color = new Color(timePanel.color.r, timePanel.color.g, timePanel.color.b, 150f / 255f);
        }
    }

    private void FixedUpdate()
    {
        if (satiety == 0)
        {
            return;
        }
        if (satiety < 0)
        {
            satiety = 0;
            satietyScroll.value = satiety / maxsatiety;

            return;
        }
        satiety -= Time.deltaTime;
        satietyScroll.value = satiety / maxsatiety;
    }
    private void OnDestroy()
    {
        PlayerPrefs.SetFloat("satiety", satiety);
        SetTime();

    }

    private void SetTime()
    {
        nowTime = DateTime.Now;
        timeDay = nowTime.Day;
        timeM = nowTime.Minute;
        timeH = nowTime.Hour;
        timeMonth = nowTime.Month;
        timeYear = nowTime.Year;
        Debug.Log($"����ð�{timeYear}�� {timeMonth}�� {timeDay}�� {timeH}�� {timeM}�� �Դϴ�");
        PlayerPrefs.SetInt("timeH", timeH);
        PlayerPrefs.SetInt("timeM", timeM);
        PlayerPrefs.SetInt("timeDay", timeDay);
        PlayerPrefs.SetInt("timeMonth", timeMonth);
        PlayerPrefs.SetInt("timeYear", timeYear);


    }

}
