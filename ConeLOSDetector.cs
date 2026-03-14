using UnityEngine;

public class ConeLOSDetector : MonoBehaviour {

    private Transform thisTransform; // Reference to the player GameObject
    public Transform targetTransform; // Reference to the monster GameObject
    public float fieldOfViewAngle = 90f; // The field of view angle of the monster
    public int viewDistance = 20; // How far away the player can be without being seen.

    public bool targetVisible = false, visibilityOverride = false; // Flag to indicate if the player is in sight
    public LayerMask ignoreMeLayer;

    public bool inViewDist = false, inFieldOfView = false, inLineOfSight = false;

    private void OnEnable() {
        thisTransform = gameObject.transform;
    }


    /*
     * Update() checks once a frame if the player meets all the requirements to be considered within line of sight.
     */
    private void Update() {
        inViewDist = IsPlayerInViewDistance();
        inFieldOfView = IsPlayerInFieldOfView();
        inLineOfSight = IsPlayerInLineOfSight(thisTransform);

        if(visibilityOverride || (inViewDist && inFieldOfView && inLineOfSight)){
            targetVisible = true;
        }
        else {
            targetVisible = false;
        }
    }


    /*
     * IsPlayerInFieldOfView() uses the angle of the Vector3 to the player to see if he is within the field of view angle.
     * Returns true if this is the case.
     */
    private bool IsPlayerInFieldOfView() {
        Vector3 directionToPlayer = thisTransform.position - targetTransform.position;
        float angle = Vector3.Angle(thisTransform.forward, directionToPlayer);

        if(angle >= (360 - fieldOfViewAngle) * 0.5f) {
            return true;
        }

        return false;
    }

    /*
     * IsPlayerInLineOfSight() checks whether the RayCast beam touches something else before it reaches the player.
     * If it touches the player first, then the line of sight is clear, with no obstructions, and returns true.
     */
    private bool IsPlayerInLineOfSight(Transform targetFOV) {
        RaycastHit hit;

        Vector3 directionToPlayer = targetTransform.position - thisTransform.position;

        if(Physics.Raycast(thisTransform.position, directionToPlayer, out hit, Mathf.Infinity, ~ignoreMeLayer)) {
            if(hit.transform.name.Equals(targetTransform.name)) {
                return true;
            }
        }
        return false;
    }

    /*
     * IsPlayerInViewDistance() checks if the player is within a certain range of the monster.
     * Returns true if this is the case.
     */
    private bool IsPlayerInViewDistance() {
        if(Vector3.Distance(targetTransform.position, thisTransform.position) < viewDistance) return true;
        else return false;
    }


}
