using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fancy_Emission : MonoBehaviour
{
    private Material myMaterial;

    public float cycleSpeed = 1.0f;
    private float timeCounter = 0.0f;

    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        myMaterial = renderer.material;

        // 초기값 설정
        myMaterial.SetColor("_EmissionColor", new Color(1f, 0f, 0f)); // 초기값: R = 1, G = 0, B = 0
    }

    void Update()
    {
        timeCounter += Time.deltaTime * cycleSpeed;

        float r = Mathf.Sin(timeCounter);
        float g = Mathf.Sin(timeCounter - (2f * Mathf.PI / 3f));
        float b = Mathf.Sin(timeCounter - (4f * Mathf.PI / 3f));

        r = Mathf.Clamp01((r + 1f) / 2f); // 0에서 1 사이로 조절
        g = Mathf.Clamp01((g + 1f) / 2f);
        b = Mathf.Clamp01((b + 1f) / 2f);

        // Emission 값 설정
        Color emissionColor = new Color(r, g, b);
        myMaterial.SetColor("_EmissionColor", emissionColor);
    }
}
