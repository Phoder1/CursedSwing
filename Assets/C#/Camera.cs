using UnityEngine;
public class Camera : MonoBehaviour
{
    public float cameraRotSpeed;
    public Transform playerTrans;

    public Transform camTrans;

    public float rotFromPlayer;
    public float distanceFromPlayer;
    public float cameraRotOffset;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        camTrans.transform.localPosition = (Quaternion.Euler(rotFromPlayer, 0f, 0f) * Vector3.back) * distanceFromPlayer;

        camTrans.rotation = Quaternion.LookRotation(playerTrans.position - camTrans.position, Vector3.up) * Quaternion.Euler(cameraRotOffset,0f,0f);

        //camTrans.transform.localRotation = Quaternion.Euler(xRotOffSet, 0f, 0f);
        //camTrans.transform.localPosition = new Vector3(0f, camHeight, zDistanceFromPlayer);
        transform.position = playerTrans.position;
        if (Input.GetMouseButton(2))
        {
            transform.Rotate(0f, Input.GetAxis("Mouse X") * cameraRotSpeed * Time.deltaTime, 0f);
        }

    }
}
