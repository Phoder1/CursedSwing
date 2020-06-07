using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {

    enum EnemyStates { Patroling, Following, Attacking, Stunned, Death, None }
    //Rb Hit Force calculations mostly (go to onCollision methods for implementation)
    Rigidbody rb;
    float collisionForce;
    const float forceAmount = 50f;
    float impulseForce;
    [SerializeField]
    float maxImpulseForce = 1.5f;
    bool moveForward;


    //State Machine
    EnemyStates currentState = EnemyStates.Patroling;
    EnemyStates lastState = EnemyStates.None;
    

    //Patrol variables
    [SerializeField]
    private Transform[] PatrolPositions;

    private int patrolIndex;

    //Follow variables
    [SerializeField]
    float FOLLOW_UPDATE_INTERVAL = 0.7f;
    [SerializeField]
    float PLAYER_DETECTION_DISTANCE = 15f;

    float timerSinceFollow;

    //Attacking variables
    const float PLAYER_ATTACK_RANGE = 3f;



    private float distanceFromPlayer;

    private bool canAttack;
    private bool isAlive;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float DetectionDiatance;
    //AI stuff
    private NavMeshAgent agent;
    private Transform playerPos;
    private Vector3 nextPos;
    //probabaly awake cuz its gonna be instantiated in the near future, most def
    private void Awake() {
        patrolIndex = 0;
        canAttack = true;
        isAlive = true;
        agent = GetComponent<NavMeshAgent>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
    }

    private void Update() {
        distanceFromPlayer = Vector3.Distance(transform.position, playerPos.position);
        StateMachine();
    }


    private void StateMachine() {
        //Global case end check
        if (distanceFromPlayer <= PLAYER_ATTACK_RANGE) {
            currentState = EnemyStates.Attacking;
        }
        else if (distanceFromPlayer <= PLAYER_DETECTION_DISTANCE) {
            currentState = EnemyStates.Following;
        }
        else {
            currentState = EnemyStates.Patroling;
        }


        if (lastState != currentState) {
            Debug.Log("Switched state! to: " + currentState.ToString() + " ,from: " + lastState.ToString());
                lastState = currentState;
            switch (currentState) {
                case EnemyStates.Patroling:
                    //Enter state action




                    break;
                case EnemyStates.Following:
                    //Enter state action
                    FollowTarget();
                    timerSinceFollow = Time.timeSinceLevelLoad;



                    break;
                case EnemyStates.Attacking:
                    //Enter state action




                    break;
                case EnemyStates.Stunned:
                    //Enter state action




                    break;
            }
        }





        switch (currentState) {
            case EnemyStates.Patroling:
                //Case Update
                Patrol();


                //Case end condition checking


                break;
            case EnemyStates.Following:
                //Case Update
                if (Time.timeSinceLevelLoad - timerSinceFollow >= FOLLOW_UPDATE_INTERVAL) {
                    FollowTarget();
                    timerSinceFollow = Time.timeSinceLevelLoad;
                }


                //Case end condition checking


                break;
            case EnemyStates.Attacking:
                //Case Update
                Attacking();


                //Case end condition checking


                break;
            case EnemyStates.Stunned:
                //Case Update



                //Case end condition checking


                break;
        }

        Debug.Log("State: " + currentState.ToString());
    }


    //When Hit with anything, instances are set in collision matrix
    private void OnTriggerEnter(Collider collision) {
        if (collision.transform.CompareTag("Player")) {
            Stunned();
        }
        collisionForce = PlayerController.rotationAngle;
        impulseForce = Mathf.Clamp(Mathf.Abs(collisionForce) * Time.deltaTime * forceAmount, 0f, maxImpulseForce);
        rb.AddForce((transform.position - collision.transform.position) * impulseForce, ForceMode.Impulse);
        //Debug.Log("NITROOOOOO : " + (transform.position - collision.transform.position));
    }
    //Agent Target destination
    private void GoHere(Vector3 target) {
        agent.SetDestination(target);
    }
    //Amir's shitty ass patrol loop
    public void Patrol() {
        nextPos = PatrolPositions[patrolIndex].position;
        if (Vector3.Distance(transform.position, PatrolPositions[patrolIndex].position) < 5f) {
            patrolIndex++;
        }
        if (patrolIndex > PatrolPositions.Length - 1) {
            patrolIndex = 0;
        }
        GoHere(nextPos);
    }
    public void FollowTarget() {
        bool hitNavMesh;
        Vector3 target;
        RaycastHit hit;
        hitNavMesh = Physics.Raycast(playerPos.position, Vector3.down, out hit, 20f, NavMesh.AllAreas);
        target = hit.point;
        GoHere(target);
    }
    public void Attacking() {

    }
    public void Stunned() {

    }
    public void Death() {

    }


    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, PLAYER_DETECTION_DISTANCE);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, PLAYER_ATTACK_RANGE);
    }
}
