using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEditor;
using System.IO;
using Xxtea;
using MyEntryption;


public class BuildHelper : Editor
{
    [MenuItem("BuildHelper/Add logo object")]
    static void AddLogo()
    {
        GameObject mainCamera = GameObject.Find("Camera (eye)");
        if (mainCamera)
        {
            foreach (Transform oneChild in mainCamera.transform)
            {
                if (oneChild.name.Contains("LogoContainer"))
                {
                    GameObject.DestroyImmediate(oneChild.gameObject);
                } 
            }
            GameObject logoContainerPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/BuildHelper/Editor/Material/LogoContainer.prefab", typeof(GameObject));
            if (logoContainerPrefab)
            {
                GameObject logoContainer = Instantiate(logoContainerPrefab) as GameObject;
                logoContainer.transform.parent = mainCamera.transform;
                logoContainer.transform.localPosition = Vector3.zero;
                logoContainer.transform.localRotation = Quaternion.identity;
            }
        }
    }

    [MenuItem("BuildHelper/Add check logo object")]
    static void AddCheckLogo()
    {
        GameObject mainCamera = GameObject.Find("Camera (eye)");
        if (mainCamera)
        {
            foreach (Transform oneChild in mainCamera.transform)
            {
                if (oneChild.name.Contains("LogoContainer")) GameObject.DestroyImmediate(oneChild.gameObject);
            }
            GameObject logoContainerPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/3rd-Party/BuildHelper/Editor/Material/LogoContainerCheck.prefab", typeof(GameObject));
            if (logoContainerPrefab)
            {
                GameObject logoContainer = Instantiate(logoContainerPrefab) as GameObject;
                logoContainer.transform.parent = mainCamera.transform;
                logoContainer.transform.localPosition = Vector3.zero;
                logoContainer.transform.localRotation = Quaternion.identity;
            }
        }
    }

    [MenuItem("BuildHelper/Delete logo object")]
    static void DeleteLogo()
    {
        GameObject mainCamera = GameObject.Find("Camera (eye)");
        if (mainCamera)
        {
            foreach (Transform oneChild in mainCamera.transform)
            {
                if (oneChild.name.Contains("LogoContainer")) GameObject.DestroyImmediate(oneChild.gameObject);
            }
        }
    }

    [MenuItem("BuildHelper/Add checker")]
    static void AddChecker()
    {
        GameObject userChecker = GameObject.Find("UserCheck");
        if (!userChecker)
        {
            GameObject logoContainerPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/3rd-Party/BuildHelper/Editor/Material/UserCheck.prefab", typeof(GameObject));
            if (logoContainerPrefab)
            {
                GameObject logoContainer = Instantiate(logoContainerPrefab) as GameObject;
                logoContainer.name = "UserCheck";
            }
        }
    }

    [MenuItem("BuildHelper/build win x86")]
    static void BuildWindows_x86()
    {
        BuildWindows(BuildTarget.StandaloneWindows);
    }

    [MenuItem("BuildHelper/build win x64")]
    static void BuildWindows_x64()
    {
        BuildWindows(BuildTarget.StandaloneWindows64);
    }

    #region windows打包
    static void BuildWindows(BuildTarget _bt)
    {
        string path = EditorUtility.SaveFilePanel(_bt.ToString(), EditorPrefs.GetString("BuildPath"), PlayerSettings.productName, "exe");
        if (string.IsNullOrEmpty(path))
            return;
        BuildPlayerOptions _buildOptions = new BuildPlayerOptions();
        _buildOptions.locationPathName = path;
        _buildOptions.scenes = EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes);
        _buildOptions.target = _bt;
        //BuildPipeline.BuildPlayer(_buildOptions);

        //加密
        EncryptAssemblyCSharp(path);
        //替换解密mono.dll
        ReplaceMonoDll(path, _bt);

        int num = path.LastIndexOf("/");
        path = path.Substring(0, num);
        EditorPrefs.SetString("BuildPath", path);
        EditorUtility.OpenWithDefaultApp(path);
    }
    #endregion

    #region xxtea加密

    static void EncryptAssemblyCSharp(string path)
    {
        string _acsPath = path.Replace(".exe", "_Data") + "/Managed/Assembly-CSharp.dll";
        byte[] _readByte = File.ReadAllBytes(_acsPath);
        string key = RealString.MySecSum(Application.productName);
        byte[] encrypt_data = Xxtea.XXTEA.Encrypt(_readByte, key);
        File.WriteAllBytes(_acsPath, encrypt_data);
    }
    #endregion

    #region 替换为含解密的mono dll
    static void ReplaceMonoDll(string path, BuildTarget _bt)
    {
        string monoPath = "/Mono/mono.dll";
        string monoDllPath = "/mono_5_6.txt";
        if (Application.unityVersion.Contains("2017.4"))
        {
            monoPath = "/Mono/EmbedRuntime/mono.dll";
            monoDllPath = "/mono_2017_4.txt";
        }

        string _mdPath = path.Replace(".exe", "_Data") + monoPath;
        byte[] _readByte = File.ReadAllBytes(Application.dataPath + "/3rd-Party/BuildHelper/Editor/mono/" + _bt.ToString() + monoDllPath);
        File.WriteAllBytes(_mdPath, _readByte);
    }
    #endregion
}
