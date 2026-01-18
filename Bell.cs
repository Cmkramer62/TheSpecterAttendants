using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bell : MonoBehaviour {

    public AudioSource source;
    public AudioClip ringClip;

    public bool bellOnCooldown = false;

    public Animator bellAnimator;

    private Enemy ghostScript;
    public bool ghostSearchWithSound = false;
    public int ghostSoundOdds = 3;

    // Start is called before the first frame update
    void Start() {
        ghostScript = GameObject.Find("Ghost Enemy").GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update() {
        if(!bellOnCooldown && Input.GetKeyUp(KeyCode.F)) {

            bellAnimator.Play("BellRing");
            source.pitch = Random.Range(.95f, 1.1f);
            source.PlayOneShot(ringClip);
            StartCoroutine(BellCooldownTimer());
            TriggerCurse(true); // state here is not used.
            if(ghostSearchWithSound) ghostScript.walkPoint = gameObject.transform.parent.parent.parent.transform.GetChild(1).transform.position;
        }
    }

    private IEnumerator BellCooldownTimer() {
        bellOnCooldown = true;
        yield return new WaitForSeconds(2f);
        bellOnCooldown = false;
    }

    private void OnDisable() {
        bellOnCooldown = false;
    }

    private void TriggerCurse(bool state) {
        foreach(CursedObject objectee in gameObject.transform.parent.parent.parent.GetComponentInChildren<ToolController>().objectsList) {
            objectee.DisplayCurse(CursedObject.CursedTypes.Sound, state);
        }
    }
}
