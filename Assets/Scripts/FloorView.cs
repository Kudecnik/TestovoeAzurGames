using System;
using UnityEngine;
using UnityEngine.UI;

public class FloorView : MonoBehaviour
{
    private const string FLOOR_NAME = "Этаж :";

    public Action<LiftCallCommand> LiftCalled;

    [SerializeField] private Text _floorNameText;
    [SerializeField] private Button _upButton;
    [SerializeField] private Button _downButton;
    [SerializeField] private GameObject _indicator;
    [SerializeField] private Image _buttonImageUp;
    [SerializeField] private Image _buttonImageDown;
    [SerializeField] private Color _higlighedColor;

    private Color _defaultColor;
    private int _floorNumber;
    private DirectionType _directionRequest = DirectionType.None;
    private int _liftCurrentFloor;
    private ILiftFloorInfo _liftFloorInfo;
    
    public int FloorNumber
    {
        get { return _floorNumber; }
    }

    public DirectionType DirectionRequest
    {
        get { return _directionRequest; }
    }

    private void Awake()
    {
        _defaultColor = _buttonImageUp.color;
        _upButton.onClick.AddListener(MoveUpRequest);
        _downButton.onClick.AddListener(MoveDownRequest);
    }

    private void OnDisable()
    {
        _liftFloorInfo.FloorChanged -= FloorChanged;
        _liftFloorInfo.LiftStopOnTheFloor -= LiftStop;
    }

    public void Init(int number, int floorsCount)
    {
        if (number == 0)
        {
            _downButton.gameObject.SetActive(false);   
        }

        if (number == floorsCount)
        {
            _upButton.gameObject.SetActive(false);
        }
        
        _floorNameText.text = FLOOR_NAME + number;
        _floorNumber = number;
    }

    public void Init(ILiftFloorInfo liftFloorInfo)
    {
        _liftFloorInfo = liftFloorInfo;
        liftFloorInfo.FloorChanged += FloorChanged;
        liftFloorInfo.LiftStopOnTheFloor += LiftStop;
    }

    private void MoveUpRequest()
    {
        CallTheLift(DirectionType.Up, _buttonImageUp);
    }

    private void MoveDownRequest()
    {
        CallTheLift(DirectionType.Down, _buttonImageDown);
    }

    private void CallTheLift(DirectionType directionRequest, Image buttonImage)
    {
        if (_directionRequest == DirectionType.None && _liftCurrentFloor != _floorNumber)
        {
            _directionRequest = directionRequest;
            buttonImage.color = _higlighedColor;

            var liftCallCommand = new LiftCallCommand(_floorNumber, _directionRequest);

            if (LiftCalled != null)
            {
                LiftCalled(liftCallCommand);
            }
        }
    }

    private void FloorChanged(int floor)
    {
        UpdateIndicator(floor == _floorNumber);
        _liftCurrentFloor = _liftFloorInfo.CurrentFloor;
    }

    private void LiftStop(int floor)
    {
        if (floor == _floorNumber)
        {
            Reset();
        }
    }

    private void Reset()
    {
        _directionRequest = DirectionType.None;
        _buttonImageUp.color = _defaultColor;
        _buttonImageDown.color = _defaultColor;
    }

    private void UpdateIndicator(bool isLiftHere)
    {
        _indicator.SetActive(isLiftHere);
    }
}