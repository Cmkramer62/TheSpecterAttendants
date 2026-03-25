using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatorTrigger : MonoBehaviour {
    public ActivatorSwitch activatorScript;
    public LightFlicker lightScriptDirect;

    [SerializeField] private float distToPlayer = 10f;
    [SerializeField] private int chargesToAdd = 1;
    private Transform playerReference;
    
    private void Awake() {
        playerReference = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnTriggerEnter(Collider other) {

        if(other.name == "Ghost Enemy" && activatorScript != null && activatorScript.state && other.GetComponent<Enemy>().invisible) {
            activatorScript.Activate();
            if(CheckIfPlayerInRange() && !other.GetComponentInChildren<ParanormalNoises>().IsPlayingParanormal()) {
                other.GetComponent<Enemy>().IncreaseCharges();
                other.GetComponentInChildren<ParanormalNoises>().StartCooldown();
            }
        }
        else if(other.name == "Ghost Enemy" && lightScriptDirect != null && lightScriptDirect.alive && other.GetComponent<Enemy>().invisible) {
            
            if(CheckIfPlayerInRange() && !other.GetComponentInChildren<ParanormalNoises>().IsPlayingParanormal()) {
                other.GetComponent<Enemy>().IncreaseCharges();
                other.GetComponentInChildren<ParanormalNoises>().StartCooldown();
                if(other.GetComponent<Enemy>().invisSpeed >= 7) {
                    lightScriptDirect.BlowUpLight();
                }
                else {
                    lightScriptDirect.TurnOffLight(false);
                }
            }
            else {
                lightScriptDirect.TurnOffLight(false);
            }
        }

        
    }


    private bool CheckIfPlayerInRange() {
        return Vector3.Distance(playerReference.position, transform.position) < distToPlayer;
    }
}
