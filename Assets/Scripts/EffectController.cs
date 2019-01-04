using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    private new ParticleSystem particleSystem;

    private ParticleSystem[] particleSystems;

	void Start ()
    {
        particleSystem = GetComponent<ParticleSystem>();
	}

    private void OnEnable()
    {
        EventManager.Subscribe("PauseGame", Disable);
        EventManager.Subscribe("ContinueGame", Enable);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe("PauseGame", Disable);
        EventManager.Unsubscribe("ContinueGame", Enable);
    }

    private void Disable(object argument)
    {
        if (particleSystem)
            particleSystem.Pause();

        if (particleSystems == null)
            particleSystems = GetComponentsInChildren<ParticleSystem>();

        foreach (ParticleSystem system in particleSystems)
        {
            system.Pause();
        }
    }

    private void Enable(object argument)
    {
        if (particleSystem)
            particleSystem.Play();

        if (particleSystems == null)
            particleSystems = GetComponentsInChildren<ParticleSystem>();

        foreach (ParticleSystem system in particleSystems)
        {
            system.Play();
        }
    }

}
