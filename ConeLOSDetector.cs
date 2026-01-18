using UnityEngine;

public class ConeLOSDetector : MonoBehaviour {

    private Transform thisGameObject; // Reference to the player GameObject
    public Transform target; // Reference to the monster GameObject
    public float fieldOfViewAngle = 90f; // The field of view angle of the monster
    public int viewDistance = 20; // How far away the player can be without being seen.

    public bool targetVisible = false, isMonster, visibilityOverride = false; // Flag to indicate if the player is in sight

    public LayerMask ignoreMeLayer;

    private bool wasHunting = false;

    public bool inViewDist = false, playerFieldView = false, playerLOS = false;

    private void OnEnable() {
        thisGameObject = gameObject.transform;
    }


    /*
     * Update() checks once a frame if the player meets all the requirements to be considered within line of sight.
     */
    private void Update() {
        inViewDist = IsPlayerInViewDistance();
        playerFieldView = IsPlayerInFieldOfView();
        playerLOS = IsPlayerInLineOfSight(thisGameObject);

        if(visibilityOverride || (inViewDist && playerFieldView && playerLOS)){//&& ((isMonster && target.gameObject.GetComponent<Flashlight>().flashlightState) || !isMonster)) {
            targetVisible = true;
        }
        else { //if(!inViewDist || !playerFieldView || !playerLOS){ // || (isMonster && !target.gameObject.GetComponent<Flashlight>().flashlightState)) {
            targetVisible = false;
        }
    }


    /*
     * IsPlayerInFieldOfView() uses the angle of the Vector3 to the player to see if he is within the field of view angle.
     * Returns true if this is the case.
     */
    private bool IsPlayerInFieldOfView() {
        Vector3 directionToPlayer = thisGameObject.transform.position - target.position;
        float angle = Vector3.Angle(thisGameObject.transform.forward, directionToPlayer);

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

        Vector3 directionToPlayer = target.transform.position - gameObject.transform.position;

        if(Physics.Raycast(transform.position, directionToPlayer, out hit, Mathf.Infinity, ~ignoreMeLayer)) {
            if(hit.transform.name.Equals(target.name)) {
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
        if(Vector3.Distance(target.position, thisGameObject.position) < viewDistance) return true;
        else return false;
    }


}
