using System;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [Serializable]
    internal class MoveDimension {
        [SerializeField]
        float maxVelocity;

        internal CharacterController MoveController;

        float speed;

        
        internal void UpdateVelocity(float value) {
            speed = value * Time.deltaTime;
            speed = Mathf.Clamp(speed, -maxVelocity, maxVelocity);
        }

        internal void SetPosition(Vector3 _direction) {
            MoveController.Move( _direction * speed);
        }

    }

    [SerializeField]
    Transform mainCamera;
    [SerializeField]
    Animator swordAnim;
    [SerializeField]
    Animator shieldAnim;
    [SerializeField]
    float Speed;
    [SerializeField]
    MoveDimension moveX;
    [SerializeField]
    MoveDimension moveZ;
    [SerializeField]
    float jumpForce;
    [SerializeField]
    float jumpGravity;
    [SerializeField]
    float dropGravity;
    const float RESET_GRAVITY = -2f;
    float velocityY = RESET_GRAVITY;
    bool isGrounded;
    CharacterController playerMoveController;

    
    public static float rotationAngle;
    const float MAX_ANGLE_ROTATION = 360f;



    // Start is called before the first frame update
    void Start() {
        moveX.MoveController = moveZ.MoveController = playerMoveController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update() {
        UpdateMovement();
        UpdateRotation();
        CheckControlls();

    }

    private void CheckControlls() {
        if (Input.GetMouseButton(1)) {
            swordAnim.SetBool("IsShielding", true);
            shieldAnim.SetBool("IsShielding", true);
        }
        else {
            swordAnim.SetBool("IsShielding", false);
            shieldAnim.SetBool("IsShielding", false);
        }
    }

    private void UpdateRotation() {
        float playerAngle = (transform.rotation.eulerAngles.y < 180f ? 1 : -1) * Vector3.Angle(Vector3.forward, transform.forward);
        float cameraAngle = (mainCamera.rotation.eulerAngles.y < 180f ? 1 : -1) * Vector3.Angle(Vector3.forward, mainCamera.forward);
        float mouseAngle = Mathf.Sign(Input.mousePosition.x - Screen.width / 2) * Vector2.Angle(Vector2.up, (Vector2)Input.mousePosition - new Vector2(Screen.width / 2, Screen.height / 2));
        rotationAngle = Mathf.Clamp(Mathf.DeltaAngle(playerAngle, cameraAngle + mouseAngle), -MAX_ANGLE_ROTATION * Time.deltaTime, MAX_ANGLE_ROTATION * Time.deltaTime);
        //Rotation angle math


        //Debug.Log("Camera angle: " + cameraAngle + " ,Rotation angle: " + rotationAngle + " ,Mouse angle: " + mouseAngle + " ,Player angle: " + playerAngle + " ,Angle delta: " + rotationAngle);
        transform.Rotate(Vector3.up * rotationAngle);
    }

    private void UpdateMovement() {
        moveX.UpdateVelocity(Input.GetAxis("Horizontal") * Speed);
        moveZ.UpdateVelocity(Input.GetAxis("Vertical") * Speed);
        moveX.SetPosition(mainCamera.right);
        moveZ.SetPosition(mainCamera.forward);


        //Y
        if (playerMoveController.isGrounded) {
            isGrounded = true;
        }

        if (isGrounded && Input.GetKeyDown("space")) {
            velocityY += jumpForce;
            isGrounded = false;
        }
        else if (!isGrounded) {
            if (velocityY > 0) {
                velocityY += jumpGravity * Time.deltaTime;
            }
            else {
                velocityY += dropGravity * Time.deltaTime;
            }
        }
        else {
            velocityY = RESET_GRAVITY;
            //Debug.Log("Grounded");
        }
        playerMoveController.Move(Vector3.up * velocityY * Time.deltaTime);
        
    }

}

