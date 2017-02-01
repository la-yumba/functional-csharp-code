using System;
using LaYumba.Functional;
using LaYumba.Functional.Data.BinaryTree;

// adapted from Brian Beckman: The Zen of Stateless State
// https://channel9.msdn.com/Shows/Going+Deep/Brian-Beckman-The-Zen-of-Expressing-State-The-State-Monad

namespace Examples.StateEx
{
   using static Tree;
   using static StatefulComputation<int>;
   using static F;

   // A tree containing data of type "a" is either a Leaf
   // containing an instance of type a, "Leaf a", or a Branch
   // containing two trees recursively containing data of type a:
   //
   // > data Tree a = Leaf a | Branch (Tree a) (Tree a)
   // >  deriving Show

   public class State_Number_Tree
   {
      // Our C# translation of the labeled tree, this bit of Haskell
      //
      // "Lt" stands for "Labeled tree," and it's just an ordinary
      // tree, as defined above, but containing a pair of a variable
      // of type S for state and a variable of type a, where S, the
      // type of state, is a just an Int.
      // 
      // > type Lt a = (Tree (S, a))
      // > type S = Int  -- Count, or "S", is just an Int
      //
      // The Count is the stateful bit we thread through the
      // labeling machinery.
      //
      // In another exercise, make this completely general,
      // generalizing over the type of the state variable with
      // another generic type parameter. For now, a state is an
      // integer, pure and simple, and a Count is a state.

      #region "non-monadically labeled tree"

      // The plan is to convert trees containing "content" data into
      // trees containing pairs of contents and labels.

      // In Haskell, just construct the type "pair of state and
      // contents" on-the-fly as a pair-tuple of type (S, a). In
      // C#, create a class such pairs since tuples are not
      // primitive as they are in Haskell.

      // The first thing we need is a class or type for
      // state-content pairs, call it Numbered.  Since the type of the
      // state is hard-coded as "Int," Numbered<a> has only one type
      // parameter, the type a of its contents.


      // Here's the Haskell for labeling a tree by manually
      // threading state through function arguments. Later, we
      // derive a generic state monad by abstracting parts of this
      // definition.
      //
      // Reminder: a labeled tree, Lt<a>, is a tree with an Numbered<a>
      // as its contents. The following function, Count, takes a 
      // Tree a and returns a Lt a = Tree (S, a), by calling a helper
      // function, Count, keeping only the second element of the
      // tuple that Count returns:
      //
      // > Count :: Tree a -> Lt a
      // > Count tr = snd (lab tr 0)
      // >  where ...

      // Here is our C# manual-labeling function, Count, which takes
      // a Tree<a> as input and returns a Tree<Numbered<a>> (for which we
      // have a new type in the Haskell version.) All this does is
      // call the helper function with a starting value for the
      // labels, namely 0, and then keep only the labeled-tree part
      // of the return value, which has both a Count and a labeled
      // tree. Internally, Count threads the Count part of its return
      // value to recursive calls of Count, but Count does not need
      // this value, even though it happens to be the value of the
      // next Count that would be applied to a tree node..


      // Count's helper function threads the Count (i.e., the state)
      // around.  It's easiest to create a new data structure to
      // hold a pair of a current Count and a partially labeled
      // Tree<Numbered<a>> = Lt a because we build up the fully labeled tree
      // recursively.

      // "Count" takes an old tree and a state value, and returns a
      // pair of state and new tree, which is, itself, a tree of
      // pairs:

      // >   lab :: Tree a -> S -> (S, Lt a)
      // >   lab (Leaf contents) n = ((n+1), (Leaf (n, contents))) -- returned pair
      // >   lab (Branch trs) n0     = let (n1, l') = lab l n0  -- pat match in
      // >                             (n2, r') = lab r n1  -- recurive calls
      // >                         in  (n2, Branch l' r')       -- returned pair

      // Direct transcription into C#:

      private class TreeNumbering1
      {
         public Tree<Numbered<T>> Number<T>(Tree<T> tree)
            => Number(tree, 0).Tree;

