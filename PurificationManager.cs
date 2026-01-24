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
        ritualTimerMax = ritualTimer;
        cursedObjectScript = potentialCursedItem.GetComponentInChildren<CursedObject>();
        var canvas = cursedObjectScript.purificationCanvas;

        canvas.GetComponentInChildren<LookAtConstraint>().constraintActive = true;
        cursedObjectScript.pSourceA.Play();
        cursedObjectScript.pSourceB.Play();
        cursedObjectScript.purificationParticles.Play();
        cursedObjectScript.purificationSlider.maxValue = ritualTimerMax;

        //StartCoroutine(PurificationTimer(canvas));
        if(ghostScript.invisible) ghostScript.InvertVisibility();
        ghostVisionScript.visibilityOverride = true;
        DisplayQuestion();
        endPortalScript.activated = true;
        allowedToDisplayQuestion = true;
    }

    private IEnumerator PurificationTimer(GameObject canvas) {

        if(ritualTimer != 0) {
            ritualTimer--;
            
            canvas.transform.GetChild(0).GetComponentInChildren<Slider>().value = ritualTimer;
            yield return new WaitForSeconds(1f);
            StartCoroutine(PurificationTimer(canvas));
        }
        else {
            GetComponent<Death>().Jumpscare();
        }
        

    }
}
