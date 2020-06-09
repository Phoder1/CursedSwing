using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using UnityEngine.UI;
using System.Threading;

public class EnemyController : MonoBehaviour
{

    enum EnemyStates { Patroling, Following, Attacking, Stunned, Death, None }
    private float HP = 100f;
    private Slider sliderRef;
    //Rb Hit Force calculations mostly (go to onCollision methods for implementation)
    Rigidbody rb;
    float collisionForce;
    const float forceAmount = 50f;
    float impulseForce;
    [SerializeField]
    float maxImpulseForce = 1.5f;
    bool moveForward;

    public Transform GFXHeadTrans;


    //State Machine
    EnemyStates currentState = EnemyStates.Patroling;
    EnemyStates lastState = EnemyStates.None;


    //Patrol variables
    [SerializeField]
    private Transform[] PatrolPositions;

    private int patrolIndex;
    private float timeSincePatrolled;
    private float PatrolUpdateInterval;
    private bool arrivedAtLocation;

    //Follow variables
    [SerializeField]
    float FollowUpdateInterval = 0.7f;
    [SerializeField]
    float playerDetectionDistance = 20f;
    float DetectionDisIncris = 20f;
    float timerSinceFollow;

    //Attacking variables
    const float PLAYER_ATTACK_RANGE = 3f;

    //Stunned variables
    float StunUpdateInterval = 5f;
    float timerSinceStunned;
    private int numberOfContactPoints;

    private float distanceFromPlayer;

    private bool canAttack;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float DetectionDiatance;
    //AI stuff
    private NavMeshAgent agent;
    private Transform playerPos;
    private Vector3 nextPos;

    [SerializeField]
    Collider swordCollider;
    //probabaly awake cuz its gonna be instantiated in the near future, most def
    private void Awake()
    {
        patrolIndex = 0;
        canAttack = true;
        agent = GetComponent<NavMeshAgent>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        //swordCollider = GameObject.FindGameObjectWithTag("Weapon").GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        sliderRef = transform.parent.GetComponentInChildren<Slider>();
        sliderRef.value = HP / 100;

    }

    private void Update()
    {
        distanceFromPlayer = Vector3.Distance(transform.position, playerPos.position);
        StateMachine();
    }


    private void StateMachine()
    {
        //Global case end check
        if (HP <= 0)
        {
            BoxCollider boxCollider = GFXHeadTrans.GetComponent<BoxCollider>();
            //started from the buttom now we here (enemyGFX => enemyGuy)
            Destroy(this.transform.parent.gameObject);
            return;
        }
        
        if (lastState != currentState)
        {
            Debug.Log("Switched state! to: " + currentState.ToString() + " ,from: " + lastState.ToString());
            lastState = currentState;
            switch (currentState)
            {
                case EnemyStates.Patroling:
                    //Enter state action
                    timeSincePatrolled = Time.timeSinceLevelLoad;



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
                    Stunned();

                    timerSinceStunned = Time.timeSinceLevelLoad;


                    break;
            }
        }



        switch (currentState)
        {
            case EnemyStates.Patroling:
                //Case Update
                if (arrivedAtLocation)
                {
                    timerSinceFollow = Time.timeSinceLevelLoad;
                }
                if (Time.timeSinceLevelLoad - timeSincePatrolled >= PatrolUpdateInterval)
                {
                    arrivedAtLocation = false;
                    Patrol();
                }
                //Case end condition checking
                if (distanceFromPlayer <= playerDetectionDistance)
                {
                    currentState = EnemyStates.Following;
                }
                break;
            case EnemyStates.Following:
                //Case Update
                if (Time.timeSinceLevelLoad - timerSinceFollow >= FollowUpdateInterval)
                {
                    FollowTarget();
                    timerSinceFollow = Time.timeSinceLevelLoad;
                }
                //Case end condition checking
                if (distanceFromPlayer > DetectionDiatance + DetectionDisIncris)
                {
                    currentState = EnemyStates.Patroling;
                }
                break;
            case EnemyStates.Attacking:
                //Case Update
                Attacking();
                //Case end condition checking
                currentState = EnemyStates.Following;

                break;
            case EnemyStates.Stunned:
                //Case Update

                //Case end condition checking

                if (Time.timeSinceLevelLoad - timerSinceStunned >= StunUpdateInterval)
                {
                    rb.mass = 1;
                    rb.isKinematic = true;
                    //transform.position = new Vector3(transform.position.x, 0.5905833f, transform.position.z);
                    transform.DORotate(new Vector3(0f, 0f, 0f), 1f).OnComplete(EnablePhysics);
                    

                }
                break;
        }

        //Debug.Log("State: " + currentState.ToString());
    }
    void EnablePhysics() {
        agent.enabled = true;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        currentState = EnemyStates.Patroling;
    }

    //collisionForce = PlayerController.rotationAngle;
    //impulseForce = Mathf.Clamp(Mathf.Abs(collisionForce) * Time.deltaTime * forceAmount, 0f, maxImpulseForce);
    //rb.AddForce((transform.position - collision.transform.position) * impulseForce, ForceMode.Impulse);

    //When Hit with anything, instances are set in collision matrix
    private void OnCollisionEnter(Collision collision)
    {
        numberOfContactPoints = collision.contactCount;
        ContactPoint[] contacts = new ContactPoint[collision.contactCount];
        collision.GetContacts(contacts);
        for (int i = 0; i < collision.contactCount; i++) {
            if (contacts[i].otherCollider.CompareTag("Weapon")) {
                currentState = EnemyStates.Stunned;
                rb.isKinematic = false;
                agent.enabled = false;
                rb.constraints = RigidbodyConstraints.None;
                impulseForce = Mathf.Clamp(Mathf.Abs(PlayerController.rotationAngle) * forceAmount, 0f, maxImpulseForce);
                rb.AddForce((transform.position - contacts[i].point) * impulseForce, ForceMode.Impulse);
                
            }
        }
        //somehow recognizes the sword like this

    }

    //Agent Target destination

    private void GoHere(Vector3 target)
    {
        agent.SetDestination(target);
    }
    //Amir's shitty ass patrol loop
    public void Patrol()
    {
        nextPos = PatrolPositions[patrolIndex].position;
        if (Vector3.Distance(transform.position, PatrolPositions[patrolIndex].position) < 5f)
        {
            arrivedAtLocation = true;
            patrolIndex++;
        }
        if (patrolIndex > PatrolPositions.Length - 1)
        {
            patrolIndex = 0;
        }
        if (!arrivedAtLocation)
        {
            GoHere(nextPos);
        }
    }
    public void FollowTarget()
    {
        bool hitNavMesh;
        Vector3 target;
        RaycastHit hit;
        hitNavMesh = Physics.Raycast(playerPos.position, Vector3.down, out hit, 20f, NavMesh.AllAreas);
        target = hit.point;
        GoHere(target);
    }
    public void Attacking()
    {

    }
    public void Stunned()
    {
        
        HP -= numberOfContactPoints * 10;
        HP = Mathf.Clamp(HP, 0, 100);
        sliderRef.value = HP / 100;
        Debug.LogError(HP);
    }
    internal bool IsDead { get => HP >= 0; }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, (currentState == EnemyStates.Following ? playerDetectionDistance + DetectionDisIncris : playerDetectionDistance));
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, PLAYER_ATTACK_RANGE);
    }
}
