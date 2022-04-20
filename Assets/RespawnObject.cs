using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnObject : MonoBehaviour
{
    [SerializeField] Vector3 respawnPoint = new Vector3();

    private void OnTriggerEnter(Collider other)
    {
        other.transform.position = respawnPoint;
    }
}
