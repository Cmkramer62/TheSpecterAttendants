using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleSystem : MonoBehaviour {

    public int candleCurrentCount = 0, candleGoalIndex = 5;
    public GameObject[] exitEyes, candles;
    public Door exitDoor;
    public bool nonHideAndSeekDoor = false;
    public AudioSource source;
    public AudioClip obtainClip, finalClip;
    public float volume = .2f;

    public void CandleActivate() {
        exitEyes[candleCurrentCount].SetActive(true);
        if(candleCurrentCount == 4 && nonHideAndSeekDoor) {
            exitDoor.OpenCloseDoor();
            source.PlayOneShot(finalClip, volume);
        }
        else {
            source.PlayOneShot(obtainClip, volume);
        }
        candleCurrentCount++;
    }

    public void CandleAppear() {
        if(candleCurrentCount < 5) {
            List<GameObject> candleList = new List<GameObject>();
            foreach(GameObject candle in candles) {
                if(!candle.activeSelf) {
                    candleList.Add(candle);
                }
            }
            candleList[Random.Range(0, candleList.Count)].SetActive(true);
        }
    }

}
