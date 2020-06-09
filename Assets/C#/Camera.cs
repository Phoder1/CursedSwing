using UnityEngine;
public class Camera : MonoBehaviour
{
    float CameraMoveY;
    public float cameraRotSpeed;
    public Transform playerTrans;

    public Transform camTrans;

    public float xRotOffSet;
    public float zDistanceFromPlayer;
    public float camHeight;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        camTrans.transform.localRotation = Quaternion.Euler(xRotOffSet, 0f, 0f);
        camTrans.transform.localPosition = new Vector3(0f, camHeight, zDistanceFromPlayer);
        transform.position = playerTrans.position;
        CameraMoveY = Input.GetAxis("Mouse X");
        if (Input.GetMouseButton(2))
        {
            transform.Rotate(0f, CameraMoveY * cameraRotSpeed, 0f);
        }

    }
}
