using UnityEngine;
using TMPro;

public class TextAdder : MonoBehaviour {

    public float interval = 0.06f, delay = 0.0f;

    public string endWord;
    private TextMeshProUGUI text;
    private int i = 0;

    public bool useAudio = false;
    public AudioSource source;
    public AudioClip clip; // can be set by another script.

    private void OnEnable() {
        StartAddingText();
    }

    public void StartAddingText() {
        text = GetComponent<TextMeshProUGUI>();
        //endWord = text.text;
        text.text = "";
        i = 0;
        InvokeRepeating("AddingText", delay, interval);

        
    }

    private void AddingText() {
        if(i >= endWord.Length) {
            CancelInvoke("AddingText");
            if(useAudio) source.Pause();
        }
        else {
            if(useAudio && !endWord[i].Equals(' ')) {
                source.Stop();
                source.PlayOneShot(clip); }
            text.text += endWord[i];
            i++;
        }
        
    }

    public void CancelText() {
        CancelInvoke("AddingText");
        text.text = "";
        if(useAudio) source.Pause();
    }
}
