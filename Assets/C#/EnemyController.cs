using UnityEngine;

public class EnemyController : MonoBehaviour {
    float collisionForce;
    Rigidbody2D rb;
    const float forceAmount = 50f;
    float impulseForce;
    [SerializeField]
    float maxImpulseForce = 1.5f;
    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update() {
        
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        collisionForce = PlayerController.rotationAngle;
        impulseForce = Mathf.Clamp(Mathf.Abs(collisionForce) * Time.deltaTime * forceAmount,0f, maxImpulseForce);
        rb.AddForce(((Vector2)transform.position - collision.GetContact(0).point) * impulseForce, ForceMode2D.Impulse);
        Debug.Log("NITROOOOOO : " + impulseForce);
    }
}
