namespace Examples
{
   public class Numbered<T>
   {
      public Numbered(T Value, int Number)
      {
         this.Value = Value;
         this.Number = Number;
      }

      public int Number { get; set; }
      public T Value { get; set; }

      public override string ToString()
         => $"({Number}, {Value})";

      public static Numbered<T> Create(T Value, int Number)
         => new Numbered<T>(Value, Number);
   }
}