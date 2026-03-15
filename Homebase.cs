using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Homebase : MonoBehaviour {

    public AudioSettings saveSystem;
    public TextMeshProUGUI resultsText;
    public GameObject firstMessage;

    [SerializeField] private PostRoundManager postRoundScript;


    // Start is called before the first frame update

    private void OnEnable() {
        AudioSettings.OnSaveLoaded += HandleSaveLoaded;
        // IMPORTANT: catch missed event
       // if(SaveSystem. != null)
       //     HandleSaveLoaded();
    }

    private void OnDisable() {
        AudioSettings.OnSaveLoaded -= HandleSaveLoaded;
    }

    private void HandleSaveLoaded() {
        Debug.Log("Save finished loading! " + saveSystem.level);
        
        if(saveSystem.level == -1) {
            DisplayPostRoundResults(false, 333, 2, 4, 13);
            //resultsText.gameObject.GetComponent<TextAdder>().endWord = "Mission: Failed";
            //resultsText.gameObject.SetActive(true);
        }
        else if(saveSystem.level == 1) {
            DisplayPostRoundResults(true, 333, 2, 4, 13);
            //resultsText.gameObject.GetComponent<TextAdder>().endWord = "Mission: Success";
            //resultsText.gameObject.SetActive(true);
        }
        else {
            // don't call display postround results at all, because we got to here (the hub) from somewhere other than a mission.
            //resultsText.gameObject.GetComponent<TextAdder>().endWord = "Welcome";
            //resultsText.gameObject.SetActive(true);
        }
        
        firstMessage.SetActive(saveSystem.hubFirstData);
    }

    public void DisplayPostRoundResults(bool success, int secondsSpent, int livesLeft, int timeSpotted, int longestChase) {
        postRoundScript.DisplayPostRoundResults(success, secondsSpent, livesLeft, timeSpotted, longestChase);
    }


}
