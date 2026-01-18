using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    public void SetClip(AudioClip newClip) {
        clip = newClip;
    }

    //Loads a scene based on index number
    public void LoadScene(int sceneNumber) {
        loadingScreen.SetActive(true);
        inventoryScript.SaveData();
        PlayerPrefs.SetInt("levelNumber", sceneNumber);
        FadeOutAllSources();
        StartCoroutine(LoadAsynchronously(sceneNumber));
    }

    /*
     * Loads a scene, but does not save the location we are going to.
     * Used in Main Menu and Credits scenes.
     * We don't want to save progress until they make it to a new level.
     */
    public void LoadSceneWithoutSavingNext(int sceneNumber) {
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

        while(!operation.isDone) {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;
            yield return null;
        }
    }

    public void FadeOutAllSources() {
        foreach(AudioSource source in sourcesToFade) {
            AudioController.FadeOutAudio(this, source, 1f);
        }
    }

}
