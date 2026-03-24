using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrigger : MonoBehaviour {

    public bool touchTrigger = false, angelDeath = false;
    public Death deathScript;

    [SerializeField]
    private bool triggered = false;

    private void OnTriggerEnter(Collider other) {
        if(other.name == "Player" && touchTrigger && !triggered) {
            deathScript.Jumpscare(angelDeath);
            triggered = true;
        }    
    }

}
