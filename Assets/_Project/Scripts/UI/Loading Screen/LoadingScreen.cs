using System;
using System.Collections;
using System.Collections.Generic;
using LGamesDev.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    private Transform _loadingText;
    public ProgressBar progressBar;

    private ParallaxBackgroundUI _parallaxBackground;
    private Coroutine _scrollingCoroutine;

    private void Awake()
    {
        _parallaxBackground = transform.Find("ParallaxBackground").GetComponent<ParallaxBackgroundUI>();

        _loadingText = transform.Find("Progress Panel").Find("LoadingText");
        progressBar = transform.Find("Progress Panel").Find("ProgressBar").GetComponent<ProgressBar>();
    }
    
    public IEnumerator EnableWaitingScreen()
    {
        gameObject.SetActive(true);
        
        _parallaxBackground.gameObject.SetActive(false);
        progressBar.gameObject.SetActive(false);
        _loadingText.gameObject.SetActive(true);

        LTDescr anim = LeanTween.alpha(transform.GetComponent<RectTransform>(), .4f, .25f).setEase(LeanTweenType.linear);

        yield return new WaitForSeconds(anim.time);
    }
    
    public IEnumerator DisableWaitingScreen()
    {
        LTDescr anim = LeanTween.alpha(transform.GetComponent<RectTransform>(), 0f, .25f).setEase(LeanTweenType.linear);

        yield return new WaitForSeconds(anim.time);
            
        gameObject.SetActive(false);
    }
    
    public IEnumerator EnableLoadingScreen()
    {
        gameObject.SetActive(true);
        
        _parallaxBackground.gameObject.SetActive(true);
        progressBar.gameObject.SetActive(true);
        _loadingText.gameObject.SetActive(false);

        _scrollingCoroutine = StartCoroutine(_parallaxBackground.ScrollingBackground());

        LTDescr anim = LeanTween.alpha(transform.GetComponent<RectTransform>(), 0f, 1f).setEase(LeanTweenType.linear).setOnComplete(() =>
        {
            LeanTween.alpha(transform.GetComponent<RectTransform>(), 1f, 1f).setEase(LeanTweenType.linear);
        });

        yield return new WaitForSeconds(anim.time);
    }

    public IEnumerator DisableLoadingScreen()
    {
        LTDescr anim = LeanTween.alpha(transform.GetComponent<RectTransform>(), 1f, 1f).setEase(LeanTweenType.linear).setOnComplete(() =>
        {
            LeanTween.alpha(transform.GetComponent<RectTransform>(), 0f, 1f).setEase(LeanTweenType.linear);
        });

        yield return new WaitForSeconds(anim.time);

        gameObject.SetActive(false);
    }
}
