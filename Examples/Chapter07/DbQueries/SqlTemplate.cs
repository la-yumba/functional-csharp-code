namespace Examples
{
   public class SqlTemplate
   {
      string Value { get; }
      public SqlTemplate(string value) { Value = value; }

      public static implicit operator string(SqlTemplate c)
         => c.Value;
      public static implicit operator SqlTemplate(string s)
         => new SqlTemplate(s);

      public override string ToString() => Value;
   }
}
