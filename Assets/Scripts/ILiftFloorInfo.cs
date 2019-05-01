using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILiftFloorInfo
{
    Action<int> FloorChanged { get; set; }
    Action<int> LiftStopOnTheFloor { get; set; }
    int CurrentFloor { get; }
}
