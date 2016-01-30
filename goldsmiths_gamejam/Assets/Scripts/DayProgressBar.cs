using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Slider))]
public class DayProgressBar : MonoBehaviour
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
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = 1.0f - (Time.time - startTime) / gm.dayLengh;
    }
}
