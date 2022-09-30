using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParallaxBackgroundUI : MonoBehaviour
{
    private RawImage _background;
    private RawImage _backMountains;
    private RawImage _clouds;
    private RawImage _mountains;
    private RawImage _treesGround;

    private void Awake()
    {
        _backMountains = transform.Find("BackMountains")
            .GetComponent<RawImage>();
        _clouds = transform.Find("Clouds")
            .GetComponent<RawImage>();
        _mountains = transform.Find("Mountains")
            .GetComponent<RawImage>();
        _treesGround = transform.Find("TreesGround")
            .GetComponent<RawImage>();
    }
    
    public IEnumerator ScrollingBackground()
    {
        while (gameObject.activeInHierarchy)
        {
            _backMountains.uvRect = new Rect(_backMountains.uvRect.position + new Vector2(0.005f, 0) * Time.deltaTime,
                _backMountains.uvRect.size);
            _clouds.uvRect = new Rect(_clouds.uvRect.position + new Vector2(0.01f, 0) * Time.deltaTime,
                _clouds.uvRect.size);
            _mountains.uvRect = new Rect(_mountains.uvRect.position + new Vector2(0.015f, 0) * Time.deltaTime,
                _mountains.uvRect.size);
            _treesGround.uvRect = new Rect(_treesGround.uvRect.position + new Vector2(0.02f, 0) * Time.deltaTime,
                _treesGround.uvRect.size);

            yield return new WaitForEndOfFrame();
        }
    }
}
