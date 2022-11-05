using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DayNightTime : MonoBehaviour
{

    // Variable
    public Text TimeHud;
    public int hourTime = 12; 
    public int minTime = 23;
    public int day = 0;
    public int hourSurvive = 0;
    public bool night = false;
    private bool AddHour = false;
    private float WaitTime = 1f;//Seconds
    private int minutesTime = 0;


    
    // Start is called before the first frame update
    void Start()
    {
        

        
    }

    // Update is called once per frame
    IEnumerator TimeCal()
    {
        AddHour = true;
        while (minTime > 0){
            yield return new WaitForSeconds(WaitTime);
            minTime -= 1;    
            //Debug.Log(minTime);
            if (minTime % 4 == 0)
            {
                minutesTime += 10;
            }
        }
        if (minTime <= 0){
            minutesTime = 0;
            minTime = 25;
            hourTime += 1;
            hourSurvive += 1;
            AddHour = false;
        }
        
    
    }
    void Update()
    {
        if(AddHour == false)
        {
            StartCoroutine(TimeCal());
        }
        if(minutesTime == 0){
            TimeHud.text = "Day: " + day + " Time: " + hourTime + ":" + minutesTime + "0";
        } else {
            TimeHud.text = "Day: " + day + " Time: " + hourTime + ":" + minutesTime;
        }
        if (hourTime > 23){
            hourTime = 0;
            day += 1;
        }
        else if (hourTime == 18){
            night = true;
        }
        else if (hourTime == 6){
            night = false;
        }
        
    }
}
