# JJScript
Toy script language built on top of .NET

# Features

## comment
- Line after "//" is skipped. Can be used as comment

```c#
// This is comment
```

## println
- Prints line to console.
- Supports inline variable strings

```c#
printl("Hello World");
println(myStringName);
```

## Variables

### String
- Stores string into string variable
- Bug: Strings can't have whitespaces
	- Design flaw in the line/input handling
```c#
string myString = "Hello!";
```

## Example code

```c#

// This is comment
// int x = 10;

printl("Hello World");
printl("Hello World2");

printl ("hello");

printl ( "Hello");
printl ( "Hell o");
printl ( " Hel l o " ) ;

string myString = "My String Value, Stored into variable";
printl(myString);

string myString = "Let's change it";
printl(myString);

```

### Output

```
Hello World
Hello World2
hello
Hello
Hell o
 Hel l o
MyStringValue,Storedintovariable
Let'schangeit
Program ended
```