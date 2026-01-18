using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RanBackgroundNoise : MonoBehaviour {

    public AudioSource twoDimAudioSource;
    public AudioClip[] RandomClips;
    public float minInterval, maxInterval, startingTime;
    public bool startImmediately = true;
    public GameObject monster;

    private int lastClipIndex;
    private Coroutine noiseRoutine;

    // Start is called before the first frame update
    void Start() {
        if(startImmediately) StartCoroutine(RandomBackgroundNoiseTimer());
    }

    public void StartNoises() {
        StopCoroutine(noiseRoutine);
        StartCoroutine(RandomBackgroundNoiseTimer2());
    }

    private IEnumerator RandomBackgroundNoiseTimer() {
        yield return new WaitForSeconds(startingTime);
        noiseRoutine = StartCoroutine(RandomBackgroundNoiseTimer2());
    }

    private IEnumerator RandomBackgroundNoiseTimer2() {
        yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));
        int newIndex = Random.Range(0, RandomClips.Length);
        if(newIndex == lastClipIndex) newIndex = newIndex / 2 + 1;
        
        lastClipIndex = newIndex;
        StartCoroutine(RandomBackgroundNoiseTimer2());
    }
}
