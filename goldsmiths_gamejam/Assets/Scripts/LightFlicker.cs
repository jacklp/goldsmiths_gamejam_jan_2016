using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Light))]
public class LightFlicker : MonoBehaviour {

    public float speed = 10f;
    public float intensityMin = 0.1f;
    public float intensityMax = 1.1f;

    Light light;
	// Use this for initialization
	void Start () {
        light = GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {
        light.intensity = Mathf.Lerp(light.intensity, Random.Range(intensityMin, intensityMax), Time.deltaTime * speed);
	}
}
