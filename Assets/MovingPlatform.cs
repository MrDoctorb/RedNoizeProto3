using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovingPlatform : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Vector3 endPos;
    Vector3 startPos;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
        StartCoroutine(MoveTo(endPos));
    }

    IEnumerator MoveTo(Vector3 point)
    {
        while (Vector3.Distance(transform.position, point) > .1f)
        {
            rb.velocity = (point - transform.position).normalized * speed;
            yield return new WaitForEndOfFrame();
        }

        if (point == endPos)
        {
            StartCoroutine(MoveTo(startPos));
        }
        else
        {
            StartCoroutine(MoveTo(endPos));
        }
    }
}
