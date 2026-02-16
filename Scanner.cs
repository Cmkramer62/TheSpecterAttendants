using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour {

    public GameObject[] levelsUI;
    public Animator scannerAnimator;
    public int levelEMF = 0, fakeEMF = 0;
    public bool allowedToScan = true;

    public AudioSource source;
    public AudioClip beep; // play same beep but at different pitches.??

    private Coroutine fluctuationRoutine;

    private void OnEnable() {
        allowedToScan = true;
    }

    // Start is called before the first frame update
    void Start() {
        InvokeRepeating("RandomFluctuation", 0, Random.Range(1, 12));  //in one second, start calling this function, every 2secs
        //ActivateEffectsEMF(2);
    }

    // Update is called once per frame
    void Update() {
        if(Input.GetKeyDown(KeyCode.Mouse0) && allowedToScan) {
            scannerAnimator.SetBool("ScannerView", true);
        }
        if(Input.GetKeyUp(KeyCode.Mouse0) || !allowedToScan) {
            scannerAnimator.SetBool("ScannerView", false);
        }

    }

    private void RandomFluctuation() {
        if(fluctuationRoutine != null) StopCoroutine(fluctuationRoutine);
        if(gameObject.activeSelf) fluctuationRoutine = StartCoroutine(FluctuationActivationTimer());

        if(!gameObject.activeSelf) ActivateEffectsEMF(levelEMF);
    }

    private IEnumerator FluctuationActivationTimer() {
        FluctuationAmount();
        ActivateEffectsEMF(fakeEMF);
        yield return new WaitForSeconds(Random.Range(0.1f, 1f));
        FluctuationAmount();
        ActivateEffectsEMF(fakeEMF);
        yield return new WaitForSeconds(Random.Range(0.1f, 1f));
        FluctuationAmount();
        ActivateEffectsEMF(fakeEMF);
        yield return new WaitForSeconds(Random.Range(0.1f, 5f));
        ActivateEffectsEMF(levelEMF);
    }

    public void FluctuationAmount() {
        fakeEMF = levelEMF + Random.Range(-2, 3);
        if(fakeEMF <= -1) fakeEMF = 0;
        else if(fakeEMF > 8) fakeEMF = 8;
    }

    public void ActivateEffectsEMF(int level) {
        for(int i = 0; i < levelsUI.Length; i++) {
            levelsUI[i].SetActive(i < level);
        }
        if(level != 0) {
            source.pitch = .8f;
            source.pitch += level / 10f;
            source.PlayOneShot(beep);
        }
    }

}
