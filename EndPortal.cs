using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EndPortal : MonoBehaviour {

    public PurificationManager purificationScript;
    public Death deathScript;
    public SceneLoader sceneLoaderScene;
    public bool activated;

    public AudioSource source, ghostSource;
    public AudioClip enterClip, enterWrongClip, leaveClip, correctClip, ghostDeathClip;

    public Animator portalAnimation;
    public Enemy ghostScript;

    private void OnTriggerEnter(Collider other) {
        if(activated && other.name == "Player") {
            GetComponent<BoxCollider>().enabled = false;
            AudioController.FadeOutAudio(this, purificationScript.cursedObjectScript.pSourceA, 1f);
            AudioController.FadeOutAudio(this, purificationScript.cursedObjectScript.pSourceB, 1f);

            StartCoroutine(EndGoalTimer());
        }
        else if(other.name == "Player") {
            source.PlayOneShot(enterClip);
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.name == "Player") {
            GetComponent<InteractPrompt>().InteractWithObject();
            source.PlayOneShot(leaveClip);
        }   
    }

    private IEnumerator EndGoalTimer() {
        source.PlayOneShot(purificationScript.potentialCursedItem.name == "Goal Curse" ? enterClip : enterWrongClip);

        yield return new WaitForSeconds(1f);
       


        if(purificationScript.potentialCursedItem.name == "Goal Curse") {
            portalAnimation.Play("WipeAwayAnim");
            source.PlayOneShot(correctClip);
            yield return new WaitForSeconds(.5f);
            ghostScript.allowedToMove = false;
            for(int i = 0; i < ghostScript.gameObject.transform.childCount; i++) {
                if(ghostScript.transform.GetChild(i).gameObject.activeSelf) {
                    Debug.Log(ghostScript.transform.GetChild(i).name);
                    ghostScript.transform.GetChild(i).GetComponent<Animator>().SetTrigger("Death");
                    ghostScript.GetComponent<NavMeshAgent>().speed = 0;
                    ghostSource.PlayOneShot(ghostDeathClip); 
                    break;
                }
            }
            //ghostScript.GetComponent<Animator>().SetTrigger("Death");
            
            yield return new WaitForSeconds(7f);
            sceneLoaderScene.LoadScene(3);
        }
        else {
            deathScript.Jumpscare();
        }
    }
}
