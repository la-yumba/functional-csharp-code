using static System.Math;
public class Circle
{
   public Circle(double radius) { Radius = radius; }
   public double Radius { get; }
   public double Area => PI * Pow(Radius, 2);
}