using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILiftFloorChanged
{
    Action<int> FloorChanged { get; set; }
}
