
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveData_Script : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }     
}



