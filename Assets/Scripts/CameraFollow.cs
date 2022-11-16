using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;
    [SerializeField] float speed;

    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectOfType<PlayerController>().transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //transform.position = Vector3.Lerp(transform.position,target.position + offset,Time.deltaTime*speed);
        transform.position = target.position + offset;
    }
}
