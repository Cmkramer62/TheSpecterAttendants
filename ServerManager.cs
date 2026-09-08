using Unity.Netcode;
using UnityEngine;
using System;

public class ServerManager : NetworkBehaviour {
    [SerializeField] private GameObject timerPrefab, curseGamePrefab;
    public bool spawnGhost = true;
    public int timerAmount = 600;

    public override void OnNetworkSpawn() {
        if(!IsServer) return;

        StartGame();
    }

    void StartGame() {
        var timer = Instantiate(timerPrefab);
        timer.GetComponent<NetworkObject>().Spawn();
        timer.GetComponent<GameTimer>().timeLeft.Value = timerAmount;

        var cursegame = Instantiate(curseGamePrefab);
        cursegame.GetComponent<CurseGameManager>().spawnGhost = spawnGhost;
        cursegame.GetComponent<NetworkObject>().Spawn();
    }

}
