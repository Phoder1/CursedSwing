using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class EnemyController : MonoBehaviour
{
    //Rb Hit Force calculations mostly (go to onCollision methods for implementation)
    Rigidbody rb;
    float collisionForce;
    const float forceAmount = 50f;
    float impulseForce;
    [SerializeField]
    float maxImpulseForce = 1.5f;


    //State Machine
    Animator animator;
    [SerializeField]
    private Transform[] PatrolPositions;
    private int patrolIndex;
    private float DistanceFromPlayer;

    private bool canAttack;
    private bool isAlive;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float DetectionDiatance;
    //AI stuff
    private NavMeshAgent agent;
    private Transform playerPos;

    //probabaly awake cuz its gonna be instantiated in the near future, most def
    private void Awake()
    {
        patrolIndex = 0;
        animator = GetComponent<Animator>();
        canAttack = true;
        isAlive = true;
        agent = GetComponent<NavMeshAgent>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
    }


    //When Hit with anything, instances are set in collision matrix
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Stunned();
        }
        collisionForce = PlayerController.rotationAngle;
        impulseForce = Mathf.Clamp(Mathf.Abs(collisionForce) * Time.deltaTime * forceAmount, 0f, maxImpulseForce);
        rb.AddForce((transform.position - collision.transform.position) * impulseForce, ForceMode.Impulse);
        //Debug.Log("NITROOOOOO : " + (transform.position - collision.transform.position));
    }
    //Agent Target destination
    private void GoHere(Vector3 target)
    {
        agent.SetDestination(target);
    }
    //Amir's shitty ass patrol loop
    public void Patrol()
    {
        Vector3 nextPos;
        if (Vector3.Distance(transform.position, PatrolPositions[patrolIndex].position) < 5f)
        {
            patrolIndex++;
        }
        if (patrolIndex > PatrolPositions.Length - 1)
        {
            patrolIndex = 0;
        }
        nextPos = PatrolPositions[patrolIndex].position;
        GoHere(nextPos);
    }
    public void FollowTarget()
    {
        Vector3 target;
        RaycastHit hit;
        if (Physics.Raycast(playerPos.position, Vector3.down, out hit, 20f) && hit.point != null)
        {
            target = hit.point;
        }
        GoHere(target);
    }
    public void Attacking()
    {

    }
    public void Stunned()
    {

    }
    public void Death()
    {

    }
    private void Update()
    {
        DistanceFromPlayer = Vector3.Distance(transform.position, playerPos.position);
        animator.SetFloat("DistanceFromPlayer", DistanceFromPlayer);
    }
}
