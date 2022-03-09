using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour, ITakeLight
{
    GameObject reflection;
    [SerializeField] GameObject lightPrefab;
    public void LightHit(Ray ray = default)
    {
        if (reflection == null)
        {
            GameObject obj = Instantiate(lightPrefab, transform.position, Quaternion.identity);
            reflection = obj;
        }

        reflection.transform.forward = Vector3.Reflect(ray.direction, transform.forward);
    }

}
