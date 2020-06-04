using UnityEngine;

public class EnemyController : MonoBehaviour {
    float collisionForce;
    Rigidbody rb;
    const float forceAmount = 50f;
    float impulseForce;
    [SerializeField]
    float maxImpulseForce = 1.5f;
    private void Start() {
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update() {
        
    }

    private void OnCollisionEnter(Collision collision) {
        collisionForce = PlayerController.rotationAngle;
        impulseForce = Mathf.Clamp(Mathf.Abs(collisionForce) * Time.deltaTime * forceAmount, 0f, maxImpulseForce);
        rb.AddForce((transform.position - collision.GetContact(0).point) * impulseForce, ForceMode.Impulse);
        Debug.Log("NITROOOOOO : " + impulseForce);
    }

}
