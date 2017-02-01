namespace Examples
{
   public class ConnectionString
   {
      string Value { get; }
      public ConnectionString(string value) { Value = value; }

      public static implicit operator string(ConnectionString c)
         => c.Value;
      public static implicit operator ConnectionString(string s)
         => new ConnectionString(s);

      public override string ToString() => Value;
   }
}
