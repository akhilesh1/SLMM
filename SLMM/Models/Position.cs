namespace SLMM.Models
{
    public enum OrientationEnum
    {
        North,
        East,
        South,
        West   
    }
    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public OrientationEnum Orientation { get; set; }
    }
}
