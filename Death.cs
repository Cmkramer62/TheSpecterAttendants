using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class Death : MonoBehaviour {

    public GameObject player, jumpscareObject, cameraParent, realGhost, jumpscareChildObject, deathUI;
    public AudioSource source;
    public AudioClip jumpscareClip, hitDamageClip;
    public AudioClip[] stingerClips;
    public float scareVolume = 1.0f;

    public int lives = 3;
    public GameObject[] bloodUI, heartsUI;
    public bool allowDeath = true;

    [HideInInspector]
    public GameObject realGhostChild;

    public AudioSettings saveSystem;
    public AudioMixer masterMixer;

    public void Jumpscare() {
        saveSystem.SetLevel(-1);
        StartCoroutine(JumpscareTimer());
    }

    private IEnumerator JumpscareTimer() {
        masterMixer.SetFloat("MainVolumeParam", -80);

        realGhost.SetActive(false);
        realGhostChild.SetActive(false);
        realGhostChild.transform.parent = jumpscareObject.transform;
        realGhostChild.transform.position = jumpscareChildObject.transform.position;
        realGhostChild.transform.rotation = jumpscareChildObject.transform.rotation;
        realGhostChild.transform.localScale = new Vector3(1.7f, 1.7f, 1.7f);
        realGhostChild.GetComponent<Animator>().runtimeAnimatorController = jumpscareChildObject.GetComponent<Animator>().runtimeAnimatorController;
        realGhost.GetComponent<Enemy>().MakeVisible();
        realGhostChild.SetActive(true);
        jumpscareChildObject.SetActive(false);
        player.SetActive(false);
        jumpscareObject.SetActive(true);
        realGhostChild.transform.GetChild(1).GetComponent<Animator>().Play("JumpscareFaceAnimator");
        source.PlayOneShot(jumpscareClip, scareVolume);
        Cursor.lockState = CursorLockMode.None;
        GetComponent<PauseGame>().normalUI.SetActive(false);
        GetComponent<PauseGame>().pausedUI.SetActive(false);
        GetComponent<GameTimer>().KillTimer();
        GetComponent<PurificationManager>().KillTimer();
        GetComponent<ToolController>().masterAllowed = false;
        GetComponent<PauseGame>().allowedToPause = false;
        //wait for 1 (?) seconds, then pause the game. Load a menu that's animated without using timescale. What to do about the pause menu functionality?
        yield return new WaitForSeconds(1.13333f);
        realGhostChild.GetComponent<Animator>().speed = 0;
        
        AudioController.FadeOutAudio(this, GetComponent<PurificationManager>().cursedObjectScript.pSourceB, .5f);
        yield return new WaitForSeconds(1f);
        deathUI.SetActive(true);

        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoseLife() {
        StartCoroutine(BloodTimer());
    }

    private IEnumerator BloodTimer() {
        source.PlayOneShot(hitDamageClip);

        if(lives - 1 == 0) {
            // player is dead.
            bloodUI[3 - lives].SetActive(true);
            heartsUI[lives - 1].GetComponent<Animator>().Play("HeartIconLoss");
            source.PlayOneShot(stingerClips[3 - lives]);
            if(allowDeath) Jumpscare();
            else {
                yield return new WaitForSeconds(1f);
                bloodUI[3 - lives].SetActive(false);
                heartsUI[lives - 1].transform.GetChild(1).gameObject.SetActive(false);
            }
        }
        else {
            bloodUI[3 - lives].SetActive(true);
            heartsUI[lives - 1].GetComponent<Animator>().Play("HeartIconLoss");
            source.PlayOneShot(stingerClips[3 - lives]);
            yield return new WaitForSeconds(1f);
            bloodUI[3 - lives].SetActive(false);
            heartsUI[lives - 1].transform.GetChild(1).gameObject.SetActive(false);
            lives--;

        }
    }

}
