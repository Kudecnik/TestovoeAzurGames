using UnityEngine;
using UnityEngine.UI;

public class GameInitializer : MonoBehaviour
{
    private const string CURRENT_FLOORS = "Количество этаже :";

    [Header("Prefabs")] [SerializeField] private GameObject _floor;
    [SerializeField] private GameObject _liftButton;
    [SerializeField] private GameObject _liftStopButton;

    [Space] [Header("Transforms")] [SerializeField]
    private RectTransform _floorsContent;

    [SerializeField] private RectTransform _liftContent;

    [Header("Game setups")] [Space] [SerializeField]
    private Slider _floorsNumberSlider;

    [SerializeField] private Button _startButton;
    [SerializeField] private GameObject _floorsHolder;
    [SerializeField] private GameObject _liftHolder;
    [SerializeField] private Text _floorsCountText;
    [SerializeField] private GameObject _setupsObjects;

    private int _floorsAmount = 20;
    private LiftController _liftController;
    private FloorsController _floorsController;

    private void Start()
    {
        SetFloorsAmount(_floorsNumberSlider.value);
        _floorsNumberSlider.onValueChanged.AddListener(SetFloorsAmount);
        _startButton.onClick.AddListener(StartGame);
    }

    private void SetFloorsAmount(float floorsAmount)
    {
        _floorsAmount = (int) floorsAmount;
        _floorsCountText.text = CURRENT_FLOORS + floorsAmount;
    }

    private void StartGame()
    {
        _liftHolder.SetActive(true);
        _floorsHolder.SetActive(true);
        _setupsObjects.SetActive(false);
        
        _liftController = FindObjectOfType<LiftController>();
        _floorsController = FindObjectOfType<FloorsController>();
        
        InitializeFloors();
        InitializeLiftButtons();
    }

    private void InitializeFloors()
    {
        for (int i = 0; i < _floorsAmount; i++)
        {
            var floorView = Instantiate(_floor, _floorsContent).GetComponent<FloorView>();
            floorView.Init(i, _floorsAmount - 1);
            _floorsController.AddNewFloorView(floorView);
        }
    }

    private void InitializeLiftButtons()
    {
        var stopButton = Instantiate(_liftStopButton, _liftContent).GetComponent<StopButtonView>();
        _liftController.AddStopButton(stopButton);

        for (int i = 0; i < _floorsAmount; i++)
        {
            var floorView = Instantiate(_liftButton, _liftContent).GetComponent<LiftButtonView>();
            floorView.Init(i, _liftController);
            _liftController.AddNewLiftButton(floorView);
        }
    }
}