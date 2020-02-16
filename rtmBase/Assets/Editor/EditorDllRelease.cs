using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class EditorDllRelease
{
    static void Quit()
    {
        RtmWrapperDll.release();
    }

    static EditorDllRelease()
    {
        EditorApplication.quitting += Quit;
    }
}
