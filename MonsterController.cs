using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour {

    public enum Mode { Idle, Searching, Hunting }
    public Mode currentMode;
    private NavMeshAgent agent;

    public List<Transform> targets;
    public int currentTargetIndex = 0;
    public Animator animator;

    public ParticleSystem smokeParticle;
    public SkinnedMeshRenderer[] bodyMeshes;
    public GameObject deathTrigger;

    public AudioSource source;
    //public AudioClip footstepsClip;

    void OnEnable() {
        if(agent == null) {
            agent = GetComponent<NavMeshAgent>();
            //StartCoroutine(AlternatingAnim());
            //currentMode = Mode.Searching;
            if(currentMode == Mode.Hunting) StartHunting(); // can also call this secondarily, if agent does not start this way.
        }
    }

    // Update is called once per frame
    void Update() {
        if(currentMode == Mode.Searching && !agent.pathPending && agent.remainingDistance < agent.stoppingDistance) {
            if(!agent.hasPath || agent.velocity.sqrMagnitude == 0f) {
                // Agent has reached the current target
                currentTargetIndex++;
                if(currentTargetIndex < targets.Count) {
                    SetNextDestination();
                }
                else {
                    // All targets reached
                    Debug.Log("All targets reached!");
                    currentMode = Mode.Idle;
                    currentTargetIndex = 0;
                    GameObject.Find("Game Manager").GetComponent<HideAndSeekManager>().FinishRoundSequence();
                    StartCoroutine(BodyHide(false));
                    // Optional: Disable agent, loop, or trigger an event
                }
            }
        }
        // NOTE: If you want the monster to prioritize hunting when both are true, make this the first IF statement.
        else if(currentMode == Mode.Hunting) { //&& !agent.pathPending && agent.remainingDistance < agent.stoppingDistance) {
            agent.SetDestination(GameObject.Find("Player").transform.position);
        }
        animator.SetFloat("Speed", agent.velocity.sqrMagnitude);

        if(agent.velocity.sqrMagnitude >= 5f && !source.isPlaying) {
            source.Play();
        }
        else if(agent.velocity.sqrMagnitude < .5f && source.isPlaying) {
            source.Stop();
        }
    }

    private IEnumerator BodyHide(bool state) {
        smokeParticle.Play();
        yield return new WaitForSeconds(.5f);
        foreach(SkinnedMeshRenderer mesh in bodyMeshes) {
            mesh.enabled = state;
        }
    }

    private void SetNextDestination() {
        if(targets[currentTargetIndex] != null) {
            agent.SetDestination(targets[currentTargetIndex].position);
        }
        else {
            Debug.LogWarning("Target at index " + currentTargetIndex + " is null.");
            // Handle null target (e.g., skip to next, stop)
        }
    }

    public void StartHunting() {
        
        StartCoroutine(BodyHide(true));
        gameObject.GetComponent<DeathTrigger>().touchTrigger = true;
    }

    public void EnterSearching() {
        StartCoroutine(BodyHide(true));
        Debug.Log("Searching");
        currentMode = Mode.Searching;
        SetNextDestination();
    }

    public IEnumerator AlternatingAnim() {
        yield return new WaitForSeconds(Random.Range(0f, .1f));
        animator.speed = 0;

        foreach(SkinnedMeshRenderer mesh in bodyMeshes) {
            mesh.enabled = false;
        }
        //agent.speed = 0;

        yield return new WaitForSeconds(Random.Range(0f, .1f));
        animator.speed = 1;
        foreach(SkinnedMeshRenderer mesh in bodyMeshes) {
            mesh.enabled = true;
        }
        //agent.speed = 12;
        StartCoroutine(AlternatingAnim());
    }


}
