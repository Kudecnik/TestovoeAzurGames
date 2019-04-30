using UnityEngine;
using UnityEngine.UI;

public class GameInitializer : MonoBehaviour
{   
    [Header("Prefabs")]
    [SerializeField] private GameObject _floor;
    [SerializeField] private GameObject _liftButton;
    [SerializeField] private GameObject _liftStopButton;

    [Space] [Header("Transforms")] 
    [SerializeField] private RectTransform _floorsContent;
    [SerializeField] private RectTransform _liftContent;

    [Header("Game setups")]
    [Space] 
    [SerializeField] private Slider _floorsNumberSlider;
    [SerializeField] private Button _startButton;
    [SerializeField] private GameObject _floorsHolder;
    [SerializeField] private GameObject _liftHolder;
    
    private int _floorsAmount;
    private LiftController _floorChanged;
    
    private void Awake()
    {
        _floorChanged = FindObjectOfType<LiftController>();
        
        _floorsNumberSlider.onValueChanged.AddListener(SetFloorsAmount);
        _startButton.onClick.AddListener(StartGame);
    }

    private void SetFloorsAmount(float floorsAmount)
    {
        _floorsAmount = (int)floorsAmount;
    }

    private void StartGame()
    {
        InitializeFloors();
        InitializeLiftButtons();
        
        _floorsHolder.SetActive(true);
        _liftHolder.SetActive(true);
    }
    
    private void InitializeFloors()
    {
        for (int i = 0; i < _floorsAmount; i++)
        {
            var floorView = Instantiate(_floor, _floorsContent).GetComponent<FloorView>();
            floorView.Init(i);
        }
    }
    
    private void InitializeLiftButtons()
    {
        for (int i = 0; i < _floorsAmount; i++)
        {
            var floorView = Instantiate(_liftButton, _liftContent).GetComponent<LiftButtonView>();
            floorView.Init(i,_floorChanged);
        }
    }
}