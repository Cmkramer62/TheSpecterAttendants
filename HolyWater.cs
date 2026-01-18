using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyWater : MonoBehaviour {

    public ParticleSystem steamParticle;

    public void TurnSteam(bool state) {
        if(steamParticle.gameObject.activeSelf) {
            if(state) steamParticle.Play();
            else steamParticle.Stop();
        }
        
    }

}
