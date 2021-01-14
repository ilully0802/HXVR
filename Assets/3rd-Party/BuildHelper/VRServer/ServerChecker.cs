using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;
using UnityEngine.UI;
using MyEntryption;
using UnityHTTP;

public class ServerChecker : MonoBehaviour
{

    // Use this for initialization
    //private static extern int add(int x, int y);

    public float checkInterval = 10;
    public float checkTimeout = 30;
    //private string serverURL = "https://www.huaxingzhiqu.com/api/";
    private string serverURL = "https://api.huaxingzhiqu.com/vr_client/";  
    private string commandLogin = "login";
    private string commandKeepAlive = "keepAlive";
    private string commandKeepLogout = "logout";
    private char[] keyChar = { 'O', 'h', 'H', 'Y', 'o', 'g', '9', 'h', 'V', 'U', '3', 'W', '9', 'T', 'c', 'q', 'k', 'r', 'N', '0', 'c', 'L', 'E', 'w', 'H', 'N', '9', '9', 'y', 'E', 'r', 'Q', '9', 'e', 'k', 'M', 'Z', 'H', 'y', 'L', 'h', 's', 'd', 'Y', 'a', 'Q', 'Y', '9', 'O', 'N', 'o', 'y', 'H', 'N', 'A', 'z', 'V', 'H', 'E', 'p', 'M', 'z', 'k', 'J' };
    private string token = null;
    private string appID;
    private string key;
    private DateTime lastAliveTime;
    private GameObject logoObject = null;

    void Awake()
    {
        //DontDestroyOnLoad(transform.gameObject);
    }

    void Start()
    {
        string CommandLine = Environment.CommandLine;
        string[] CommandLineArgs = Environment.GetCommandLineArgs();
        if (CommandLineArgs.Length != 3)
        {
            //Debug.Log("Command line error, please check! ");
            Application.Quit();
            return;
        }
        else
        {
            token = CommandLineArgs[1];
            appID = CommandLineArgs[2];
        }
        //Debug.Log("token: " + token);
        //Debug.Log("appID： " + appID);

        //token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOjE0LCJrZXkiOiI0OGY2Yzg3ZC0xYmFjLTRkNzAtODMzZS03YjViMzAwMGMxZTkiLCJpYXQiOjE1MTA3MjAwNTB9.pAEGfJ02H6RLkpbSB6zDfxlUFfzgPJX9vDUF730rI5A";
        //appID = "16";
        key = new string(keyChar);
        lastAliveTime = System.DateTime.Now;
        logoObject = gameObject.transform.Find("Logos").gameObject;
        InvokeRepeating("KeepAliveWithServer", 0, checkInterval);
    }

    void Start0()
    {
        GetTestParameters();
        //string CommandLine = Environment.CommandLine;
        //string[] CommandLineArgs = Environment.GetCommandLineArgs();
        //if (CommandLineArgs.Length != 3)
        //{
        //    //Debug.Log("Command line error, please check! ");
        //    Application.Quit();
        //    return;
        //}
        //else
        //{
        //    token = CommandLineArgs[1];
        //    appID = CommandLineArgs[2];
        //}
        ////Debug.Log("token: " + token);
        ////Debug.Log("appID： " + appID);

        token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOjE0LCJrZXkiOiIzZTlmYWJjYS04MjdmLTRhZjgtYTliYy00MDIxNmZkODQxN2MiLCJpYXQiOjE1MTA3OTg1MzR9.v7q_yxibnA4spkRIjOX5HzAN2DCjBYN1VdmbV-5O_z4";
        appID = "16";
        key = new string(keyChar);
        lastAliveTime = System.DateTime.Now;
        logoObject = gameObject.transform.Find("Logos").gameObject;
        InvokeRepeating("KeepAliveWithServer", 0, checkInterval);
    }

    private void GetTestParameters()
    {
        string user = "qqqqqqqqqqqqqq";
        string password = "qqqqqqqqqqqqqqq";
        string key = new string(keyChar);
        DateTime nowTime = DateTime.Now;
        string temTimeStr = nowTime.ToString("yyyy-MM-ddTHH:mm:sszzz");
        Debug.Log("Time: " + temTimeStr);
        string hash = MixString(user + password + temTimeStr  + key);
        Debug.Log("hash: " + hash);
    }

    string MixString(string source)
    {
        string temString = RealString.SecSum(source);
        return (temString.Substring(16, 16) + temString.Substring(0, 16)).ToLower();
    }

    void KeepAliveWithServer()
    {
        DateTime nowTime = DateTime.Now;
        TimeSpan keeptAliveTimeInterval = nowTime - lastAliveTime;
        //Debug.Log("Time : keeptAliveTimeInterval -- " + keeptAliveTimeInterval.TotalSeconds.ToString() + "checkTimeout -- " + checkTimeout.ToString());
        if (keeptAliveTimeInterval.TotalSeconds > checkTimeout)
        {
            //Debug.LogError("Keep alive timeout.");
            Application.Quit();
            return;
        }

        string temTimeStr = nowTime.ToString("yyyy-MM-ddTHH:mm:sszzz");
        Hashtable data = new Hashtable();
        data.Add("token", token);
        data.Add("appId", appID);
        data.Add("timestamp", temTimeStr);

        //Debug.Log("token: " + token);
        //Debug.Log("temTimeStr: " + temTimeStr);
        string hash = MixString(token + temTimeStr + appID + key);
        data.Add("hash", hash);

        //Debug.Log("hash: " + hash);

        //Debug.Log("token: " + token);
        //Debug.Log("appId: " + appID);
        //Debug.Log("timestamp: " + temTimeStr);
        //Debug.Log("hash: " + hash);

        // When you pass a Hashtable as the third argument, we assume you want it send as JSON-encoded
        // data.  We'll encode it to JSON for you and set the Content-Type header to application/json
        string targetURL = serverURL + commandKeepAlive;
        Request theRequest = new Request("post", targetURL, data);
        theRequest.Send((request) => {
            //Debug.Log("request.response: " + request.response.Text);
            if (request.response == null)
            {
                //Application.Quit();
                return;
            }
            bool ret = false;
            Hashtable result = (Hashtable)JSON.JsonDecode(request.response.Text, ref ret);
            if ((!ret) || (result == null))
            {
                return;
            }

            //foreach (var val in result.Values)
            //{
            //    Debug.Log("val" + val);
            //}
            if (!result.ContainsKey("code"))
            {
                return;
            }

            if (result["code"].ToString() != "0")
            {
                Application.Quit();
                return;
            }
            if (!result.ContainsKey("data"))
            {
                return;
            }
            Hashtable dataDic = result["data"] as Hashtable;
            Debug.Log("dataDic: " + dataDic["showLogo"].ToString());
            if ((dataDic != null) && dataDic.ContainsKey("showLogo") && dataDic["showLogo"].ToString() != "False")
            {
                logoObject.SetActive(true);
            }
            else
            {
                logoObject.SetActive(false);
            }
            lastAliveTime = DateTime.Now;
            PlayerPrefs.SetString("value", lastAliveTime.Ticks.ToString());

        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
