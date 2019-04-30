using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftController : MonoBehaviour, ILiftFloorChanged
{
    public Action<int> FloorChanged { get; set; }
    
    private List<LiftButtonView> _liftButtonViews = new List<LiftButtonView>();
    private DirectionType _currentDirectionType;
    private int _currentFloor;
        
    public DirectionType CurrentDirectionType
    {
        get { return _currentDirectionType; }
    }

    public void StopLift()
    {
        if (FloorChanged != null)
        {
            FloorChanged(_currentFloor);
        }
    }
}
