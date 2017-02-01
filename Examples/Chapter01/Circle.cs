using static System.Math;
public class Circle
{
   public Circle(double radius) { Radius = radius; }

   public double Radius { get; }

   public (double Circumference, double Area) Stats
      => (Circumference, Area);

   public double Circumference 
      => PI * 2 * Radius;
   
   public double Area
   {
      get
      {
         double Square(double d) => Pow(d, 2);
         return PI * Square(Radius);
      }
   }
}