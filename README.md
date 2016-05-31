# Functional Programming in C# #

This repo contains the code samples, exercises and solutions for the book 
[Functional Programming in C#](https://www.manning.com/books/functional-programming-in-c-sharp?a_aid=functional-programming-in-c-sharp&a_bid=ad9af506) 
currently available as part of the Manning Early Access Program.

[![Functional Programming in C#](https://images.manning.com/255/340/resize/book/1/b287ae1-c2bd-45ec-aa1b-deafdf104773/FPinCSharp_hires.png)](https://www.manning.com/books/functional-programming-in-c-sharp?a_aid=functional-programming-in-c-sharp&a_bid=ad9af506)

The code samples are organized in the following projects:

- **Boc**: a long-running example based on a banking scenario
- **Examples**: shorter examples, organized by chapter and topic
- **Exercises**: placeholders for you to do the exercises, compile and run them; 
  and compare to the provided solutions
- **LaYumba.Functional**: a functional library that we develop throughout the book
- **LaYumba.Functional.Tests**: also illustrative of topics explained in the book, and 
  useful to better understand the constructs in the library
- **TestRunner**: given NUnit's limited integration, this is required to run all the tests

**Note:** you are welcome to use `LaYumba.Functional`, but the main intent of this library
is pedagogical. For a more fully-fledged functional library, consider [language-ext](https://github.com/louthy/language-ext)

## Set-up

- install [.NET Core](https://www.microsoft.com/net/core)
- run `dotnet restore`

## Running the tests

```
$ cd src/TestRunner
$ dotnet run
```

## Doing the exercises

- edit the code in `src/Exercises` as needed
- edit `src/Exercises/Program.cs` to start the class or tests you want
- run it with:

  ```
  $ cd src/Exercises
  $ dotnet run
  ```
