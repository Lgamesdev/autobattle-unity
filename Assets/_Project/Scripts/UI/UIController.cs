using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    [SerializeField] private ModalWindowPanel _modalWindow;

    public ModalWindowPanel modalWindow => _modalWindow;

    private void Awake()
    {
        Instance = this;
    }
}
