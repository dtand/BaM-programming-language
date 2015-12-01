//
//  BaMInterpreter.h
//  BaMInterpreter
//
//  Created by Daniel Anderson on 10/23/15.
//  Copyright (c) 2015 Daniel Anderson. All rights reserved.
//

#ifndef __BaMInterpreter__BaMInterpreter__
#define __BaMInterpreter__BaMInterpreter__

#include <stdio.h>
#include "BaMParser.h"

class BaMInterpreter
{
private:
    //Constant strings
    const char* PROMPT = "BaM_interpreter> ";
    const char* PROMPT2 = "> ";
    const char* END_FUN = "endfun";
    const char* QUIT = "$quit";
    const char* SHOW_GLOBAL = "$global_table";
    const char* SHOW_GLOBAL_VARS = "$global_table.vars";
    const char* SHOW_GLOBAL_FUNCS = "$global_table.functions";
    
    //Symbols
    const char COMMENT_CHAR = '%';
    const char EOF_SYMBOL = '$';
    
    //Input handling variables
    int pos;
    unsigned inputLength;
    
    //Parser for Interpreter
    BaMParser *parser;
    LexicalAnalyzer *lexer;
    
    void _parseInput(); //Internal function to parse input
    void _resetInput(); //Internal function to reset input
    bool running;       //Keeps track if program is running
    
public:
    
    //Contructor and destructor
    BaMInterpreter();
    ~BaMInterpreter();
    
    //Calls all reader functions to begin functionality
    void begin();
    
    //Reads in all input from user
    bool readInput();
    
    //Gets next character from input adds to input array
    void nextChar(char &c);
    
    //Helper functions for holding input buffer
    void readParens();
    void readFunctionDef();
    void waitEndIf();
    int matches(int s, int leng, const char* nm);
    int isDelim(char c);
    bool addSemiColon();
    bool handleUtilities();
    
};

#endif /* defined(__BaMInterpreter__BaMInterpreter__) */
