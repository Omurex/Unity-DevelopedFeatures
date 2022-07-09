using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAttractor : MonoBehaviour
{
    [SerializeField] ParticleSystem linkedParticleSystem; // Particle system to affect

    [SerializeField] [Range(.0001f, 1000f)] float attractSpeed; // How fast the particle should go towards the attractor
    [SerializeField] [Range(0, 20)] float attractDelay; // How long after a particle a spawn should it be attracted

    ParticleSystem.Particle[] particles; // Array of particles currently emitted by linkedParticleSystem; Updated every update cycle


    void Start()
    {
        particles = new ParticleSystem.Particle[linkedParticleSystem.main.maxParticles];
    }


    public void ParticleAttractorInitialization(ParticleSystem pS, float attractSpd, float attractDly) // If set by designers, no need to call this. If script attached from code, MUST call this function
    {
        linkedParticleSystem = pS;
        attractSpeed = attractSpd;
        attractDelay = attractDly;
    }


    void Update()
    {
        AttractParticles(); // Every update cycle, try to attract existing particles
    }


    private void AttractParticles()
    {
        linkedParticleSystem.GetParticles(particles); // Get currently emitted particles
        int particleCount = linkedParticleSystem.particleCount; 

        for(int i = 0; i < particleCount; i++) // For each existing particle, try to attract
        {
            ParticleSystem.Particle p = particles[i];

            if(p.remainingLifetime <= p.startLifetime - attractDelay) // If attractDelay amount of time has passed, attract particle
            {
                particles[i].position = Vector3.Lerp(p.position, transform.position, Time.deltaTime * attractSpeed);
            }
        }

        linkedParticleSystem.SetParticles(particles, particleCount);
    }
}
