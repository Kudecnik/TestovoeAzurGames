using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StopButtonView : MonoBehaviour
{
    public Action StopButtonClicked;
    
    [SerializeField] private Button _stopButton;

    private void Awake()
    {
        _stopButton.onClick.AddListener(StopButtonClick);
    }

    private void StopButtonClick()
    {
        if (StopButtonClicked != null)
        {
            StopButtonClicked();
        }
    }
}