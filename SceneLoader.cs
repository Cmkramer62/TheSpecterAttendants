using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

/*Summary
 * This script simply loads a new scene. It 
 * turns on the loading screen while it does so.
 */
public class SceneLoader : MonoBehaviour {

    public GameObject loadingScreen;
    public Slider slider;
    private AudioClip clip;
    public int levelNumber = -1;
    public Inventory inventoryScript;
    public AudioSource[] sourcesToFade;
    public AudioSettings saveSystem;
    public AudioMixer masterMixer;

    public void SetClip(AudioClip newClip) {
        clip = newClip;
    }

    //Loads a scene based on index number
    public void LoadScene(int sceneNumber) {
        //if(sceneNumber == 3) saveSystem.SetLevel(0);
        loadingScreen.SetActive(true);
        inventoryScript.SaveData();
        PlayerPrefs.SetInt("levelNumber", sceneNumber);
        FadeOutAllSources();
        StartCoroutine(LoadAsynchronously(sceneNumber));
    }

    /*
     * Loads a scene, but does not save the location we are going to.
     * Used in Main Menu, pause menu, and Credits scenes.
     * We don't want to save progress until they make it to a new level.
     */
    public void LoadSceneWithoutSavingNext(int sceneNumber) {
        if(sceneNumber == 3) saveSystem.SetLevel(0);
        loadingScreen.SetActive(true);
        FadeOutAllSources();
        StartCoroutine(LoadAsynchronously(sceneNumber));
    }

    /*
     * Exits Application without saving anything.
     * The user's data will only contain what they had when they first
     *     entered their current level.
     */
    public void ExitGame() {
        saveSystem.SetLevel(0);
        FadeOutAllSources();
        StartCoroutine(ExitGameTimer());
    }

    private IEnumerator ExitGameTimer() {
        loadingScreen.SetActive(true);
        yield return new WaitForSeconds(1f);
        Application.Quit();
    }

    private IEnumerator LoadAsynchronously(int sceneIndex) {
        yield return new WaitForSeconds(1f);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        //operation.allowSceneActivation = false;

        while(!operation.isDone) {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;
            yield return null;
        }
        // Force bar to full
        //slider.value = 1f;

        // Optional: small delay so player actually sees 100%
        //yield return new WaitForSeconds(0.3f);

       // operation.allowSceneActivation = true;
        masterMixer.SetFloat("MainVolumeParam", 0);
    }

    public void FadeOutAllSources() {
        foreach(AudioSource source in sourcesToFade) {
            AudioController.FadeOutAudio(this, source, 1f);
        }
    }

}
