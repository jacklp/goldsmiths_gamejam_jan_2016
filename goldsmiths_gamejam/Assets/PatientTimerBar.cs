using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Slider))]
public class PatientTimerBar : MonoBehaviour
{
    Slider slider;
    public float startTime;
    GameManager gm;

    // Use this for initialization
    void OnEnable()
    {
        startTime = Time.time;
        gm = FindObjectOfType<GameManager>();
        slider = GetComponent<Slider>();
        slider.value = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = gm.patientTime / gm.patientTimeOut;
    }
}
