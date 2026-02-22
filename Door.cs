using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    public bool state, locked = false, unlockable = false, sceneLoading = false, causeInteraction = false, menuScene = false;
    public string keyname;
    public int sceneName;
    private Animator animator;

    private AudioSource source;
    [SerializeField]
    private AudioClip openClip, closeClip, lockedClip, unlockClip;
    private GameObject gameManager;
    
    private void OnEnable() {
        if(animator == null) animator = GetComponent<Animator>();
        if(source == null) source = GetComponentInChildren<AudioSource>();
        if(gameManager == null) gameManager = GameObject.Find("Game Manager");
    }

    /*
     * Interacts with door, either opening/closing, unlocking, or not budging and saying text.
     */
    public void InteractDoor() {
        if(!locked) {
            OpenCloseDoor();
        }
        else if (unlockable && locked && gameManager.GetComponent<Inventory>().inventoryDictionary.ContainsKey(keyname)){
            UnlockDoor();
        }
        else {
            source.Stop();
            source.pitch = Random.Range(.9f, 1.1f);
            source.PlayOneShot(lockedClip);
            GetComponent<InteractPrompt>().InteractWithObject();
        }
    }

    public void UnlockDoor() {
        locked = false;
        source.Stop();
        source.PlayOneShot(unlockClip);
    }

    /*
     * Opens or closes the door based on the state bool.
     */
    public void OpenCloseDoor() {
        state = !state;
        animator.SetBool("open", state);
        source.Stop();
        source.pitch = Random.Range(.9f, 1.1f);
        if(state) source.PlayOneShot(openClip);
        else source.PlayOneShot(closeClip);
        if(causeInteraction) {
            GetComponent<InteractPrompt>().EndEffect();
            causeInteraction = false;
        }
        if(sceneLoading) { 
            if(menuScene) Cursor.lockState = CursorLockMode.None;
            gameManager.GetComponent<SceneLoader>().LoadScene(sceneName); 
        }
    }

}
