public enum DirectionType
{
    Up,
    Down,
    None,
}

public static class DirectionTypeExtensions
{
    public static DirectionType OppositeDirections(this DirectionType directionType)
    {
        switch (directionType)
        {
            case DirectionType.Up:
                return DirectionType.Down;
            case DirectionType.Down:
                return DirectionType.Up;
            default:
                return DirectionType.None;
        }
    }
}