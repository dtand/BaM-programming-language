//
//  LexicalAnalyzer.cpp
//  BaMInterpreter
//
//  Created by Daniel Anderson on 10/23/15.
//  Copyright (c) 2015 Daniel Anderson. All rights reserved.
//

#include "LexicalAnalyzer.h"
LexicalAnalyzer::LexicalAnalyzer()
{
    code = userInput;
    pos = 0;
    posState = pos;
}
void LexicalAnalyzer::getChar()
{
    
    
    //Store next character in nextChar
    nextChar = code[pos];
    pos++;
    
    //Check if character is alpha and set class
    if( isalpha(nextChar) )
    {
        charClass = LETTER;
    }
    
    //Chack if character is digit and set class
    else if( isdigit(nextChar) )
    {
        charClass = DIGIT;
    }
    
    //Assume character is unknown
    else
    {
        charClass = UNKNOWN;
    }
    
}

void LexicalAnalyzer::addChar()
{
    if(lexLen <= 98)
    {
        lexeme[lexLen++] = nextChar;
        lexeme[lexLen] = 0;
    }
    else
    {
        std::cout << "Error - lexeme is too long /n";
    }
    
    
}

//Evaluates a lexeme and returns the corresponding next token
int LexicalAnalyzer::lex()
{
    
    //Set the length and get non blank space
    lexLen = 0;
    getNoneBlank();
    
    //Check Char Classes
    switch (charClass)
    {
            //We are parsing an identifier or function
        case LETTER:
            addChar();
            getChar();
            
            //Criteria for an identifier or function
            while ( charClass == LETTER || charClass == DIGIT )
            {
                addChar();
                getChar();
            }
            
            //Decide key word, or default to ident
            nextToken = decideKeyWord();
            
            break;
            
            //Parsing a number
        case DIGIT:
            addChar();
            getChar();
            
            //Parse a numeric value
            while(charClass == DIGIT)
            {
                addChar();
                getChar();
            }
            
            nextToken = INT_LIT;
            break;
            
            //Parse parenthesis and operators
        case UNKNOWN:
            lookup(nextChar);
            getChar();
            break;
    }
    
    //Output information for each lexical analysis
    //std::cout << "Next token is: " << nextToken << ", Next lexeme is: " << lexeme << std::endl;
    
    //Return the next token
    return nextToken;
}

//Looks up corresponding token given some lexeme
//returns that token
int LexicalAnalyzer::lookup(char ch)
{
    
    //Evaluates the lexeme and sets the next
    //token to the corresponding token
    switch (ch) {
        case '(':
            addChar();
            nextToken = LEFT_PAREN;
            break;
            
        case ')':
            addChar();
            nextToken = RIGHT_PAREN;
            break;
            
        case '+':
            addChar();
            nextToken = ADD_OP;
            break;
            
        case '-':
            addChar();
            nextToken = SUB_OP;
            break;
            
        case '*':
            addChar();
            nextToken = MULT_OP;
            break;
            
        case '/':
            addChar();
            nextToken = DIV_OP;
            break;
            
        case '=':
            addChar();
            nextToken = ASSIGN_OP;
            break;
            
        case ';':
            addChar();
            nextToken = MULT_LINE_OP;
            break;
            
        case ',':
            addChar();
            nextToken = COMMA;
            break;
            
        case '\n':
            addChar();
            nextToken = NEW_LINE_CHAR;
            break;
            
        case '$':
            addChar();
            nextToken = END_INPUT_SYMBOL;
            break;
            
        default:
            addChar();
            nextToken = EOF;
            break;
    }
    
    return nextToken;
}

char LexicalAnalyzer::delimLookahead()
{
    
    int temp = pos;
    
    while (isspace(code[temp]))
    {
        temp++;
    }
    
    return code[temp];
}

char LexicalAnalyzer::decideKeyWord()
{
    if ( isIf() )
    {
        return IF_START;
    }
    else if( isFunDef() )
    {
        return FUNC_START;
    }
    else if( isEndfun() )
    {
        return FUNC_END;
    }
    else if( isEndIf() )
    {
        return IF_END;
    }
    
    return IDENT;
}

void LexicalAnalyzer::getNoneBlank()
{
    while(isspace(nextChar))
        getChar();
}

bool LexicalAnalyzer::isIf()
{
    return (lexeme[0] == 'i' &&
            lexeme[1] == 'f');
}

bool LexicalAnalyzer::isEndIf()
{
    return (lexeme[0] == 'e' &&
            lexeme[1] == 'n' &&
            lexeme[2] == 'd' &&
            lexeme[3] == 'i' &&
            lexeme[4] == 'f');
}

bool LexicalAnalyzer::isFunDef()
{
    return (lexeme[0] == 'f' &&
            lexeme[1] == 'u' &&
            lexeme[2] == 'n' &&
            lexeme[3] == 'd' &&
            lexeme[4] == 'e' &&
            lexeme[5] == 'f');
}

bool LexicalAnalyzer::isEndfun()
{
    return (lexeme[0] == 'e' &&
            lexeme[1] == 'n' &&
            lexeme[2] == 'd' &&
            lexeme[3] == 'f' &&
            lexeme[4] == 'u' &&
            lexeme[5] == 'n');
}

//Switches pointer to different input
void LexicalAnalyzer::switchPointer(char *thisInput)
{
    pushCodeState();
    code = thisInput;
    pos = 0;
}

//Resets pointer to user input
void LexicalAnalyzer::resetPointerState()
{
    code = codeStateStack.top().code;
    pos = codeStateStack.top().pos;
    popCodeState();
}

void LexicalAnalyzer::pushCodeState()
{
    codeStateStack.push(CodeState(pos-1, code));
}

void LexicalAnalyzer::popCodeState()
{
    codeStateStack.pop();
}

