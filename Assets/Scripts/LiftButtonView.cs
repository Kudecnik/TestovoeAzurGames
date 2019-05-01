using System;
using UnityEngine;
using UnityEngine.UI;

public class LiftButtonView : MonoBehaviour
{
    public Action<int> FloorSelected;

    [SerializeField] private Color _higlightedColor;
    [SerializeField] private Text _floorText;
    [SerializeField] private Button _floorSelectButton;
    [SerializeField] private Image _liftButtonImage;

    private Color _defaultColor;
    private int _floor;
    private bool _isFloorSelected;
    private ILiftFloorInfo _liftFloorInfo;

    private void Awake()
    {
        _defaultColor = _liftButtonImage.color;
        _floorSelectButton.onClick.AddListener(FloorSelect);
    }

    public void Init(int floor, ILiftFloorInfo liftFloorInfo)
    {
        _floorText.text = floor.ToString();
        _floor = floor;
        _liftFloorInfo = liftFloorInfo;
        liftFloorInfo.FloorChanged += OnFloorChanged;
    }

    private void FloorSelect()
    {
        if (_isFloorSelected == false && _liftFloorInfo.CurrentFloor != _floor)
        {
            if (FloorSelected != null)
                FloorSelected(_floor);

            _liftButtonImage.color = _higlightedColor;
            _isFloorSelected = true;
        }
    }

    private void OnFloorChanged(int floor)
    {
        if (floor == _floor)
        {
            _isFloorSelected = false;
            _liftButtonImage.color = _defaultColor;
        }
    }
}