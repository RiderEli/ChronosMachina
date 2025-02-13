using System.Collections;
using UnityEngine;

public class Flare : MonoBehaviour
{
    public Light flareLight; // The light component of the flare
    public ParticleSystem flareParticles; // Optional: Particle system for the flare

    public float flareDuration = 2f; // Total lifetime of the flare
    public float maxIntensity = 5f; // Maximum intensity of the light
    public float maxAlpha = 1f; // Maximum alpha of the sprite

    private float timer;
    private Color initialColor;

    void Start()
    {
        if (flareLight)
        {
            flareLight.intensity = maxIntensity;
        }

        if (flareParticles)
        {
            flareParticles.Play();
        }

        timer = flareDuration;
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            float progress = timer / flareDuration;

            if (flareLight)
            {
                flareLight.intensity = maxIntensity * progress;
            }

            yield return null;
        }

        Destroy(gameObject);
    }
}