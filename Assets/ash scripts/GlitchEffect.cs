using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GlitchEffect : MonoBehaviour
{
    [Header("Glitch Settings")]
    public float minDelay = 3f;
    public float maxDelay = 7f;

    [Header("Assign These")]
    public RawImage backgroundImage;  // drag your background image here

    private Vector3 originalPos;
    private Material mat;

    void Start()
    {
        originalPos = transform.position;

        // Make a copy of the material so we don't edit the original
        if (backgroundImage != null)
            mat = backgroundImage.material = new Material(backgroundImage.material);

        StartCoroutine(GlitchLoop());
    }

    IEnumerator GlitchLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
            yield return StartCoroutine(PlayGlitch());
        }
    }

    IEnumerator PlayGlitch()
    {
        int waves = Random.Range(2, 5);

        for (int i = 0; i < waves; i++)
        {
            // Shake the whole menu
            transform.position = originalPos + new Vector3(
                Random.Range(-8f, 8f),
                Random.Range(-4f, 4f),
                0
            );

            // Tint the background red/white briefly
            if (backgroundImage != null)
                backgroundImage.color = Random.value > 0.5f
                    ? new Color(1f, 0.3f, 0.3f, 1f)
                    : new Color(1.5f, 1.5f, 1.5f, 1f);

            yield return new WaitForSeconds(Random.Range(0.03f, 0.1f));

            // Reset
            transform.position = originalPos;
            if (backgroundImage != null)
                backgroundImage.color = Color.white;

            yield return new WaitForSeconds(Random.Range(0.02f, 0.08f));
        }
    }
}