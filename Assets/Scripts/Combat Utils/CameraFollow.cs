using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;
    [SerializeField] float speed;

    // Start is called before the first frame update
    // Find the target player when starting the game
    void Start()
    {
        target = FindObjectOfType<PlayerController>().transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //The camera will move according to the character frame by frame
        /*  transform.position = Vector3.Lerp(transform.position,target.position + offset,Time.deltaTime*speed);
        if (Vector3.Distance(transform.position, target.position - offset) < 0.1f) transform.position = target.position + offset;*/
        transform.position = target.position + offset;
    }
}
