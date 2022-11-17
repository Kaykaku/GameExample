using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingFlatform : MonoBehaviour
{
    [SerializeField] Transform topPoint, bottomPoint;
    Vector3 target;
    [SerializeField] int speed;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = topPoint.position;
        target = bottomPoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position,target,Time.deltaTime*speed);

        if (Vector3.Distance(transform.position, topPoint.position) <0.1f)
        {
            target = bottomPoint.position;
        }else if (Vector3.Distance(transform.position, bottomPoint.position) < 0.1f)
        {
            target = topPoint.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.transform.SetParent(null);
    }
}
