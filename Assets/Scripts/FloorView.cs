using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class FloorView : MonoBehaviour
{
    private const string FLOOR_NAME = "Floor";
    
    public ILiftFloorChanged liftFloorChanged;
    
    [SerializeField] private Text _floorNameText;
    [SerializeField] private Button _upButton;
    [SerializeField] private Button _downButton;
    [SerializeField] private GameObject _indicator;
    
    private int _floorNumber;
    private DirectionType _directionRequest = DirectionType.None;   
    public int FloorNumber
    {
        get { return _floorNumber; }
    }

    public DirectionType DirectionRequest
    {
        get { return _directionRequest; }
    }

    private void Start()
    {
        _upButton.onClick.AddListener(MoveUpRequest);
        _downButton.onClick.AddListener(MoveDownRequest);
        liftFloorChanged.FloorChanged += FloorChanged;
    }
    
    public void Init(int number)
    {
        _floorNameText.text = FLOOR_NAME + number;
        _floorNumber = number;
    }

    private void MoveUpRequest()
    {
        _directionRequest = DirectionType.Up;
    }

    private void MoveDownRequest()
    {
        _directionRequest = DirectionType.Down;
    }

    private void FloorChanged(int floor)
    {
        UpdateIndicator(floor == _floorNumber);
    }
    
    private void UpdateIndicator(bool isLiftHere)
    {
        _indicator.SetActive(isLiftHere);
    }
}

