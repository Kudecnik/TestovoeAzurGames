using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Brain : MonoBehaviour
{
    private List<FloorView> _floorsViews = new List<FloorView>();
    private LiftController _liftFloorChanged;

    private void Awake()
    {
        _liftFloorChanged.FloorChanged += OnFloorChanged;
    }
    
    public void AddNewFloorView(FloorView floorView)
    {
        _floorsViews.Add(floorView);
    }

    private void OnFloorChanged(int floor)
    {
        var floorView = GetFloorView(floor);
        
        if (floorView.DirectionRequest != DirectionType.None && floorView.DirectionRequest == _liftFloorChanged.CurrentDirectionType)
        {
            _liftFloorChanged.StopLift();
        }
    }

    private FloorView GetFloorView(int floor)
    {
        var view = _floorsViews.FirstOrDefault(p => p.FloorNumber == floor);

        if (view == null)
        {
            Debug.LogErrorFormat("Does not find view with floor == {0}",floor);
        }

        return view;
    }
}
