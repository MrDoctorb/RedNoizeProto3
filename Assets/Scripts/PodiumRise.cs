using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodiumRise : MonoBehaviour, IReactive
{
    float startY;
    // Start is called before the first frame update
    void Start()
    {
        startY = transform.position.y;
    }

    public void TurnOn()
    {
        StartCoroutine(Move());
    }

    public void TurnOff()
    {

    }

    IEnumerator Move()
    {

        transform.position += Vector3.up * Time.deltaTime;
        yield return new WaitForEndOfFrame();
        if(transform.position.y -startY < 2)
        {
            StartCoroutine(Move());
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
}
