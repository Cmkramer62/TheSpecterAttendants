using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candle : MonoBehaviour {

    public GameObject candleEffects;
    public ParticleSystem candlefireParticle, smokeParticle;
    public bool candleState = false;

    private CandleSystem candleSystem;

    private void OnEnable() {
        candleSystem = GameObject.Find("Game Manager").GetComponent<CandleSystem>();
    }

    public void InteractWithCandle() {
        if(!candleState) {
            candleEffects.SetActive(true);
            candlefireParticle.Play();
            candleState = true;
            gameObject.layer = 0;
            candleSystem.CandleActivate();
        }
    }

}
