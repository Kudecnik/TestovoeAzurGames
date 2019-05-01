using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LiftController : MonoBehaviour, ILiftFloorInfo
{
    [SerializeField] private float _liftStopTime = 1f;
    [SerializeField] private float _liftFloorReachTime = 1f;
    [SerializeField] private LiftUiView _liftUiView;

    private readonly List<LiftButtonView> _liftButtonViews = new List<LiftButtonView>();
    private readonly List<LiftCallCommand> _liftCallCommands = new List<LiftCallCommand>();
    private StopButtonView _stopButtonView;
    private DirectionType _currentDirectionType = DirectionType.None;
    private int _currentFloor;
    private Coroutine _liftMoveCoroutine;
    private Coroutine _liftStopCoroutine;
    private bool _isLiftStopped;

    public Action<int> FloorChanged { get; set; }
    public Action<int> LiftStopOnTheFloor { get; set; }

    public int CurrentFloor
    {
        get { return _currentFloor; }
    }

    public DirectionType CurrentDirectionType
    {
        get { return _currentDirectionType; }
    }

    private void Start()
    {
        UpdateLiftState(true);

        if (FloorChanged != null)
        {
            FloorChanged(_currentFloor);
        }
    }

    private void OnDisable()
    {
        foreach (var liftButtonView in _liftButtonViews)
        {
            liftButtonView.FloorSelected -= RequestFloor;
        }

        _stopButtonView.StopButtonClicked -= ForceStop;
    }

    public void AddNewLiftButton(LiftButtonView liftButtonView)
    {
        _liftButtonViews.Add(liftButtonView);

        liftButtonView.FloorSelected += RequestFloor;
    }

    public void AddStopButton(StopButtonView stopButtonView)
    {
        _stopButtonView = stopButtonView;
        stopButtonView.StopButtonClicked += ForceStop;
    }

    public void StopLift()
    {
        StopLiftCoroutines();

        if (LiftStopOnTheFloor != null)
        {
            LiftStopOnTheFloor(_currentFloor);
        }

        _liftStopCoroutine = StartCoroutine(LiftStop());
    }

    public void RequestFloor(LiftCallCommand liftCallCommand)
    {
        var direction = liftCallCommand.Floor > _currentFloor ? DirectionType.Up : DirectionType.Down;
        
        Request(liftCallCommand, direction);
    }

    private void RequestFloor(int floor)
    {
        var direction = floor > _currentFloor ? DirectionType.Up : DirectionType.Down;
        var currentRequest = new LiftCallCommand(floor, direction);

        CheckActiveRequests();
        
        Request(currentRequest, direction);
    }

    private void Request(LiftCallCommand liftCallCommand, DirectionType directionType)
    {
        _liftCallCommands.Add(liftCallCommand);

        if (_currentDirectionType == DirectionType.None)
        {
            _currentDirectionType = directionType;
            MoveLift();
        }
    }
    
    private void ForceStop()
    {
        StopLiftCoroutines();

        UpdateLiftState(true);

        _isLiftStopped = true;
    }

    private void StopLiftCoroutines()
    {
        if (_liftMoveCoroutine != null)
        {
            StopCoroutine(_liftMoveCoroutine);
        }

        if (_liftStopCoroutine != null)
        {
            StopCoroutine(_liftStopCoroutine);
        }
    }

    private void MoveLift()
    {
        StopLiftCoroutines();

        _isLiftStopped = false;
        _liftMoveCoroutine = StartCoroutine(LiftMove());
    }

    private IEnumerator LiftMove()
    {
        UpdateLiftState(true);
        yield return new WaitForSeconds(_liftFloorReachTime);
        FloorReached();
    }

    private IEnumerator LiftStop()
    {
        UpdateLiftState(false);
        yield return new WaitForSeconds(_liftStopTime);
        CheckActiveRequests();
    }

    private void FloorReached()
    {
        _currentFloor += _currentDirectionType == DirectionType.Up ? 1 : -1;

        if (FloorChanged != null)
        {
            FloorChanged(_currentFloor);
        }

        var floorRequest = _liftCallCommands.FirstOrDefault(p => p.Floor == _currentFloor);

        if (floorRequest == null)
        {
            MoveLift();
        }
        else
        {
            if (floorRequest.Direction == _currentDirectionType || !HasSameDirectionRequests())
            {
                RemoveAllSameRequests(floorRequest.Floor);
                StopLift();
            }
            else
            {
                MoveLift();
            }
        }
    }

    private void RemoveAllSameRequests(int floor)
    {
        var requests = _liftCallCommands.FindAll(p => p.Floor == _currentFloor);

        foreach (var request in requests)
        {
            _liftCallCommands.Remove(request);
        }
    }

    private bool HasSameDirectionRequests()
    {
        return _liftCallCommands.FirstOrDefault(p =>
                   p.Direction == _currentDirectionType && IsFloorOnTheSameDirection(p.Floor)) != null;
    }

    private void CheckActiveRequests()
    {
        if (_liftCallCommands.Count == 0)
        {
            _currentDirectionType = DirectionType.None;
            UpdateLiftState(true);
            return;
        }

        var sameDirectionRequest = _liftCallCommands.FirstOrDefault(p =>
            p.Direction == _currentDirectionType && IsFloorOnTheSameDirection(p.Floor));

        if (sameDirectionRequest != null)
        {
            MoveLift();
        }
        else
        {
            var sameDirectionFloor = _liftCallCommands.FirstOrDefault(p => IsFloorOnTheSameDirection(p.Floor));

            if (sameDirectionFloor != null)
            {
                MoveLift();
            }
            else
            {
                _currentDirectionType = _currentDirectionType.OppositeDirections();
                MoveLift();
            }
        }
    }

    private bool IsFloorOnTheSameDirection(int floor)
    {
        switch (_currentDirectionType)
        {
            case DirectionType.Up:
                return floor > _currentFloor;
            case DirectionType.Down:
                return floor < _currentFloor;
            default:
                return true;
        }
    }

    private void UpdateLiftState(bool isDoorClosed)
    {
        _liftUiView.UpdateView(isDoorClosed, _currentFloor, _currentDirectionType);
    }
}