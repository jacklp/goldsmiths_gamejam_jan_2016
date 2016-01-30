using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class Fader : MonoBehaviour {

    public bool autoStart = true;
    public float fadeTime = 1.0f;
    public float finalAlpha;
    CanvasGroup image;

    public float startTime;
    Color startColor;
	// Use this for initialization
	void Start () {
        image = GetComponent<CanvasGroup>();
        //startColor = image.color;
	}

    void OnEnable()
    {
        if (autoStart)
        {
            startTime = Time.time;
        }
    }
	
	// Update is called once per frame
	void Update () {
        image.alpha = Mathf.Lerp(image.alpha, finalAlpha, Time.deltaTime * fadeTime);
	}
}
