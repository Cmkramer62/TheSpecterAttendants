using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;

public class CursedObject : MonoBehaviour {

    public enum CursedTypes { Glowing, EMF, Aura, Thermo, Unholy, Sound}
    public List<CursedTypes> cursesList;

    public GameObject distortion;
    public Light geistLight;
    public ParticleSystem geistLightParticles;
    public int emfLevel = 7, temperature = -20;

    public ToolController toolControllerScript;
    public List<int> index;
    private Coroutine lightRoutine;
    public float charge = 0f, defaultMinLight = 0f, defaultMaxLight = 0.1f;

    private bool lowering = false;

    public AudioSource source, geistAudioA, geistAudioB;
    public AudioClip geistlightClip, cameraWhooshClip;
    public AudioClip[] cursedAudioClips;

    public ParticleSystem purificationParticles;
    public GameObject purificationCanvas;
    public Slider purificationSlider;
    public AudioSource pSourceA, pSourceB;

    void Start() {
        pSourceA.Play();
        pSourceA.Stop();

        pSourceB.Play();
        pSourceB.Stop();

        purificationParticles.Play();
        purificationParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    void Awake() {
        var LAC = purificationCanvas.GetComponent<LookAtConstraint>();
        LAC.locked = false;
        var source = new ConstraintSource {
            sourceTransform = Camera.main.transform,
            weight = 1f
        };

        LAC.AddSource(source);
        LAC.locked = true;
    }

    public void SetRandomGoal() {
        //Debug.Log("Random Goals " + cursesList.Count);
        if(cursesList.Count == 3) return;
        else {
            CursedTypes curseToAdd;
            int rand = Random.Range(0, 6);
            if(rand == 0) curseToAdd = CursedTypes.Glowing;
            else if(rand == 1) curseToAdd = CursedTypes.EMF;
            else if(rand == 2) curseToAdd = CursedTypes.Aura;
            else if(rand == 3) {
                curseToAdd = CursedTypes.Thermo;
                temperature = -20;
            }
            else if(rand == 4) curseToAdd = CursedTypes.Unholy;
            else curseToAdd = CursedTypes.Sound;

            if(!cursesList.Contains(curseToAdd)) {
                cursesList.Add(curseToAdd);
                //Debug.Log("Goal Curse: " + curseToAdd.ToString());
                index.Add(rand);
            }
            SetRandomGoal();
            
        }
    }

    public void SetRandomCurses() {
        if(cursesList.Count == 2) return;
        else {
            CursedTypes curseToAdd;
            int rand = Random.Range(0, 6);
            if(rand == 0) curseToAdd = CursedTypes.Glowing;
            else if(rand == 1) curseToAdd = CursedTypes.EMF;
            else if(rand == 2) curseToAdd = CursedTypes.Aura;
            else if(rand == 3) {
                curseToAdd = CursedTypes.Thermo;
                temperature = -20;
            }
            else if(rand == 4) curseToAdd = CursedTypes.Unholy;
            else curseToAdd = CursedTypes.Sound;

            if(!cursesList.Contains(curseToAdd)) {
                cursesList.Add(curseToAdd);
            }
            SetRandomCurses();
            
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.name == "Player") {
            toolControllerScript.objectsList.Add(this);

            //if(cursesList.Contains(CursedTypes.Thermo)) {
            //    toolControllerScript.defaultTemp = temperature;
            //}
            if(cursesList.Contains(CursedTypes.EMF)) {
                toolControllerScript.defaultEMF = emfLevel;
            }
        }
    }

    public void Update() {
        if(charge > 0) {
            charge -= 10 * Time.deltaTime;
            lowering = true;
        }
        
        if(charge <= 0 && lowering) {
            lowering = false;
            DisplayCurse(CursedTypes.Glowing, false);
        }

    }


    private void OnTriggerExit(Collider other) {
        if(other.name == "Player") {
            toolControllerScript.objectsList.Remove(this); //flawed. What if another curse removes itself before
                                                           // we have a chance to for this specific one?
           // if(cursesList.Contains(CursedTypes.Thermo)) {
           //     toolControllerScript.defaultTemp = 60;
           // }
            if(cursesList.Contains(CursedTypes.EMF)) {
                toolControllerScript.defaultEMF = 0;
            }
        }
    }

    public void DisplayCurse(CursedTypes type, bool state) {
        bool found = false;
        foreach(CursedTypes curCurse in cursesList) {
            if(type == curCurse) found = true;
        }
        // run a check to see if the "type" is even in our list of curses in "cursesList".
        if(found && type == CursedTypes.Glowing) {
            //geistLight.gameObject.SetActive(state);
            //Debug.Log("starting routine");
            if(lightRoutine != null) StopCoroutine(lightRoutine);
            lightRoutine = StartCoroutine(LerpLight(state));
            if(state) {
                geistAudioA.volume = 0.773f;
                geistAudioA.Play();
                geistAudioB.volume = 1f;
                geistAudioB.Play();
                geistLightParticles.Play();
            }
            else {
                AudioController.FadeOutAudio(this, geistAudioA, .1f);
                AudioController.FadeOutAudio(this, geistAudioB, .1f);
                geistLightParticles.Stop();
            }
            //if(state) source.PlayOneShot(geistlightClip);
        }
        if(found && type == CursedTypes.Aura) {
            distortion.SetActive(state);
            if(!source.isPlaying) source.PlayOneShot(cameraWhooshClip, 1);
            // play jumpscare sound? Something very light. Perhaps even from a small random array of them.
            // is this a common thing amongst other curse reveals?..
        }
        if(found && type == CursedTypes.Sound) {
            source.pitch = Random.Range(.8f, 1.2f);
            source.PlayOneShot(cursedAudioClips[Random.Range(0, cursedAudioClips.Length)]);
        }
    }

    private IEnumerator LerpLight(bool state) {
        float start, end;
        
        if(state) {
            start = geistLight.intensity;
            end = 0.1f;
        }
        else {
            start = geistLight.intensity;
            end = 0f;
        }

        float time = 0f;
        // geistLight.intensity = start;

        while(time < 3) {
            time += Time.deltaTime;
            float t = time / 1;

            geistLight.intensity = Mathf.Lerp(start, end, t);
            yield return null;
        }

        geistLight.intensity = end; // ensure final value hits exactly 1
    }
}
