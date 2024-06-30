using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringProcess : MonoBehaviour
{
    public static string Trim(string str)
    {
        str = str.Trim();
        str = str.Replace("\u200B", "");
        return str;
    }
}
