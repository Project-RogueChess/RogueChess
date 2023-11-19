using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionFXController : MonoBehaviour
{
    public Transform model;
    public AnimationCurve explosionCurve;

    private void Start()
    {
        StartCoroutine(Animation());
    }

    IEnumerator Animation()
    {
        var timer = 0f;
        model.gameObject.SetActive(true);
        while (timer < explosionCurve.keys[explosionCurve.keys.Length - 1].time)
        {
            timer += Time.deltaTime;
            model.localScale = Vector3.one * Mathf.Max(0.001f, explosionCurve.Evaluate(timer));
            yield return null;
        }
        Destroy(this.gameObject);
    }
}
