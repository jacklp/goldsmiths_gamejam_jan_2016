using UnityEngine;
using System.Collections;

public class Illness {

    private string name;

    // Key combination for curing
    private string combo;

    // Time to input the key combination (in seconds)
    private float time;

    public Illness() {
        name = "Default";
        combo = "";
        time = 0.0f;
    }

    public Illness(string name, string combo, float time = 3.0f) {
        this.name = name;
        this.combo = combo;
        this.time = time;
    }

    public string GetName() { return name; }
    public void SetName(string name) { this.name = name; }
    public string GetCombo() { return combo; }
    public void SetCombo(string combo) { this.combo = combo; }
    public float GetTime() { return time; }
    public void SetTime(float time) { this.time = time; }

}
