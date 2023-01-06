using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParallaxBackgroundUI : MonoBehaviour
{
    private RawImage _background;
    private RawImage _clouds;
    private RawImage _farBackTrees;
    private RawImage _backTrees;
    private RawImage _ground;

    private void Awake()
    {
        _clouds = transform.Find("Clouds")
            .GetComponent<RawImage>();
        _farBackTrees = transform.Find("FarBackTrees")
            .GetComponent<RawImage>();
        _backTrees = transform.Find("BackTrees")
            .GetComponent<RawImage>();
        _ground = transform.Find("Ground")
            .GetComponent<RawImage>();
    }
    
    public IEnumerator ScrollingBackground()
    {
        while (gameObject.activeInHierarchy)
        {
            _clouds.uvRect = new Rect(_clouds.uvRect.position + new Vector2(.001f, 0) * Time.deltaTime,
                _clouds.uvRect.size);
            _farBackTrees.uvRect = new Rect(_farBackTrees.uvRect.position + new Vector2(.002f, 0) * Time.deltaTime,
                _farBackTrees.uvRect.size);
            _backTrees.uvRect = new Rect(_backTrees.uvRect.position + new Vector2(.003f, 0) * Time.deltaTime,
                _backTrees.uvRect.size);
            _ground.uvRect = new Rect(_ground.uvRect.position + new Vector2(.004f, 0) * Time.deltaTime,
                _ground.uvRect.size);

            yield return new WaitForEndOfFrame();
        }
    }
}
