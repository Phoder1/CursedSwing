using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoveTools;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    Camera mainCamera;
    [SerializeField]
    float Speed;
    [SerializeField]
    MoveDimension moveX;
    [SerializeField]
    MoveDimension moveZ;
    public static float rotationAngle;
    const float MAX_ANGLE_ROTATION = 360f;
    Vector3 mousePosition;
    float speedX;
    float speedY;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        moveX.UpdateVelocity(Input.GetAxis("Horizontal") * Speed);
        moveZ.UpdateVelocity(Input.GetAxis("Vertical") * Speed);
        moveX.SetPosition(Vector3.right);
        moveZ.SetPosition(transform.position - mainCamera.transform.position);

        
        Debug.Log(Input.mousePosition);

        float playerAngle = (transform.rotation.eulerAngles.y > 180f ? -1 : 1) * Vector3.Angle(Vector3.forward, transform.forward);
        float mouseAngle = Mathf.Sign(Input.mousePosition.x - Screen.width/2) * Vector2.Angle(Vector2.up, (Vector2)Input.mousePosition - new Vector2(Screen.width / 2,Screen.height/2));
        rotationAngle = Mathf.Clamp(Mathf.DeltaAngle(playerAngle, mouseAngle), -MAX_ANGLE_ROTATION* Time.deltaTime, MAX_ANGLE_ROTATION * Time.deltaTime);


        //Rotation angle math


        Debug.Log("position: " + transform.position + " ,mouse position: " + mousePosition + " ,Mouse angle: " + mouseAngle +" ,Player angle: " + playerAngle + " ,Angle delta: "+ rotationAngle);
        transform.Rotate(Vector3.up * rotationAngle);

        //cameraTransform.position = new Vector3(transform.position.x,transform.position.y,cameraTransform.position.z);
        
        //speedX = Input.GetAxis("Horizontal") * Time.deltaTime * Speed;
        //speedY = Input.GetAxis("Vertical") * Time.deltaTime * Speed;

        //transform.position += Vector3.up; 
        



    }
}
