using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideNSeekTrigger : MonoBehaviour {
    public bool touchTrigger = false;

    private void OnTriggerEnter(Collider other) {
        if(other.name == "Player" && touchTrigger) {
            other.gameObject.transform.parent.GetComponentInChildren<HideAndSeekManager>().StartRound();
            GameObject.Destroy(gameObject);
        }
    }
}
