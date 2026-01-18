using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideAndSeekManager : MonoBehaviour {

    public int currentRound = 1, gracePeriodTime = 30, countdownTime = 30;
    public GlitchCountdown[] canvasCountdowns;
    public HidingSpot[] hidingSpots;
    public HidingSpot goodHidingSpot;
    private HidingSpot currentSpot;
    private bool inASpot = false;

    public AudioSource mainMusicSource, musicSFXSource;
    public float musicVolume = 0.1f, effectsVolume = 0.5f;
    public AudioClip graceStartsClip, countdownStartsClip, hidingMusicClip, finalHidingClip, searchingClip, textSFX;

    // Start is called before the first frame update
    void Start() {
        //StartRound();
        canvasCountdowns = GameObject.FindObjectsOfType<GlitchCountdown>();
    }

    private void SetWorldTexts(int count, bool red) {
        foreach(GlitchCountdown glitch in canvasCountdowns) {
            glitch.currentInt = count;
            if(red) glitch.text.color = Color.red;
            else glitch.text.color = Color.white;
            glitch.eyeIcon.SetActive(false);
        }
    }

    private void ActivateEyes(bool state) {
        foreach(GlitchCountdown glitch in canvasCountdowns) {
            glitch.eyeIcon.SetActive(state);
        }
    }

    private void ActivateGlitchCountdowns() {
        foreach(GlitchCountdown glitch in canvasCountdowns) {
            glitch.gameObject.SetActive(true);
        }
    }

    private void AlterHidingSpots() {
        int safeSpot = Random.Range(0, hidingSpots.Length);
        Debug.Log("safe: " + safeSpot);
        for(int i = 0; i < hidingSpots.Length; i++) {
            if(i != safeSpot) hidingSpots[i].AlterHidingSpot();
            else goodHidingSpot = hidingSpots[i];
        }
    }

    public void StartRound() {
        ActivateGlitchCountdowns();
        //Grace period. Player must find the hiding spots and memorize them.

        // countdown starts.
        // In tandem ^: hiding spots change

        // countdown ends, and player either dies or lives. 
        // Hiding spots reset
        // if round is total rounds, unlock exit. 
        // else increment current round, and after moment, repeat this method.
        StartCoroutine(RoundSequence());
    }

    private IEnumerator RoundSequence() {
        ActivateEyes(false);
        Debug.Log("Round Starting.");
        gameObject.GetComponent<CandleSystem>().CandleAppear();
        gameObject.GetComponent<TextPrompter>().QueueTextPrompt("Memorize.", textSFX);
        musicSFXSource.volume = effectsVolume;
        musicSFXSource.PlayOneShot(graceStartsClip);
        mainMusicSource.volume = musicVolume;
        mainMusicSource.Play();
        for(int i = gracePeriodTime; i >= 0; i--) {
            Debug.Log("Grace Period: " + i);
            SetWorldTexts(i, false);
            yield return new WaitForSeconds(1);
        }
        Debug.Log("Grace Time Over.");
        AlterHidingSpots();

        musicSFXSource.PlayOneShot(countdownStartsClip);
        mainMusicSource.volume = musicVolume;
        mainMusicSource.PlayOneShot(hidingMusicClip);
        gameObject.GetComponent<TextPrompter>().QueueTextPrompt("Hide.", textSFX);
        for(int i = countdownTime; i >= 0; i--) {
            Debug.Log("Countdown: " + i);
            SetWorldTexts(i, true);
            yield return new WaitForSeconds(1);
            if(i == 5) {
                bool initialInSpot = false;
                foreach(HidingSpot spot in hidingSpots) {
                    if(spot.hidingHere == true) {
                        initialInSpot = true;
                    }
                }
                if(!initialInSpot) {
                    mainMusicSource.PlayOneShot(finalHidingClip, musicVolume);
                    AudioController.FadeOutAudio(this, mainMusicSource, 5);
                }
                AudioController.FadeOutAudio(this, mainMusicSource, 5);
                AudioController.FadeOutAudio(this, musicSFXSource, 5);
            }
        }
        mainMusicSource.PlayOneShot(searchingClip);
        AudioController.FadeInAudio(this, mainMusicSource, 3, musicVolume);

        // #1.
        GameObject.Find("Monster").GetComponent<MonsterController>().EnterSearching();
        gameObject.GetComponent<TextPrompter>().QueueTextPrompt("Wait.", textSFX);
        ActivateEyes(true);

        inASpot = false;
        currentSpot = null;
        foreach(HidingSpot spot in hidingSpots) {
            if(spot.hidingHere == true) {
                inASpot = true;
                currentSpot = spot;
            }
             
        }

        // #2.
        if(!inASpot) {
            GetComponent<Death>().Jumpscare();
        }
        // #3. 
        else {
            foreach(HidingSpot spot in hidingSpots) {
                spot.scareOnExit = true;
            }
        }

    }

    public void ResetHidingSpots() {
        bool ready = false;
        foreach(HidingSpot spot in hidingSpots) {
            if(spot.scareOnExit) ready = true;
        }

        if(ready) {
            foreach(HidingSpot spot in hidingSpots) {
                spot.scareOnExit = false;
                spot.UndoAlteration();
            }
            if(GetComponent<CandleSystem>().candleCurrentCount == 5) {
                Debug.Log("Level Complete.");
                // unlock exit
                GetComponent<CandleSystem>().exitDoor.OpenCloseDoor();
            }
            else {
                currentRound++;
                foreach(HidingSpot spot in hidingSpots) {
                    spot.UndoAlteration();
                }
                StartCoroutine(RoundSequence());
            }
        }
        
    }

    public void FinishRoundSequence() {
        // #5. Called by monster
        // #6.
        goodHidingSpot.scareOnExit = false;
        AudioController.FadeOutAudio(this, mainMusicSource, 5f);
        gameObject.GetComponent<TextPrompter>().QueueTextPrompt("Repeat.", textSFX);
    }

}
