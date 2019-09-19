using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEditor;
using System.IO;

public class HXVR_Helper : Editor
{

    [MenuItem("HXVRHelper/Add PCVR objects")]
    static void AddPCVR()
    {
        var so = ScriptableObject.CreateInstance(typeof(HXVRPluginStub));
        var script = MonoScript.FromScriptableObject(so);
        var path = AssetDatabase.GetAssetPath(script);
        string prefabsPath = path.Substring(0, path.Length - "/HXVR_Helper/Editor/HXVRPluginStub.cs".Length) + "/HXVR_Helper/Prefabs/HXPCVR_Setup.prefab";
        GameObject pcVRPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(prefabsPath, typeof(GameObject));
        if (pcVRPrefab)
        {
            GameObject pcVR = Instantiate(pcVRPrefab) as GameObject;
            pcVR.name = pcVR.name.Replace("(Clone)", "");
            pcVR.transform.localPosition = Vector3.zero;
            pcVR.transform.localRotation = Quaternion.identity;
        }
    }

    [MenuItem("HXVRHelper/Add Pico VR objects")]
    static void AddPicoVR()
    {
        var so = ScriptableObject.CreateInstance(typeof(HXVRPluginStub));
        var script = MonoScript.FromScriptableObject(so);
        var path = AssetDatabase.GetAssetPath(script);
        string prefabsPath = path.Substring(0, path.Length - "/HXVR_Helper/Editor/HXVRPluginStub.cs".Length) + "/HXVR_Helper/Prefabs/HXPICOVR_Setup.prefab";
        GameObject picoVRPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(prefabsPath, typeof(GameObject));
        if (picoVRPrefab)
        {
            GameObject picoVR = Instantiate(picoVRPrefab) as GameObject;
            picoVR.name = picoVR.name.Replace("(Clone)", "");
            picoVR.transform.localPosition = Vector3.zero;
            picoVR.transform.localRotation = Quaternion.identity;
        }
    }


    [MenuItem("HXVRHelper/Add LeapMotion VR objects")]
    static void AddLeapmotinVR()
    {
        var so = ScriptableObject.CreateInstance(typeof(HXVRPluginStub));
        var script = MonoScript.FromScriptableObject(so);
        var path = AssetDatabase.GetAssetPath(script);
        string prefabsPath = path.Substring(0, path.Length - "/HXVR_Helper/Editor/HXVRPluginStub.cs".Length) + "/HXVR_Helper/Prefabs/HXLeapmotinVR_Setup.prefab";
        GameObject picoVRPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(prefabsPath, typeof(GameObject));
        if (picoVRPrefab)
        {
            GameObject picoVR = Instantiate(picoVRPrefab) as GameObject;
            picoVR.name = picoVR.name.Replace("(Clone)", "");
            picoVR.transform.localPosition = Vector3.zero;
            picoVR.transform.localRotation = Quaternion.identity;
        }
    }
}
