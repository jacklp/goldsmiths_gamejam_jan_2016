using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class InputController : MonoBehaviour {

    private StringBuffer inputBuffer;
    private List<Illness> currentIllnesses;
    private bool checkSequence;

    private static List<Illness> illnessesDef;

    public static List<Illness> IllnessesDef
    {
        get { return InputController.illnessesDef; }
    }
    private GameManager gameManager;

    private int nrIllnesses;

    public int NrIllnesses
    {
        get { return nrIllnesses; }
    }

	public event Action<bool, string> successComboEvent;

    void Awake() {
        inputBuffer = new StringBuffer();
        currentIllnesses = new List<Illness>();
        illnessesDef = new List<Illness>();

        if (PlayerPrefs.GetInt("hardmode") == 1) {
            illnessesDef.Add(new Illness("Arrow", "udruldur", 1.3f));
            illnessesDef.Add(new Illness("Axe", "llurdul", 1.0f));
            illnessesDef.Add(new Illness("Eye", "lrlruld", 1.2f));
            illnessesDef.Add(new Illness("Hair", "udldrulu", 1.3f));
            illnessesDef.Add(new Illness("Green", "dudulrud", 1.5f));
            illnessesDef.Add(new Illness("Knife", "rulruudru", 1.5f));
        } else {
            illnessesDef.Add(new Illness("Arrow", "dudu", 0.8f));
            illnessesDef.Add(new Illness("Hair", "udlr", 0.8f));
            illnessesDef.Add(new Illness("Eye", "lurldr", 1.3f));
            illnessesDef.Add(new Illness("Hair", "uurdd", 1.3f));
            illnessesDef.Add(new Illness("Green", "lldrru", 1.5f));
            illnessesDef.Add(new Illness("Knife", "durulu", 1.5f));
        }

        gameManager = GetComponent<GameManager>();

        checkSequence = false;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            inputBuffer.AddKey('r');
            checkSequence = true;
        } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            inputBuffer.AddKey('l');
            checkSequence = true;
        } else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            inputBuffer.AddKey('u');
            checkSequence = true;
        } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            inputBuffer.AddKey('d');
            checkSequence = true;
        }

        if (checkSequence) {
            int removeIndex = -1;
            inputBuffer.PrintBuffer();
            if (currentIllnesses.Count <= 0) {
                checkSequence = false;
                return;
            }
            Illness i = currentIllnesses[0];
            if (inputBuffer.CheckSequence(i.GetCombo(), i.GetTime())) {
                Debug.Log("********* Found combo: " + i.GetCombo());
                removeIndex = currentIllnesses.IndexOf(i);
                if (successComboEvent != null) {
                    successComboEvent(false, i.GetName());
                }
            }

            if (removeIndex >= 0) {
                //currentIllnesses.RemoveAt(removeIndex);
                if (currentIllnesses.Count <= 0) {
                    if (successComboEvent != null) {
                        successComboEvent(true, "");
                    }
                } else {
                    ComboUI.Instance.ClearChildren();
                    ComboUI.Instance.AddCombo(currentIllnesses[0].GetCombo());
                }
            }
            checkSequence = false;
        }

    }

    public void UpdateCurrentIllnesses() {
        currentIllnesses = gameManager.CurrentPatient.currentIllnesses;
        nrIllnesses = currentIllnesses.Count;
        Debug.Log("ILLNESSES: "+ nrIllnesses);
        /*
        currentIllnesses.Clear();

        int day = gameManager.currentDay;
        if (PlayerPrefs.GetInt("hardmode") == 1) {
            nrIllnesses = UnityEngine.Random.Range(Mathf.Max(4, day / 3), Mathf.Max(5, day / 2));
        } else {
            nrIllnesses = UnityEngine.Random.Range(Mathf.Max(1, day / 3), Mathf.Max(3, day / 2));
        }
        nrIllnesses = (nrIllnesses > 6) ? 6 : nrIllnesses;
        int limit = 5;
        for (uint i = 0; i < nrIllnesses; ++i) {
            int index = UnityEngine.Random.Range(0, limit);
            currentIllnesses.Add(illnessesDef[index]);
            Illness temp = illnessesDef[index];
            illnessesDef[index] = illnessesDef[5];
            illnessesDef[5] = temp;
            --index;
        }
         * */
    }

    public string GetTopCombo() {
        if (currentIllnesses.Count > 0) {
            return currentIllnesses[0].GetCombo();
        }
        return "";
    }

    private class StringBuffer {

        private KeyTime[] buffer;

        public StringBuffer() {
            buffer = new KeyTime[10];
            for (uint i = 0; i < 10; ++i) {
                KeyTime kt = new KeyTime('-', 0.0f);
                buffer[i] = kt;
            }
        }

        public void ClearBuffer() {
            for (uint i = 0; i < 10; ++i) {
                buffer[i].SetCharacter('-');
                buffer[i].SetTime(0.0f);
            }
        }

        public void AddKey(char c) {
            for (uint i = 9; i > 0; --i) {
                buffer[i].SetCharacter(buffer[i-1].GetCharacter());
                buffer[i].SetTime(buffer[i-1].GetTime());
            }
            buffer[0].SetCharacter(c);
            buffer[0].SetTime(Time.time);
        }

        public bool CheckSequence(string combo, float time) {

            if (combo.Length > 10) {
                Debug.Log("String: " + combo + " is too long!");
                return false;
            }

            for (int i = combo.Length - 1; i >= 0; --i) {
                if (buffer[i].GetCharacter() != combo[i]) {
                    return false;
                }
            }

            if (buffer[0].GetTime() - buffer[combo.Length - 1].GetTime() > time) {
                ComboUI.Instance.FlashArrows();
                return false;
            }

            ClearBuffer();
            return true;
        }

        public void PrintBuffer() {
            string chars = "";
            for (int i = 9; i >= 0; --i) {
                chars += buffer[i].GetCharacter();
            }
            Debug.Log("Buffer Chars: " + chars);
        }

    }

    private class KeyTime {
        private char character;
        private float time;

        public KeyTime(char character, float time) {
            this.character = character;
            this.time = time;
        }

        public char GetCharacter() { return character; }
        public void SetCharacter(char c) { this.character = c; }
        public float GetTime() { return time; }
        public void SetTime(float t) { this.time = t; }
    }
}
