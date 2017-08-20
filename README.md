# SIMPOL-Interpreter
IS214 Project: An Interpreter for the SIMPOL language

**DISCLAIMER: This project was submitted on November 2016 in fulfillment of IS214: Principles of Programming Languages course of the University of the Philippines Open University (UPOU). Use and modification of the application/source code must be for research and academic purposes only. Do not use this for purposes other than stated. Proper citation of this program, code, and the included document must be included in articles. I will not be liable for any damages caused by using this program.**

This interpreter is programmed in **C#** and is designed to interpret the SIMPOL language. SIMPOL is a *simple* language that only has two blocks: the `variable` block that stores all variables of a program; and the `code` block that holds all operations. All operations use the prefix or Polish (PN) notation. The SIMPOL source code must be saved with the .SIMPOL file extension.

When using the interpreter you will be greeted with a window with a Browse button that you will use to open the SIMPOL code, along with a textbox below it that will display the code of the source code as well as outputs from any operation. On the right will be two gridviews that will display the tokens or lexemes as well as the variables and their values.

Below are summarized grammar specifications for all operations of the SIMPOL language. Full documentation of the interpreter and the language can be viewed here: https://1drv.ms/w/s!AuaGlbRQ1zi4gbUabiSlDlkVdlnMkQ. A compiled version will be coming soon.

## Variables
Variables must start with a letter and can be followed by any alphanumeric character:
* `INT age`
* `STG name1`
* `BLN isHuman`

## User Input and Printing
User inputs and printing are supported with `ASK` and `PRT` keywords respectively. Supported input include:
* non-negative integers
* strings (must be enclosed in dollar signs: `$This is an example string$`)
* Boolean (`true` or `false`)

A variable must be declared in order to ask for user input (e.g. `ASK age`, `ASK name1`). The print statement will simply print the value of a variable or the output of a statement (e.g. `PRT isHuman`, `PRT ADD 1 1`).

## Arithmetic Operations
The language supports arithmetic operations with `ADD`, `SUB`, `MUL`, `DIV`, and `MOD` keywords. Examples are:
* `ADD 5 4` *= 9*
* `SUB 3 MUL 2 5` *= -7*
* `DIV ADD 10 4 MOD 4 3` *= 14*

## Comparative Operations
Relational operations are supported as well with `GRT`, `GRE`, `LET`, `LEE`, `EQL` keywords. This can be used with arithmetic operations. Examples are:
* `EQL 10 10` *= true*
* `LEE 1 ADD 1 2` *= false*
* `GRT SUB 16 2 MUL 4 2` *= true*

## Boolean Operations
Logical operations are supported too with `AND`, `OHR`, `NON` keywords and can be used with relations operations. Examples are:
* `AND true true` *= true*
* `OHR false LET 4 8` *= true*
* `NON EQL 10 ADD 5 5` *= false*

## Assignment Operations
Lastly, the assignment operation can be done with the `PUT` keyword followed by an operation or a value, then the `IN` keyword, and then the variable name. Examples are:
* `PUT $Ovuvuevuevue Enyetuenwuevue Ugbemugbem Osas$ IN name`
* `PUT SUB 2016 1994 IN age`
* `PUT NON false IN isHuman`

## Sample Program
Here's a program that will ask for a name then prints it along with the phrase "Hello there ":
```
variable {
STG name
}
code {
ASK name    
PRT $Hello there $
PRT name
}
```

Hope this project helped you in understanding interpreters and guide you in making your own. Thank you for visiting! 