         public (Tree<Numbered<T>> Tree, int Counter) Number<T>(Tree<T> tree, int count)
            => tree.Match(
               Leaf: t => 
                  (
                     Leaf(new Numbered<T>(t, Number: count)), // use the current count for this leaf
                     count + 1 // pass on the incremented count
                  ),
               Branch: (left, right) =>
               {
                  var (newLeft, count1) = Number(left, count); // recursive call
                  var (newRight, count2) = Number(right, count1); // recursive call / use the count returned from the previous call
                  return (Branch(newLeft, newRight), count2); // pass on the updated count
               });
      }

      #endregion // non-monadically labeled tree

      #region "monadically labeled tree"

      // A "S2Scp" is a function from state to state-contents pair
      // (or Count-contents pair). An instance of the state monad
      // will have one member: a function of this type.  Where is
      // such a function to get the contents part?  Obviously not
      // from its argument list, therefore from the environment --
      // the closure about the function.

      // This is the generalization of the state monad type: it
      // doesn't care what type the contents are. The state monad
      // just says "if you give me a function from a state to a
      // state-contents pair, I'll thread the state around for you."

      public delegate (a, int) S2Scp<a>(int state);

      // Here is a type for functions that takes an input of type a
      // and puts it in an instance of the state monad containing an
      // instance of type b. In other words, it both transforms an a
      // to a b and lifts the b into the state monad.

      public delegate SM<b> Maker<a, b>(a input);

      // The following is actually general, aside from the hard-coding
      // of the type of Count as int. This is the type of a Count Monad
      // with contents of any type A.

      public class SM<a>
      {
         // Here is the meat: the only data member of this monad:

         public S2Scp<a> s2scp { get; set; }

         // Any monad -- the state monad, the continuation monad,
         // the list monad, the maybe monad, etc. must implement
         // the two operators @return and @bind, which we represent
         // here as instance methods.
         //
         //     (footnote: At-sign lets me use the "return" keyword
         //     as an identifier, and it's benign to use it on
         //     "bind" for stylistic and syntactic
         //     parallelism. These two operators are required and
         //     must satisfy the monad laws. Alternative: misspell
         //     "return" as in "retern" or what-not.)
         //
         // Exercise 3 asks you to create an abstract class with
         // these operators and to derive SM from that
         // class. Exercise 8 asks you to promote them into an
         // interface.

         // @return takes some contents as an argument and returns
         // an instance of the monad. For the state monad, this
         // instance contains, as required, a closure over a
         // function from state to state-contents pair.

         // @bind takes two arguments: an instance of monad M<a>,
         // something of type a already in the monad; and a Maker
         // function of type "from a to instance of M<b>". @bind
         // returns an instance of M<b>.  Imagine wrapping a call
         // of @bind(M<a>, a->M<b>) in a function that takes an
         // instance of type c, and see that @bind effects a
         // composition of a c->M<a> and an a->M<B> to create a
         // c->M<b>. Look up "Kleisli composition."

         // @return and @bind must satisfy the monad laws:
         //
         // Left-identity:
         //
         //     @bind(@return(anything), k)  ==  k(anything)
         //
         // Right-identity:
         //
         //     @bind(m, @return)  ==  m
         //
         // Associativity:
         //
         //     @bind(m, (x => @bind(k(x), h)))  ==
         //
         //     @bind(@bind(m, k), h)
         //
         // In exercise 7, verify the monad laws for this
         // implementation.

         // Here are the particular implementations of @return
         // and @bind for the state monad:

         // >  return contents = Labeled (\st -> (st, contents))

         // No wiggle room, here: put the contents in the contents
         // slot and put the state in the state slot. The new
         // state-monad instance contains a new function closed
         // over the contents, which are supplied in the argument
         // list of @return. The function is implemented as a C#
         // lambda expression:

         public static SM<a> @return(a contents)
         {
            return new SM<a>
            {
               s2scp = st => (contents, st)
            };
         }

