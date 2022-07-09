using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIParticleAttractorSetUp : MonoBehaviour
{
    [SerializeField] GameObject worldSpaceRepresentationObject; // What gameObject will be spawned for real world representation
    [SerializeField] ParticleSystem attachedParticleSystem; // Particle system to attract particles from

    [SerializeField] Vector3 worldRepOffset; // Z controls how far from camera real world object should be spawned, X and Y fine tune positioning

    [SerializeField] [Range(.0001f, 1000f)] float attractSpeed; // How fast the particle should go towards the attractor
    [SerializeField] [Range(0, 20)] float attractDelay; // How long after a particle a spawn should it be attracted

    
    void Start()
    {
        Vector3 worldSpaceRepCoordinates = Camera.main.ScreenToWorldPoint(new Vector3(transform.position.x, transform.position.y, worldRepOffset.z));
        GameObject worldSpaceRep = GameObject.Instantiate(worldSpaceRepresentationObject);

        worldSpaceRep.transform.position = worldSpaceRepCoordinates;
        worldSpaceRep.GetComponent<ParticleAttractor>().ParticleAttractorInitialization(attachedParticleSystem, attractSpeed, attractDelay);
    }
}
