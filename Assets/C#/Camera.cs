using UnityEngine;
[ExecuteInEditMode]
public class Camera : MonoBehaviour
{
    float CameraMoveY;
    float CameraMoveX;
    [SerializeField]
    float cameraRotSpeed;
    public Transform playerTrans;

    [SerializeField]
    float xRotOffSet;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(xRotOffSet, 0f, 0f);
        transform.position = playerTrans.position;
        CameraMoveY = Input.GetAxis("Mouse X");
        if (Input.GetMouseButton(2))
        {
            transform.Rotate(CameraMoveX, CameraMoveY * cameraRotSpeed, 0f);
        }

    }
}