         // ">>=" is the infix notation for "@bind" from
         // Haskell. Here, take an instance of monad x and a
         // function (x -> monad y) and returns an instance of
         // monad y. No wiggle room here, either: much easier
         // to show in pictures:
         //
         //                                +---------+
         //                            .-->|  fany1  |
         //                            |   +---------+
         //                            |        |
         //                            |        |
         //                            |        v
         //                            |       ===
         //         +--------+  any1   |   +---------+
         //         |        |---------'   |         |---------->
         //         |  fst0  |             |  fst1   |
         //   st0   |        |   st1       |         |
         // ------->|        |------------>|         |---------->
         //         +--------+             +---------+
         //
         // fst0 produces a state-contents pair. Feed the contents
         // produced by fst0 into fany1, which produces a function
         // from state to state-contents pair. Feed the state
         // produced by fst0 into the function produced by
         // fany1. Get a new state-contents pair. Make the whole
         // thing just a function from st0, and the final result is
         // a function from state to state-contents pair.
         // Recognize this as the signature of the final, resulting
         // monad instance. The pattern is also obvious chainable.
         //
         // >  M fst0 >>= fany1 = -- fst0 :: st->(st, any)
         // >   M $ \st0 ->   -- return new monad instance: a func of st0
         // >    let (st1, any1) = fst0 st0  -- pat match new st1 and contents
         // >        M fst1 = fany1 any1 -- shove contents into fany1,
         // >                            --  getting new monad inst fst1.
         // >    in fst1 st1  -- feed st1 into new monad inst, return (st, any)
         // >                 -- and that's what we needed, a function from
         // >                 -- st0 to (st->any) implemented through the new
         // >                 -- monad instance returned by fany1.

         public static SM<b> @bind<b>(SM<a> inputMonad, Maker<a, b> inputMaker)
         {
            return new SM<b>
            {
               // The new instance of the state monad is a
               // function from state to state-contents pair,
               // here realized as a C# lambda expression:

               s2scp = (st0 =>
               {
                  // Deconstruct the result of calling the input
                  // monad on the state parameter (done by
                  // pattern-matching in Haskell, by hand here):

                  var lcp1 = inputMonad.s2scp(st0);
                  var state1 = lcp1.Item2;
                  var contents1 = lcp1.Item1;

                  // Call the input maker on the contents from
                  // above and apply the resulting monad
                  // instance on the state from above:

                  return inputMaker(contents1).s2scp(state1);
               })
            };
         }
      }

      // Here's a particular state monad instance we need to update
      // state. We're going to @bind -- that is, compose -- an
      // instance of this with leaves of the labeled tree:

      // > updateState :: Labeled S
      // > updateState =  Labeled (\n -> ((n+1),n))

      private static SM<int> UpdateState()
      {
         return new SM<int>
         {
            s2scp = n => (n, n + 1)
         };
      }

      // Here's a helper that composes UpdateState with Leaf and
      // Branch in the original unlabeled tree. This looks very
      // hairy in C#, but in Haskell it's quite short. Here's what
      // we do with leaves:
      //
      // > mkm :: Tree anytype -> Labeled (Lt anytype)
      // > mkm (Leaf x)
      // >   = do n <- updateState  -- call updateState; "n" is of type "S"
      // >        return (Leaf (n,x)) -- "return" does the heavy lifting
      // >                          --  of creating the Monad from a value.
      //
      // The "do" notation is just Haskell syntactic sugar for
      // precisely the following call of @bind:
      //
      // updateState >>= \n -> return (Leaf (n, x))
      //
      // which says "call update state, which returns an instance of
      // the state monad, then @bind it to the variable n in a
      // function that returns a leaf node labeled by the given
      // state value." We translate this directly into C# below.
      // 
      // The Branch case is a trivial recursion. Notice that
      // updateState only gets called on leaves.
      //
      // > mkm (Branch l r)
      // >   = do l' <- mkm l
      // >        r' <- mkm r
      // >        return (Branch l' r')
      //
      // Notice this is private:

