using System;
using LaYumba.Functional;
using static System.Console;

namespace Playground.Free
{
   // AST
   public interface Expr { }
   public class Const : Expr { public int value; }
   public class Add : Expr { public Expr op1; public Expr op2; }
   public class Subtract : Expr { public Expr op1; public Expr op2; }

   // interpreters - gives meaning to the AST
   public static class EvaluatingInterpreter
   {
      public static int Eval(Expr expr) => new Pattern<int>
      {
         (Const @const) => @const.value,
         (Add add) => Eval(add.op1) + Eval(add.op2),
         (Subtract sub) => Eval(sub.op1) - Eval(sub.op2),
      }.Match(expr);
   }

   public static class ToStringInterpreter
   {
      public static string Eval(Expr expr) => new Pattern<string>
      {
         (Const @const) => @const.value.ToString(),
         (Add add) => $"{Eval(add.op1)} + {Eval(add.op2)}",
         (Subtract sub) => $"{Eval(sub.op1)} + {Eval(sub.op2)}",
      }.Match(expr);
   }

   public static class ToStringEvaluatingInterpreter
   {
      public static string Eval(Expr expr) => new Pattern<string>
      {
         (Const @const) => ToStringInterpreter.Eval(@const),
         (Add add) => $"{ToStringInterpreter.Eval(add)} = {EvaluatingInterpreter.Eval(add)}",
         (Subtract sub) => $"{ToStringInterpreter.Eval(sub)} = {EvaluatingInterpreter.Eval(sub)}",
      }.Match(expr);
   }

   // run the thing
   public static class Arithmethic
   {
      public static Expr Const(int i) => new Const { value = i };
      public static Expr Add(Expr a, Expr b) => new Add { op1 = a, op2 = b };
      public static void _main()
      {
         var expr1 = new Add { op1 = Const(1), op2 = Const(2) };
         WriteLine(EvaluatingInterpreter.Eval(expr1));
         WriteLine(ToStringInterpreter.Eval(expr1));
         WriteLine(ToStringEvaluatingInterpreter.Eval(expr1));

         WriteLine();
         
         var expr2 = Add(Const(1), Add(Const(44), Const(22)));
         WriteLine(EvaluatingInterpreter.Eval(expr2));
         WriteLine(ToStringInterpreter.Eval(expr2));
         WriteLine(ToStringEvaluatingInterpreter.Eval(expr2));
      }
   }
}
