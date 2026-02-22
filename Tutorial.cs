using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {

    public enum TutorialState { Waiting, Wasd, Shift, Crouch, Jump, Hiding, TabClues, TabAspects}
    public TutorialState currentState = TutorialState.Waiting;

    public string[] tutorialLists;
    public Door[] tutorialDoors;
    public int index = 0, doorIndex = 0;

    public AudioClip textSound;
    public PlayerMovement playerScript;


    public bool usedHidingSpot = false, walkedAround = false, checkedInventory = false;

    public float walkedAroundAmount = 0f;
    private TextPrompter textPromptScript;
    private Coroutine routine;
    private bool done = false;

    private void OnEnable() {
        //start the first one. index = 0
    }


    private void Increment() {
        index++;
        // sound jingle
    }

    private void IncrementDoor() {
        tutorialDoors[doorIndex].UnlockDoor();
        index++;
        doorIndex++;
        // door unlock
        // better sound
    }

    // Called by individual hitboxes for the major tutorial starts. Called by Update() for the mini tutorial starts.
    public void StartNextTutorial() {
        if(currentState == TutorialState.Waiting) {
            playerScript.allowedToMove = true;
            currentState = TutorialState.Wasd;
        }
        DisplayIt(tutorialLists[index]);
    }

    // Update is called once per frame
    void Update() {

        if(!walkedAround && (Input.GetAxis("Horizontal") + Input.GetAxis("Vertical") != 0)) {
            walkedAroundAmount += 1 * Time.deltaTime;
            if(walkedAroundAmount >= 5) {
                walkedAround = true;
            }
        }
        if(currentState == TutorialState.TabClues && Input.GetKeyDown(KeyCode.Tab)) {
            checkedInventory = true;
        }

            // The text box should already be there.
        if(currentState == TutorialState.Wasd && walkedAround) {
            // Done with movement.
            Increment();
            currentState = TutorialState.Shift;
            StartNextTutorial();
        }
        else if(currentState == TutorialState.Shift && playerScript.GetRemainingStam() < 0.9f) {//(currentState == TutorialState.Shift && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D)) && Input.GetKeyDown(KeyCode.LeftShift)) {
            // Done with sprinting.
            Increment();
            currentState = TutorialState.Crouch;
            StartNextTutorial();
        }
        else if(currentState == TutorialState.Crouch && Input.GetKeyDown(KeyCode.LeftControl)) {
            Increment();
            currentState = TutorialState.Jump;
            StartNextTutorial();
        }
        else if(currentState == TutorialState.Jump && Input.GetKeyDown(KeyCode.Space)) {
            IncrementDoor();
            StartNextTutorial();
            index++; // We are doing one more because of the door hint.
            currentState = TutorialState.Hiding;
        }
        else if(currentState == TutorialState.Hiding && usedHidingSpot) {
            IncrementDoor();
            currentState = TutorialState.TabClues;
        }
        else if(currentState == TutorialState.TabClues && checkedInventory == true && Input.GetKeyDown(KeyCode.Tab)) {
            IncrementDoor();
            currentState = TutorialState.TabAspects;
        }
        else if(currentState == TutorialState.TabAspects && checkedInventory == true && Input.GetKeyDown(KeyCode.Tab)) {
            IncrementDoor();
            currentState = TutorialState.Waiting;
        }
    }


    private void DisplayIt(string displayText) {
        if(textPromptScript == null) textPromptScript = GameObject.Find("Game Manager").GetComponent<TextPrompter>();

        if(displayText.Contains("_")) textPromptScript.QueueTextPrompt(displayText.Split('_'), textSound);
        else textPromptScript.QueueTextPrompt(displayText, textSound);

        displayText = displayText.Replace('_', ' ');

        tutorialDoors[doorIndex].GetComponent<InteractPrompt>().displayText = displayText;
    }

}
