using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingSpot : MonoBehaviour {

    public Transform positionHide;
    public bool hidingHere = false, alteredSpot = false, scareOnExit = false;
    public AudioClip enterClip, exitClip;
    public GameObject hidingUI;

    public enum Variations { newObject, newColor, newRotation }
    public Variations currentVariation;
    public GameObject objectToAlter;
    public Material[] newColor;
    public Material ogColor;
    public MeshRenderer mesh;
    public Vector3 rotationAltered;
    public int materialIndex = 0;

    public bool tutorialHidingSpot = false;

    private GameObject player;
    private Vector3 initialPosVec;
    private Quaternion initRot;
    private Animator fadeAnimator;
    private int storedItem = 0;

    // Update is called once per frame
    void Update() {
        if(hidingHere && Input.GetKeyDown(KeyCode.E)) {
            Unhide();
        }
    }

    #region Hiding methods
    public void Hide(GameObject thisPlayer) {
        if(fadeAnimator == null) fadeAnimator = GameObject.Find("Fade Animation").GetComponent<Animator>();
        fadeAnimator.Play("Fade to Black");
        GetComponent<AudioSource>().PlayOneShot(enterClip);
        player = thisPlayer;
        player.GetComponent<PlayerMovement>().allowedToMove = false;
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.GetChild(1).GetChild(0).GetComponent<InteractRaycast>().allowedToRaycast = false;
        player.GetComponent<PlayerMovement>().isHiding = true;
        if(player.GetComponent<PlayerMovement>().enemyVisionScript.targetVisible) player.GetComponent<PlayerMovement>().enemyVisionScript.GetComponent<Enemy>().playerLastSeen = transform.position;
        storedItem = player.transform.parent.GetComponentInChildren<ToolController>().heldIndex;
        player.transform.parent.GetComponentInChildren<ToolController>().ForceToBarehand();
        StartCoroutine(HideTimer());
    }

    private IEnumerator HideTimer() {
        yield return new WaitForSeconds(.5f);
        hidingUI.SetActive(true);
        fadeAnimator.Play("Fade from Black");
        hidingHere = true;
        initialPosVec = player.transform.position;
        initRot = player.transform.rotation;
        player.transform.position = positionHide.position;
        player.transform.rotation = positionHide.rotation;
    }

    private void Unhide() {
        GetComponent<AudioSource>().PlayOneShot(exitClip);
        fadeAnimator.Play("Fade to Black");

        StartCoroutine(UnhideTimer());
    }

    private IEnumerator UnhideTimer() {
        yield return new WaitForSeconds(.5f);
        fadeAnimator.Play("Fade from Black");

        hidingUI.SetActive(false);
        hidingHere = false;
        player.transform.GetChild(1).GetChild(0).GetComponent<InteractRaycast>().allowedToRaycast = true;
        player.transform.position = initialPosVec;
        player.transform.rotation = initRot;
        player.GetComponent<CharacterController>().enabled = true;
        player.GetComponent<PlayerMovement>().allowedToMove = true;
        player.GetComponent<PlayerMovement>().isHiding = false;
        player.transform.parent.GetComponentInChildren<ToolController>().ForceToPrevhand(storedItem);

        if(tutorialHidingSpot) GameObject.Find("TutorialManager").GetComponent<Tutorial>().usedHidingSpot = true;

        if(scareOnExit) {
            gameObject.layer = 0;
            yield return new WaitForSeconds(.5f);
            GameObject.Find("Game Manager").GetComponent<Death>().Jumpscare();
        }
    }
    #endregion

    public void ScrambleSpot() {

    }

    public void AlterHidingSpot() {
        alteredSpot = true;
        
        switch(currentVariation) {
            case Variations.newObject:
                objectToAlter.SetActive(!objectToAlter.activeSelf);
                break;
            case Variations.newColor:
                mesh.material = newColor[NewColorApply()];
                //newColor = ogColor;
                //ogColor = mesh.material;
                break;
            case Variations.newRotation:
                gameObject.transform.Rotate(rotationAltered);
                rotationAltered = -rotationAltered;
                break;
        }

        /*
        int tem = Random.Range(0, 3);
        if(tem == 0) currentVariation = Variations.newObject;
        else if(tem == 1) currentVariation = Variations.newColor;
        else currentVariation = Variations.newRotation;
        */
    }

    public int NewColorApply() {
        int rand = Random.Range(0, newColor.Length);
        if(rand == materialIndex) {
            return NewColorApply();
        }
        else {
            materialIndex = rand;
            return rand;
        }
    }

    public void UndoAlteration() {
        if(alteredSpot) {
            alteredSpot = false;
            /*
            switch(currentVariation) {
                case Variations.newObject:
                    objectToAlter.SetActive(false);
                    break;
                case Variations.newColor:
                    mesh.material = ogColor;
                    break;
                case Variations.newRotation:
                    gameObject.transform.Rotate(-rotationAltered);
                    break;
            }
            */
        }
    }
}
