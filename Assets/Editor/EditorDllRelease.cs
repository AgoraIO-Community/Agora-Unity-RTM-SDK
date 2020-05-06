using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class EditorDllRelease
{
    static void Quit()
    {
        #if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        RtmWrapperDll.release();
        #endif
    }

    static EditorDllRelease()
    {
#if UNITY_2018_1_OR_NEWER
        EditorApplication.quitting += Quit;
#endif
    }
}
