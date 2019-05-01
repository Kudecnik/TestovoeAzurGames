using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FloorsController : MonoBehaviour
{
    private LiftController _liftController;
    private readonly List<FloorView> _floorsViews = new List<FloorView>();

    private void Awake()
    {
        _liftController = FindObjectOfType<LiftController>();
    }

    private void OnEnable()
    {
        _liftController.FloorChanged += OnFloorChanged;
    }

    private void OnDisable()
    {
        _liftController.FloorChanged -= OnFloorChanged;
    }

    public void AddNewFloorView(FloorView floorView)
    {
        _floorsViews.Add(floorView);
        floorView.Init(_liftController);
        floorView.LiftCalled += RequestFloor;
    }

    private void RequestFloor(LiftCallCommand liftCallCommand)
    {
        _liftController.RequestFloor(liftCallCommand);
    }

    private void OnFloorChanged(int floor)
    {
        var floorView = GetFloorView(floor);

        if (floorView.DirectionRequest != DirectionType.None &&
            floorView.DirectionRequest == _liftController.CurrentDirectionType)
        {
            _liftController.StopLift();
        }
    }

    private FloorView GetFloorView(int floor)
    {
        var view = _floorsViews.FirstOrDefault(p => p.FloorNumber == floor);

        if (view == null)
        {
            Debug.LogErrorFormat("Does not find view with floor == {0}", floor);
        }

        return view;
    }
}