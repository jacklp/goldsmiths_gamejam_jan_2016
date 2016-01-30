using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class InputController : MonoBehaviour {

    private StringBuffer inputBuffer;
    private List<Illness> currentIllnesses;
    private bool checkSequence;

    private static List<Illness> illnessesDef;
    private GameManager gameManager;

	public event Action<bool> successComboEvent;

    void Start() {
        inputBuffer = new StringBuffer();
        currentIllnesses = new List<Illness>();
        illnessesDef = new List<Illness>();

        illnessesDef.Add(new Illness("Arrow", "ddd", 1.3f));
        illnessesDef.Add(new Illness("Arrow", "ddd", 1.3f));
        illnessesDef.Add(new Illness("Arrow", "ddd", 1.3f));
        illnessesDef.Add(new Illness("Arrow", "uuu", 1.3f));
        illnessesDef.Add(new Illness("Arrow", "uuu", 1.3f));
        illnessesDef.Add(new Illness("Arrow", "uuu", 1.3f));

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
            foreach (Illness i in currentIllnesses) {
                if (inputBuffer.CheckSequence(i.GetCombo(), i.GetTime())) {
                    Debug.Log("********* Found combo: " + i.GetCombo());
                    removeIndex = currentIllnesses.IndexOf(i);
					if(successComboEvent != null)
					{
						successComboEvent(false);
					}
                }
            }

            if (removeIndex >= 0) {
                currentIllnesses.RemoveAt(removeIndex);
                if (currentIllnesses.Count <= 0) {
                    if (successComboEvent != null) {
                        successComboEvent(true);
                    }
                }
            }
            checkSequence = false;
        }

    }

    public void UpdateCurrentIllnesses() {
        currentIllnesses.Clear();

        int day = gameManager.currentDay;
        int nrIllnesses = UnityEngine.Random.Range(Mathf.Max(1, day / 3), Mathf.Max(1, day / 2));
        nrIllnesses = 3;
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
