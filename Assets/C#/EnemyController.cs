using System.Collections;
using UnityEngine;
using UnityEngine.AI;

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
    public Animator animator;
    private float DistanceFromPlayer;
    private StateController state;
    public bool canAttack;
    public bool isAlive;

    private Transform firstPatrolPos;
    private Transform SecondPatrolPos;
    public float moveSpeed;
    private Vector3 originalPos;

    //AI stuff
    private NavMeshAgent agent;
    private Transform playerPos;

    //probabaly awake cuz its gonna be instantiated in the near future, most def
    private void Awake()
    {
        canAttack = true;
        isAlive = true;
        originalPos = transform.position;
        moveForward = true;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Patrol();
    }


    //When Hit with anything, instances are set in collision matrix
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            StartCoroutine(Stunned());
        }
        collisionForce = PlayerController.rotationAngle;
        impulseForce = Mathf.Clamp(Mathf.Abs(collisionForce) * Time.deltaTime * forceAmount, 0f, maxImpulseForce);
        rb.AddForce((transform.position - collision.transform.position) * impulseForce, ForceMode.Impulse);
        Debug.Log("NITROOOOOO : " + (transform.position - collision.transform.position));
    }

    //Agent Target destination
    private void GoHere(Vector3 target)
    {
        agent.SetDestination(target);
    }

    private void Patrol()
    {
        if (DistanceFromPlayer < 30f)
        {
            FollowTarget();
            return;
        }

        if (moveForward)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x + 10, transform.position.y, transform.position.z), moveSpeed * Time.deltaTime);
            if (transform.position.x >= originalPos.x + 10f)
            {
                moveForward = false;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x - 10, transform.position.y, transform.position.z), moveSpeed * Time.deltaTime);
            if (transform.position.x <= originalPos.x)
            {
                moveForward = true;
            }
        }
    }
    private void FollowTarget()
    {
        if (DistanceFromPlayer >= 30f)
        {
            Patrol();
            return;
        }
        transform.LookAt(playerPos);
        transform.position = Vector3.MoveTowards(transform.position, playerPos.position, moveSpeed * Time.deltaTime);
        if (canAttack && DistanceFromPlayer < 15f)
        {
            moveSpeed = 0f;
            StartCoroutine(Attacking());
        }
    }
    private IEnumerator Attacking()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 10f);
        canAttack = false;
        yield return new WaitForSeconds(3f);
        canAttack = true;
        moveSpeed = 10f;
        FollowTarget();
    }
    private IEnumerator Stunned()
    {
        Debug.LogError("I am Stunned");

        yield return new WaitForSeconds(1f);
        Patrol();

    }
    private void Update()
    {
        DistanceFromPlayer = Vector3.Distance(transform.position, playerPos.position);
        animator.SetFloat("DistanceFromPlayer", DistanceFromPlayer);
        if (isAlive)
        {
            Patrol();
        }
        else
        {
            Debug.LogError("Dead");
        }
    }
}
