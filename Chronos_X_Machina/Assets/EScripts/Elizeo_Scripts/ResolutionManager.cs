using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class ResolutionManager : MonoBehaviour
{
    [Header("Dropdown thing for the Resolution")]
    [SerializeField]private TMP_Dropdown resDropdown;
    
    private Resolution[] res;
    private List<Resolution> filteredRes;

    private float currentRefreshRate;
    private int currentResolutionIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        res = Screen.resolutions;
        filteredRes = new List<Resolution>();

        resDropdown.ClearOptions();
        currentRefreshRate = (float)Screen.currentResolution.refreshRateRatio.value;

        for (int i = 0; i < res.Length; i++)
        {
            if ((float)res[i].refreshRateRatio.value == currentRefreshRate)
            {
                filteredRes.Add(res[i]);
            }
        }

        filteredRes.Sort((a, b) =>
        {
            if (a.width != b.width)
            {
                return b.width.CompareTo(a.width);
            }
            else
            {
                return b.height.CompareTo(a.height);
            }
        });

        List<string> options = new List<string>();
        for (int i = 0; i < filteredRes.Count; i++)
        {
            string resolutionOption = filteredRes[i].width + "x" + filteredRes[i].height ; ;
            options.Add(resolutionOption);

            if (filteredRes[i].width == Screen.width && filteredRes[i].height == Screen.height && (float)filteredRes[i].refreshRateRatio.value == currentRefreshRate)
            {
                currentResolutionIndex = i;
            }
        }

        resDropdown.AddOptions(options);
        resDropdown.value = currentResolutionIndex = 0;
        resDropdown.RefreshShownValue();
        SetResolution(currentResolutionIndex);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution res = filteredRes[resolutionIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
}