      private class TreeNumbering3 // intermediate step
      {
         private static StatefulComputation<int, int> GetAndIncrement 
            = count => (count, count + 1);

         public StatefulComputation<int, Tree<Numbered<T>>> Number<T>(Tree<T> tree)
            => tree.Match(

            Leaf: leaf =>
               from count in GetAndIncrement // put the current counter value into count, while incrementing the count
               select Leaf(new Numbered<T>(leaf, count)), // the leaf value, numbered with the current count value

            Branch: (left, right) =>
               from newLeft in Number(left)
               from newRight in Number(right)
               select Branch(newLeft, newRight));
      }

      private class TreeNumbering4
      {
         public StatefulComputation<int, Tree<Numbered<T>>> NumberTree<T>(Tree<T> tree)
            => tree.Match(
               Leaf: leaf =>
                  from count in Get // extract the current value of the count
                  from _     in Put(count + 1) // set the state to the incremented counter
                  select Leaf(new Numbered<T>(leaf, count)), // the leaf value, numbered with the current count value
               Branch: (left, right) =>
                  from newLeft in NumberTree(left)
                  from newRight in NumberTree(right)
                  select Branch(newLeft, newRight));
      }

      #endregion // "monadically labeled tree"

      static void _main(string[] args)
      {
         Console.WriteLine("Unlabeled Tree:");
         var t = Branch
         (
            Left: Leaf("a"),
            Right: Branch
            (
               Left: Branch
               (
                  Left: Leaf("b"),
                  Right: Leaf("c")
               ),
               Right: Leaf("d")
            )
         );
         Console.WriteLine(t);

         Console.WriteLine();
         Console.WriteLine("Non-monadically Labeled Tree:");
         var t1 = new TreeNumbering1().Number<string>(t, 0);
         Console.WriteLine(t1.Item1);

         Console.WriteLine();
         Console.WriteLine("LINQ Labeled Tree:");
         var t3 = new TreeNumbering3().Number(t)(0).Value;
         Console.WriteLine(t3);

         Console.WriteLine();
         Console.WriteLine("State get/set Labeled Tree:");
         var t4 = new TreeNumbering4().NumberTree(t)(0).Value;
         Console.WriteLine(t4);

         Console.WriteLine();
         Console.ReadKey();
      }

      // Exercise 1: generalize over the type of the state, from int
      // to <S>, say, so that the SM type can handle any kind of
      // state object. Start with Numbered<T> --> Numbered<S, T>, from
      // "Count-content pair" to "state-content pair".

      // Exercise 2: go from labeling a tree to doing a constrained
      // container computation, as in WPF. Give everything a
      // bounding box, and size subtrees to fit inside their
      // parents, recursively.

      // Exercise 3: promote @return and @bind into an abstract
      // class "M" and make "SM" a subclass of that.

      // Exercise 4 (HARD): go from binary tree to n-ary tree.

      // Exercise 5: Abstract from n-ary tree to IEnumerable; do
      // everything in LINQ! (Hint: SelectMany).

      // Exercise 6: Go look up monadic parser combinators and
      // implement an elegant parser library on top of your new
      // state monad in LINQ.

      // Exercise 7: Verify the Monad laws, either abstractly
      // (pencil and paper), or mechnically, via a program, for the
      // state monad.

      // Exercise 8: Design an interface for the operators @return
      // and @bind and rewrite the state monad so that it implements
      // this interface. See if you can enforce the monad laws
      // (associativity of @bind, Left identity of @return, Right
      // identity of @return) in the interface implementation.

      // Exercise 9: Look up the List Monad and implement it so that
      // it implements the same interface.

      // Exercise 10: deconstruct this entire example by using
      // destructive updates (assignment) in a discipline way that
      // treats the entire CLR and heap memory as an "ambient
      // monad." Identify the @return and @bind operators in this
      // monad, implement them explicitly both as virtual methods
      // and as interface methods.
   }
}
