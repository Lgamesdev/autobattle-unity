using System;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class SpriteLibManager : MonoBehaviour
{
    public static SpriteLibManager Instance;
    
    [SerializeField] private SpriteLibraryAsset maleLibrary;
    [SerializeField] private SpriteLibraryAsset femaleLibrary;
    
    private SpriteLibrary _currentLibrary;

    public delegate void OnSpriteLibChangeEvent(SpriteLib spriteLib);
    public OnSpriteLibChangeEvent OnSpriteLibChange;

    private void Awake()
    {
        Instance = this;
        
        _currentLibrary = transform.GetComponent<SpriteLibrary>();
    }

    public void SwitchLibrary(SpriteLib library)
    {
        OnSpriteLibChange?.Invoke(library);
        
        _currentLibrary.spriteLibraryAsset = library switch
        {
            SpriteLib.Male => maleLibrary,
            SpriteLib.Female => femaleLibrary,
            _ => throw new ArgumentOutOfRangeException(nameof(library), library, null)
        };
    }
}
