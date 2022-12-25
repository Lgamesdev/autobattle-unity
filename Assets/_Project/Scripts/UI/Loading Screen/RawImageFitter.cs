using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class RawImageFitter : MonoBehaviour
{
	private RawImage _image;
    private float _aspectRatio = 1.0f;
    private float _rectAspectRatio = 1.0f;

    private void Start()
    {
    	AdjustAspect();
    }

    void SetupImage()
    {
	    _image = GetComponent<RawImage>();
    	CalculateImageAspectRatio();
    	CalculateTextureAspectRatio();
    }

    void CalculateImageAspectRatio()
    {
    	RectTransform rt = transform as RectTransform;
        _aspectRatio = rt.sizeDelta.x / rt.sizeDelta.y;
    }

    void CalculateTextureAspectRatio()
    {
    	if(_image == null)
    	{
    		Debug.Log("CalculateAspectRatio: m_image is null");
    		return;
    	}

    	Texture2D texture = (Texture2D) _image.texture;
    	if(texture == null)
    	{
    		Debug.Log("CalculateAspectRatio: texture is null");
    		return;
    	}

    	
        _aspectRatio = (float)texture.width / texture.height;
    	//Debug.Log("textW=" + texture.width + " h=" + texture.height + " ratio=" + m_aspectRatio);
    }

    private void AdjustAspect()
    {
    	SetupImage();

    	bool fitY = _aspectRatio < _rectAspectRatio;

    	SetAspectFitToImage(_image, fitY, _aspectRatio);
    }


    private void SetAspectFitToImage(RawImage image,
    				 bool yOverflow, float displayRatio)
    {
    	if (image == null)
    	{
    		return;
    	}
    	
    	Rect rect = new Rect(0, 0, 1, 1);   // default
    	if (yOverflow)
    	{

    		rect.height = _aspectRatio / _rectAspectRatio;
    		rect.y = (1 - rect.height) * 0.5f;
    	}
    	else
    	{
    		rect.width = _rectAspectRatio / _aspectRatio;
    		rect.x = (1 - rect.width) * 0.5f; 
    		
    	}
    	image.uvRect = rect;
    }
}
