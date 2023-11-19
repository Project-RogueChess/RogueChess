using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlBlackHole : MonoBehaviour
{
    public Transform blackHole;
    public AnimationCurve motion;
    public float motionSpeed = 1f;

    public void StartMotion()
    {
        StartCoroutine(BlackHoleAnimation());
    }

    IEnumerator BlackHoleAnimation()
    {
        blackHole.gameObject.SetActive(true);
        var timer = 0f;

        while (timer < motion.keys[motion.keys.Length - 1].time)
        {
            timer += Time.deltaTime * motionSpeed;
            blackHole.transform.localScale = Vector3.one * Mathf.Max(0.001f, motion.Evaluate(timer));
            yield return null;
        }
        blackHole.gameObject.SetActive(false);
    }
}
