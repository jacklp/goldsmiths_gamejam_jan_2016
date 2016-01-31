using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

    public Slider volume;
    public Text normalText;
    public Text hardText;

    public Canvas menuCanvas;
    public Canvas creditsCanvas;
    public Canvas tutorialCanvas;

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
        menuCanvas.gameObject.SetActive(false);
        creditsCanvas.gameObject.SetActive(true);
    }

    public void SliderChanged() {
        PlayerPrefs.SetFloat("volume", volume.value);
    }

    public void BackButtonPressed() {
        menuCanvas.gameObject.SetActive(true);
        creditsCanvas.gameObject.SetActive(false);
    }

    public void TutorialButtonPressed() {
        tutorialCanvas.gameObject.SetActive(true);
        menuCanvas.gameObject.SetActive(false);
    }

    public void TutorialBackButtonPressed() {
        tutorialCanvas.gameObject.SetActive(false);
        menuCanvas.gameObject.SetActive(true);
    }
}
