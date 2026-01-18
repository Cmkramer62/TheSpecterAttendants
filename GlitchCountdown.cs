using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GlitchCountdown : MonoBehaviour {

    public int currentInt = -1, maxPosOdds = 3; // 3 = 1/3 chance of glitching
    public float initialWaitTime = 0f, repeatRate = .1f;
    public TextMeshProUGUI text;
    public Animator animator;
    public GameObject eyeIcon;

    private string[] symbols = { "a", "A", "e", "E", "b", "B", "d", "D", "i" , "I", "p", "P", "h", "H", "v", "V",
        "1", "2", "3", "4", "5", "6", "7", "8", "9", "!", "*", "$", "<", ">", "/", "-"};

    public void OnEnable() {
        StopGlitchText();
        InvokeRepeating("GlitchText", initialWaitTime, Random.Range(repeatRate - 0.01f, repeatRate + 0.01f));
    }

    private void GlitchText() {
        if(Random.Range(0, maxPosOdds) == 0) {
            text.text = symbols[Random.Range(0, symbols.Length)];
        }
        else text.text = currentInt.ToString();
    }

    public void StopGlitchText() {
        CancelInvoke("GlitchText");
    }

    // current number

    // continually, the number flickers to nonsense, like other numbers or ssymbols or letters. maybe even words.

    // This never stops.

    // more of these numbers appears as the countdown gets lower.
    
    // when the grace stops and countdown begins, color turns red/ or flickering gets worse?

    // when countdown ends and searching starts, either the numbers fade away or they turn into eyes.



}
