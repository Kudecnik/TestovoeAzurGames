using System;

[Serializable]
public class LiftCallCommand
{
   public  DirectionType Direction;
   public  int Floor;

   public LiftCallCommand(int floor, DirectionType direction)
   {
      Floor = floor;
      Direction = direction;
   }
}
