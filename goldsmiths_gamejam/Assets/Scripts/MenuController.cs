using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

    public Slider volume;
    public Text normalText;
    public Text hardText;

    public Canvas menuCanvas;
    public Canvas creditsCanvas;

    public void PlayButtonPressed() {
        Application.LoadLevel("GameManagerTest");
    }

    void Awake() {
        PlayerPrefs.SetInt("hardmode", 0);
        PlayerPrefs.SetFloat("volume", 1.0f);
    }

    public void NormalButtonPressed() {
        normalText.fontSize = 30;
        hardText.fontSize = 25;
        PlayerPrefs.SetInt("hardmode", 0);
    }

    public void HardButtonPressed() {
        normalText.fontSize = 25;
        hardText.fontSize = 30;
        PlayerPrefs.SetInt("hardmode", 1);
    }

    public void CreditsButtonPressed() {
        creditsCanvas.enabled = true;
        menuCanvas.enabled = false;
    }

    public void SliderChanged() {
        PlayerPrefs.SetFloat("volume", volume.value);
    }

    public void BackButtonPressed() {
        menuCanvas.enabled = true;
        creditsCanvas.enabled = false;
    }
}
