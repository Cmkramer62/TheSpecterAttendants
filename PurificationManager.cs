using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Animations;

public class PurificationManager : MonoBehaviour {
    
    public GameObject mainCamera;
    [HideInInspector]
    public GameObject potentialCursedItem;
    public int ritualTimer;
    public ConeLOSDetector ghostVisionScript;
    public Enemy ghostScript;
    public bool allowedToDisplayQuestion = false;
    public EndPortal endPortalScript;
    [HideInInspector]
    public CursedObject cursedObjectScript;

    private int ritualTimerMax;

    public void DisplayQuestion() {
        if(!allowedToDisplayQuestion) {
            GetComponent<PauseGame>().ActivateQuestionPause();
        }
    }

    public void StartPurificationRitual() {
        DisplayQuestion();


       
        StartCoroutine(PurificationTimer());



        if(ghostScript.invisible) ghostScript.InvertVisibility();
        ghostVisionScript.visibilityOverride = true;
        endPortalScript.activated = true;
        allowedToDisplayQuestion = true;
    }

    private IEnumerator PurificationTimer() {
        yield return new WaitForSeconds(1f);
        cursedObjectScript.pSourceA.Play();
        yield return new WaitForSeconds(1f);
        cursedObjectScript.pSourceB.Play();
        yield return new WaitForSeconds(1f);
        cursedObjectScript.purificationParticles.Play();

        yield return new WaitForSeconds(ritualTimer);
        GetComponent<Death>().Jumpscare();
    }
}
