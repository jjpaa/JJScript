# JJScript
Toy script language built on top of .NET

# Features

## comment
- Line after "//" is skipped. Can be used as comment
- Usage example
	- // This is comment

## println
- Prints line to console.
	- Supports inline variable strigs
- Usage example
	- printl("Hello World");
	- println(myStringName);

## Variables

### String
- Stores string into string variable
- Usage example
	- string myString = "Hello!";
- Bug: Strings can't have whitespaces
	- Design flaw in the line/input handling