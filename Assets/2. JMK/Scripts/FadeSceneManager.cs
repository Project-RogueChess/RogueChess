using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FadeSceneManager : MonoBehaviour
{
    public static FadeSceneManager instance;
    public RawImage fadeImage;

    private bool _playingFadeMotion;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    private void Start()
    {
        StartCoroutine(StartGame());
    }

    public void FadeInOut(float duration)
    {
        if (_playingFadeMotion)
            return;

        StartCoroutine(PlayFadeInOut(duration));
    }

    public void FadeIn()
    {
        if (_playingFadeMotion)
            return;

        StartCoroutine(PlayFadeIn());
    }

    public void FadeOut()
    {
        if (_playingFadeMotion)
            return;

        StartCoroutine(PlayFadeOut());
    }

    IEnumerator PlayFadeInOut(float duration)
    {
        _playingFadeMotion = true;
        fadeImage.raycastTarget = true;
        var timer = 0f;
        while (timer < 1.5f)
        {
            fadeImage.color = Color.Lerp(new Color(0f, 0f, 0f, 0f), Color.black, timer / 1.5f);
            timer += Time.deltaTime;
            yield return null;
        }
        timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        timer = 0f;
        while (timer < 1.5f)
        {
            fadeImage.color = Color.Lerp(Color.black,new Color(0f, 0f, 0f, 0f), timer / 1.5f);
            timer += Time.deltaTime;
            yield return null;
        }
        fadeImage.raycastTarget = false;
        _playingFadeMotion = false;
    }

    IEnumerator PlayFadeIn()
    {
        _playingFadeMotion = true;
        fadeImage.raycastTarget = true;
        fadeImage.color = Color.black;

        var timer = 0f;
        while (timer < 1.5f)
        {
            fadeImage.color = Color.Lerp(Color.black, new Color(0f, 0f, 0f, 0f), timer / 1.5f);
            timer += Time.deltaTime;
            yield return null;
        }

        fadeImage.raycastTarget = false;
        _playingFadeMotion = false;
    }

    IEnumerator PlayFadeOut()
    {
        _playingFadeMotion = true;
        fadeImage.raycastTarget = true;
        fadeImage.color = new Color(0f, 0f, 0f, 0f);

        var timer = 0f;
        while (timer < 2f)
        {
            fadeImage.color = Color.Lerp(new Color(0f, 0f, 0f, 0f), Color.black, timer / 1.5f);
            timer += Time.deltaTime;
            yield return null;
        }

        fadeImage.raycastTarget = false;
        _playingFadeMotion = false;
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(PlayFadeIn());
    }
}
