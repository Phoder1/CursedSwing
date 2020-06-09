using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;
[ExecuteInEditMode]

public class FloatingHPcanvas : MonoBehaviour
{
    public Transform BigDaddy;
    private Transform camTrans;
    public float AddedHeight;
    // Start is called before the first frame update
    void Start()
    {
        camTrans = FindObjectOfType<UnityEngine.Camera>().transform;
    }
    private void LateUpdate()
    {
        transform.LookAt(transform.position + camTrans.forward);
        transform.position = new Vector3(BigDaddy.position.x, BigDaddy.position.y + AddedHeight, BigDaddy.position.z);
    }
}
