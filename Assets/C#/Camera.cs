using UnityEngine;
public class Camera : MonoBehaviour
{
    float CameraMoveY;
    float CameraMoveX;
    [SerializeField]
    float cameraRotSpeed;
    public Transform playerTrans;

    public Transform camTrans;

    [SerializeField]
    float xRotOffSet;
    [SerializeField]
    float DistanceFromPlayer;
    [SerializeField]
    float CamHeight;


    // Start is called before the first frame update
    void Start()
    {
        camTrans.transform.localRotation = Quaternion.Euler(xRotOffSet, 0f, 0f);
        camTrans.transform.localPosition = new Vector3(0f, CamHeight, DistanceFromPlayer);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerTrans.position;
        CameraMoveY = Input.GetAxis("Mouse X");
        if (Input.GetMouseButton(2))
        {
            transform.Rotate(CameraMoveX, CameraMoveY * cameraRotSpeed, 0f);
        }

    }
}
