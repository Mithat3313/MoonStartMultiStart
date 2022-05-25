using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Management : MonoBehaviour
{

    public GameObject panel;
    public GameObject leftButton;
    public GameObject rightButton;

    public InputField leftField;
    public InputField rightField;

    private Sprite loadedLeftSprite;
    private Sprite loadedRightSprite;

    private string leftApp;
    private string rightApp;

    // Start is called before the first frame update
    void Start()
    {
        rightApp = PlayerPrefs.GetString("rightTarget", "");
        leftApp = PlayerPrefs.GetString("leftTarget", "");
        rightField.text = rightApp;
        leftField.text = leftApp;
        UpdateTexts();
    }

    public void StartRightApp() {
        StartAppPosition(rightApp);
    }

    public void StartLeftApp() {
        StartAppPosition(leftApp);
    }

    public void SaveSettings() {
        PlayerPrefs.SetString("rightTarget", rightField.text);
        PlayerPrefs.SetString("leftTarget", leftField.text);
        rightApp = rightField.text;
        leftApp = leftField.text;
        UpdateTexts();
        HidePanel();
    }

    private void UpdateTexts() {
        leftButton.GetComponentInChildren<Text>().text = leftApp;
        rightButton.GetComponentInChildren<Text>().text = rightApp;
    }

    public void ShowPanel() {
        panel.SetActive(true);
    }

    public void HidePanel() {
        panel.SetActive(false);
    }

    public void SelectImageFromPC() {
        SFB.StandaloneFileBrowser.OpenFilePanelAsync("Open File", "", "", false, (string[] paths) => {
            if (paths.Length > 0) {
                //Change this to change pictures folder
                StartCoroutine(LoadAll(paths[0]));
            }
            UnityEngine.Debug.Log("path:" + paths[0].ToString());

        });
    }

    private void StartAppPosition(string pos) {
        try {
            Process p = new Process();
            p.StartInfo.UseShellExecute = true;
            p.StartInfo.FileName = pos;
            p.Start();
        } catch (Exception e) {
            print(e);
        }
    }


    public IEnumerator LoadAll(string filePath) {
        WWW load = new WWW("file:///" + filePath);
        yield return load;
        if (!string.IsNullOrEmpty(load.error)) {
            UnityEngine.Debug.LogWarning(filePath + " error");
        } else {
            try {
                leftButton.GetComponent<Image>().material.mainTexture = load.texture;

            } catch (System.Exception e) {
                UnityEngine.Debug.LogError(e);
            }
        }
    }


}
