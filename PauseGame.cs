using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour {

    public GameObject normalUI, pausedUI, pausedUIText, questionUI;//, question, title, mapUI;
    public bool paused = false, questionPaused = false;
    public MouseLook mouseLook;
    public PlayerMovement playerMovement;

    public AudioSource source;
    public AudioClip clip;

    public GameObject[] pausedUIObjects;

    public bool allowedToPause = true, allowedToQuestionPause = true;

    // Update is called once per frame
    void Update() {
        if(allowedToPause && (Input.GetKeyDown(KeyCode.Tab))) {
            //mapUI.SetActive(false);
            if(!paused) {
                source.PlayOneShot(clip);
                normalUI.SetActive(false);
                pausedUI.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                paused = true;

                mouseLook.allowedToLook = false;
                playerMovement.allowedToMove = false;
               // question.SetActive(false);
                //title.SetActive(true);
            }
            else {
                AudioManager.PlayDeny();
                normalUI.SetActive(true);
                pausedUI.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                paused = false;

                mouseLook.allowedToLook = true;
                playerMovement.allowedToMove = true;
                ExitPausedUI();
            }
        }
    }

    public void ActivateQuestionPause() {
        if(allowedToQuestionPause) {
            source.PlayOneShot(clip);
            if(!questionPaused) {
                //normalUI.SetActive(false);
                questionUI.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                allowedToPause = false;
                questionPaused = true;
                GetComponent<ToolController>().allowedToCycle = false;
                mouseLook.allowedToLook = false;
                playerMovement.allowedToMove = false;
            }
            else {
                //normalUI.SetActive(true);
                questionUI.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                allowedToPause = true;
                questionPaused = false;
                GetComponent<ToolController>().allowedToCycle = true;
                mouseLook.allowedToLook = true;
                playerMovement.allowedToMove = true;
            }
        }
    }

    public void PausedUIActive(int index) {
        for(int i = 0; i < pausedUIObjects.Length; i++) {
            pausedUIObjects[i].SetActive(i == index);
        }
    }

    public void ExitPausedUI() {
        for(int i = 0; i < pausedUIObjects.Length; i++) {
            pausedUIObjects[i].SetActive(false);
        }
        pausedUIText.SetActive(true);
    }

}
