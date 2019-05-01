using UnityEngine;
using UnityEngine.UI;

public class LiftUiView : MonoBehaviour
{
    private const string DOOR_OPEN = "Открыто";
    private const string DOOR_CLOSE = "Закрыто";
    private const string FLOOR_TEXT = "Этаж :";
    
    [SerializeField] private Text _openCloseText;
    [SerializeField] private Text _floorNumberText;
    [SerializeField] private GameObject _moveUpIndicator;
    [SerializeField] private GameObject _moveDownIndicator;

    public void UpdateView(bool isDoorClosed, int floorIndex, DirectionType directionType)
    {
        _openCloseText.text = isDoorClosed ? DOOR_CLOSE : DOOR_OPEN;
        _floorNumberText.text = FLOOR_TEXT + floorIndex;
        
        _moveUpIndicator.SetActive(directionType == DirectionType.Up);
        _moveDownIndicator.SetActive(directionType == DirectionType.Down);
    }
}
