using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnObject : MonoBehaviour
{
    [SerializeField] GameObject respawnAnchor;
     Vector3 respawnPoint = new Vector3();

    private void Start()
    {
        respawnPoint = respawnAnchor.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        other.transform.position = respawnPoint;
    }
}
