using System;
using System.Collections;
using System.Collections.Generic;
using LGamesDev;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.Video;

namespace LGamesDev
{
    public class IntroManager : MonoBehaviour
    {
        [SerializeField] private VideoPlayer videoPlayer;
        [SerializeField] private VideoClip introLandscape;
        [SerializeField] private VideoClip introPortrait;

        private void Start()
        {
            videoPlayer.clip = Screen.orientation switch
            {
                ScreenOrientation.Portrait => introPortrait,
                ScreenOrientation.LandscapeLeft
                    or ScreenOrientation.LandscapeRight => introLandscape,
                _ => introLandscape
            };
            
            videoPlayer.loopPointReached += VideoPlayerOnloopPointReached;
            
            videoPlayer.Play();
        }

        private void VideoPlayerOnloopPointReached(VideoPlayer source)
        {
            SceneManager.LoadScene((int)SceneIndexes.PersistentScene);
        }
    }
}
