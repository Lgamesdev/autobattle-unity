using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using LGamesDev.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private Transform _loadingText;
    public ProgressBar progressBar;

    [SerializeField] private ParallaxBackgroundUI _parallaxBackground;
    private Coroutine _scrollingCoroutine;

    private bool _loadingScreenEnabled;
    private bool _waitingScreenEnabled;

    private async void Awake()
    {
        _loadingScreenEnabled = false;
        _waitingScreenEnabled = false;
        
        gameObject.SetActive(false);
    }
    
    /**
     * show a black alpha overlay with a "loading..." text
     */
    public IEnumerator EnableWaitingScreen()
    {
        _waitingScreenEnabled = true;
        
        if (!_loadingScreenEnabled)
        {
            gameObject.SetActive(true);
            _parallaxBackground.gameObject.SetActive(false);
            
            LTDescr anim = LeanTween.alpha(transform.GetComponent<RectTransform>(), .4f, .25f)
                .setEase(LeanTweenType.linear);

            yield return new WaitForSeconds(anim.time);
        }
        
        progressBar.gameObject.SetActive(false);
        _loadingText.gameObject.SetActive(true);

        yield return new WaitForEndOfFrame();
    }
    
    public IEnumerator DisableWaitingScreen()
    {
        if (!_loadingScreenEnabled)
        {
            LTDescr anim = LeanTween.alpha(transform.GetComponent<RectTransform>(), 0f, .25f).setEase(LeanTweenType.linear);

            yield return new WaitForSeconds(anim.time);
            
            gameObject.SetActive(false);
        }
        else
        {
            progressBar.gameObject.SetActive(true);
            _loadingText.gameObject.SetActive(false);
        }
        
        _waitingScreenEnabled = false;

        yield return new WaitForEndOfFrame();
    }
    
    /**
     * show a parallax background with a loading text
     */
    public async Task EnableLoadingScreen()
    {
        if (!_loadingScreenEnabled)
        {
            _loadingScreenEnabled = true;

            if (!_waitingScreenEnabled)
            {
                gameObject.SetActive(true);
            }

            _parallaxBackground.gameObject.SetActive(true);
            progressBar.gameObject.SetActive(true);
            _loadingText.gameObject.SetActive(false);

            _scrollingCoroutine = StartCoroutine(_parallaxBackground.ScrollingBackground());

            foreach (RectTransform layer in _parallaxBackground.GetComponentsInChildren<RectTransform>())
            {
                LTDescr anim = layer.LeanAlpha(1f, .50f);
                await Task.Yield();
                //yield return new WaitForSeconds(anim.time);
            }
        }

        await Task.Yield();
    }

    public async Task DisableLoadingScreen()
    {
        foreach (RectTransform layer in _parallaxBackground.GetComponentsInChildren<RectTransform>())
        {
            LTDescr anim = layer.LeanAlpha(0f, .50f);
            await Task.Yield();
            //yield return new WaitForSeconds(anim.time);
        }

        /*if (!_waitingScreenEnabled)
        {*/
            gameObject.SetActive(false);
            _waitingScreenEnabled = false;
        //}

        _loadingScreenEnabled = false;
    }
}
