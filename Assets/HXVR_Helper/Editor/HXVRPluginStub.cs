using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HXVRPluginStub : ScriptableObject
{
    // Stub helper class to locate script paths through Unity Editor API.
    // Required to be a standalone class in a separate file or else MonoScript.FromScriptableObject() returns an empty string path.
}
