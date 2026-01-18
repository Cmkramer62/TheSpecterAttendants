using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiCrouch : MonoBehaviour {
    public bool forCabinet = false;
    private bool ready = true;
    private PlayerMovement movementScript;

    private void Start() {
        movementScript = GameObject.FindObjectOfType<PlayerMovement>();
    }

    private void OnTriggerEnter(Collider other) { // Bad way to do this
        if(other.gameObject.name.Equals("Player")) {
            if(ready) other.GetComponent<PlayerMovement>().amountCrouchSpots++;
            ready = false;
            if(!Input.GetKey(KeyCode.LeftControl) && !movementScript.isCrouched) movementScript.Crouch(); // Won't work for water now: Entering forces you to crouch. Add bool for water.
            movementScript.allowedToCrouch = false;
            //movementScript.isInVent = true;
            //movementScript.unreachable = true;
            //if(forCabinet) gameObject.GetComponentInParent<NewCabinetUse>().playerInside = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.name.Equals("Player")) {
            ready = true;
            other.GetComponent<PlayerMovement>().amountCrouchSpots--;
            if(movementScript.amountCrouchSpots == 0) {
                movementScript.allowedToCrouch = true;
                //movementScript.isInVent = false;
                //movementScript.unreachable = false;
                //if(forCabinet) gameObject.GetComponentInParent<NewCabinetUse>().playerInside = false;

                if(!Input.GetKey(KeyCode.LeftControl) && movementScript.isCrouched) {
                    movementScript.Crouch();
                }
            }
        }
    }

}
