using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSound : MonoBehaviour
{
    AudioSource aSource;
    private void Start()
    {
        aSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        aSource.Stop();
        aSource.Play();
    }
}
