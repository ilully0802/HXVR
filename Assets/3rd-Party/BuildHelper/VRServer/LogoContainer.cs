using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LogoContainer : MonoBehaviour {
    public float checkInterval = 30;
    public float checkTimeout = 180;
    private GameObject logo;
    private DateTime lastAliveTime;
    // Use this for initialization
    void Start() {
        logo = transform.Find("Logo").gameObject;
        PlayerPrefs.SetString("value", "0");
        InvokeRepeating("ShowLogo", 0, checkInterval);
    }

    // Update is called once per frame
    void Update() {

    }

    void ShowLogo()
    {
        long timeStick = long.Parse(PlayerPrefs.GetString("value", "0"));
        if (timeStick == 0)
        {
            DateTime dt = DateTime.Now;
            lastAliveTime = dt;
            PlayerPrefs.SetString("value", lastAliveTime.Ticks.ToString());
        }
        else
        {
            lastAliveTime = new DateTime(timeStick);
        }
        DateTime nowTime = DateTime.Now;
        TimeSpan keeptAliveTimeInterval = nowTime - lastAliveTime;
        if (keeptAliveTimeInterval.TotalSeconds > checkTimeout)
        {
            logo.SetActive(true);
        }
    }
}
