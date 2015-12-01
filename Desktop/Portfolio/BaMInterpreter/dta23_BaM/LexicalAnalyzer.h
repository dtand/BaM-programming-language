//
//  LexicalAnalyzer.h
//  BaMInterpreter
//
//  Created by Daniel Anderson on 10/23/15.
//  Copyright (c) 2015 Daniel Anderson. All rights reserved.
//

#ifndef __BaMInterpreter__LexicalAnalyzer__
#define __BaMInterpreter__LexicalAnalyzer__

#include <stdio.h>
#include <iostream>
#include <cctype>
#include <stack>
#include "ActivationRecord.h"

/*
    Analyzer class that takes care of token recognition
    This class performs all functionality to recognize
    particular tokens given some lexeme, also identifies
    particular keywords
 
 */


//Declare class in order to friend it
class BaMParser;
class BaMInterpreter;

//Enumeration classifications for different
//character types
enum CHAR_CLASS {LETTER, DIGIT, UNKNOWN};


//Defines token classifications
enum TOKEN_TYPE {INT_LIT, IDENT, ASSIGN_OP, ADD_OP, SUB_OP, MULT_OP, DIV_OP,
    LEFT_PAREN, RIGHT_PAREN, FUNCTION, IF_START, IF_END, FUNC_START,
    FUNC_END, MULT_LINE_OP, NEW_LINE_CHAR, COMMA, END_INPUT_SYMBOL};

//Struct for stack of code states this will help
//in implementing multiple function invocations
struct CodeState
{
    int pos;
    char *code;
    
    CodeState(int pPos, char* pCode) : pos(pPos), code(pCode) {}
};

class LexicalAnalyzer
{
    //Friends
    friend BaMParser;
    friend BaMInterpreter;
    
public:
    LexicalAnalyzer();
    
    //Switches pointer to different input
    void switchPointer(char *thisInput);
    
    //Resets pointer to user input
    void resetPointerState();

    //Gets the next char from the input buffer
    void getChar();
    
    //Adds a character to lexeme[]
    void addChar();
    
    //Lexer function to parse next lexeme
    int lex();
    
    //Used to decide unknown lexemes
    int lookup(char c);

    //Decides if input is a keyword and
    //ascribes appropriate token
    char decideKeyWord();
    
    //Boolean functions to check for keywords
    bool isIf();
    bool isEndIf();
    bool isFunDef();
    bool isEndfun();
    
    void getNoneBlank();
    char delimLookahead();
    
    //Controls stack of code states
    void pushCodeState();
    void popCodeState();
    
private:
    
    std::stack<CodeState> codeStateStack;
    
    char* code; //pointer to the code, this is useful
                //when changing execution to function code
    
    //Private members for keeping track of lexing credentials
    enum CHAR_CLASS charClass;
    enum TOKEN_TYPE tokenType;
    char lexeme [100];
    char nextChar;
    int lexLen;
    int token;
    int nextToken;
    int pos;
    int posState;
    
    //User input handling
    char userInput[40000];
    unsigned inputLength;
    
};

#endif /* defined(__BaMInterpreter__LexicalAnalyzer__) */
