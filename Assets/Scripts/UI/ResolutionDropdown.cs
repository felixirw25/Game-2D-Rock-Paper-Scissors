using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResolutionDropdown : MonoBehaviour
{
    [SerializeField] TMP_Dropdown resolutionDropdown;
    public void GetOptionResolusi(int value){    
        string[] resolusi = resolutionDropdown.options[value].text.Split("x");
        int.TryParse(resolusi[0], out int width);
        int.TryParse(resolusi[1], out int height);
        // Debug.LogFormat("Resolusi Sekarang: {0} x {1}", width, height);

        Screen.SetResolution(width, height, Screen.fullScreen);
    }
}
