using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour {

    [SerializeField]
    LayerMask wallLayer = 6;

    const float RETRACTION_SPEED = 999f;
    bool wallHit;

    private void Update() {
        if (!wallHit) {
            Retract(-RETRACTION_SPEED);
        }
    }
    private void OnCollisionStay(Collision collision) {
        bool wallHit = false;
        Debug.Log("colliding!");
        ContactPoint[] contacts = new ContactPoint[collision.contactCount]; ;
        collision.GetContacts(contacts);
        for (int i = 0; i < collision.contactCount; i++) {
            if (contacts[i].otherCollider.gameObject.layer == 6) {
                Debug.Log("colliding with wall!");
                Retract(RETRACTION_SPEED);
                wallHit = true;
            }
        }
        if (!wallHit) {
            Retract(-RETRACTION_SPEED);
        }

    }
    private void OnCollisionExit(Collision collision) {
        wallHit = false;
    }

    void Retract(float degrees) {
        transform.Rotate(Vector3.right * degrees * Time.deltaTime);
    }
}
