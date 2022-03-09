using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBeamController : MonoBehaviour
{
    ParticleSystem particle;
    ParticleSystem.MainModule particleMain;
    void Start()
    {
        particle = GetComponent<ParticleSystem>();
        particleMain = particle.main;
    }

    void FixedUpdate()
    {
        RaycastHit rayHit;
        Ray ray = new Ray(transform.position, transform.forward);
        Physics.Raycast(ray, out rayHit);

        if(rayHit.collider != null)
        {
            particleMain.startLifetime = particleMain.startSpeed; //* magnitude of start to end of the ray
        }

        if (rayHit.collider.GetComponent(typeof(ITakeLight)))
        {
            ITakeLight obj = (ITakeLight)rayHit.collider.GetComponent(typeof(ITakeLight));
            obj.LightHit(ray);
        }

    }
}
