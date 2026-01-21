using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour {

    public GameObject player, jumpscareObject, scareCamera, cameraParent, realGhost;
    public AudioSource source;
    public AudioClip jumpscareClip, hitDamageClip;
    public AudioClip[] stingerClips;
    public float scareVolume = 1.0f;

    public int lives = 3;
    public GameObject[] bloodUI, heartsUI;
    public bool allowDeath = true;

    public void Jumpscare() {
        StartCoroutine(JumpscareTimer());
    }

    private IEnumerator JumpscareTimer() {
        //lerp player towards
        player.SetActive(false);
        realGhost.SetActive(false);
        jumpscareObject.SetActive(true);
        source.PlayOneShot(jumpscareClip, scareVolume);
        //yield return new WaitForSeconds(.166666f);
        //scareCamera.transform.position = cameraParent.transform.position;
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
