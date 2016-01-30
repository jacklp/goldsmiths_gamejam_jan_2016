using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour {

    public AudioSource audio1;
    public AudioSource audio2;

	void Start () {
        float volume = PlayerPrefs.GetFloat("volume");
        audio1.volume = volume/10;
        audio2.volume = volume/10;
	}
	
}
