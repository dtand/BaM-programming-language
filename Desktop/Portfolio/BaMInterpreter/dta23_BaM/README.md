To run:

1. open up terminal / command prompt and run the following commands
2. cd ../BaMInterpreter
3. make -f bam.make
4. ./bam

================================================
Utility Commands:

Additionally, the following utility commands when entered into the
interpreter may be helpful:

//All utility commands must be preceded by a ‘$’ and have no space after
//the dollar sign

1. $quit -> exits the interpreter
2. $global_table -> outputs all global functions and vars
3. $global_table.vars -> outputs all global vars
4. $global_table.functions -> outputs all functions in table

================================================
About BaM:

BaM is a simple and intutive programming language with a single data type (integer).  BaM uses a top down recurssive decent parser to both parse and evaluate user input.  BaM allows for function definitions and adheres to static/lexical scoping.  However, an if-block is joined with its parent scope, therefore only function invocations adhere to the scoping rules.  If-conditionals are evaluated as follows: values <= 0 are false, values > 0 are true.  BaM does not include any looping structures or other similar utilities. BaM does not allow parenthised expressions, and includes very minimal operators.

================================================
Operator Precedence (highest precedence on top):

*, /

+, -

================================================
Project goals:

The goal of this project is to implement a very simple and minimal purely translated programming language using a recurssive decent parser.  This is a class project designed to excersize some of the core features of programming language design.


================================================
Other Notes:

* When bam.exe is ran it will most likely give several errors because
of c++ 11 extensions, but these are merely errors and do not affect
the capability of execution