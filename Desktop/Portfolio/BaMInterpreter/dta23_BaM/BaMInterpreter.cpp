



#include "BaMInterpreter.h"
//Instantiate things
BaMInterpreter::BaMInterpreter()
{
    lexer = new LexicalAnalyzer();
    parser = new BaMParser(lexer);
    running = true;
}

//Delete things
BaMInterpreter::~BaMInterpreter()
{
    delete lexer;
    delete parser;
}

//Starts processing
void BaMInterpreter::begin()
{
    
    //Run process while interpreter is running
    while (running)
    {
        //Read input will terminate when complete, check for
        //exit code as well, if statement returns false, exit
        //has hit
        if(!readInput())
            break;
        
        //Set input length
        lexer->inputLength = inputLength;
        
        //Slap EOF at the end
        lexer->userInput[inputLength++] = ' ';
        lexer->userInput[inputLength++] = EOF_SYMBOL;
        
        
        //Call recursive descent parsing routine
        _parseInput();
        
        //Reset input
        _resetInput();
    }
    
}

void BaMInterpreter::_resetInput()
{
    pos = 0;
    inputLength = 0;
    lexer->inputLength = inputLength;
    lexer->pos = pos;
    
    //Fill first 10 with a blank to avoid strange issue of
    //arbritary characters being filled
    for(int i = 0; i < 10; i++)
    {
        lexer->userInput[i] = ' ';
    }
}

//Creates parser and runs parsing routine
void BaMInterpreter::_parseInput()
{
    parser->begin();
}

// reads input from console and stores in global userinput[]
bool BaMInterpreter::readInput()
{
    char  c;
    std::cout << PROMPT;
    pos = 0;
    
    do
    {
        pos++;
        
        nextChar(c);
        
        if (c == '\n')
            lexer->userInput[pos] = ' ';   // or space
        else
        {
            lexer->userInput[pos] = c;
        }
        
        if(lexer->userInput[pos] == '$')
        {
            if(handleUtilities())
            {
                running = false;
                return running;
            }
            
            _resetInput();
            std::cout << std::endl << PROMPT;
            pos = 0;
        }
        else
        {
        
        
            if (pos >= 6 && matches(pos - 6, 6, "fundef"))
            {
                readFunctionDef();      // keep reading until endfun token
                break;
            }
        
            else if(pos >= 2 && matches(pos - 2, 2, "if"))
            {
                waitEndIf();
                break;
            }
        
            if (lexer->userInput[pos] == '(')  // if pos >6 and matches(pos-1,6,"define")
                readParens();           // readDef  ... in def until read key word end
        }
        
    } while (c != '\n');
    
    //Set input length
    inputLength = pos;
    
    return true;
    
}

void BaMInterpreter::nextChar(char& c) // pulls next char
{
    scanf("%c", &c);
    
    if (c == COMMENT_CHAR)
    {
        while (c != '\n')
            scanf("%c", &c);
    }
}

//Lookahead checks input for matching string. Helps to check for keywords.
int BaMInterpreter::matches(int s, int leng, const char* nm)
{
    int i = 0;
    
    while (i < leng)
    {
        
        if (lexer->userInput[s] != nm[i])
            return 0;
        
        ++i;
        ++s;
    }
    
    if (!isDelim(lexer->userInput[s]))
        return 0;
    
    return 1;
    
}/* matches */

// checks char for particular delimiters
int BaMInterpreter::isDelim(char c)
{
    return ( (c == '(') || (c == ')')|| (c == ' ') || (c == COMMENT_CHAR) || (c == '\n') );
}


// assures that scanf waits until matching paren is input. Parsing will not begin until a match is found.
// This function is not necessary but allows for easy command line input
void BaMInterpreter::readParens()
{
    int parencnt; /* current depth of parentheses */
    char c = ' ';
    parencnt = 1; // '(' just read
    
    do
    {
        if (c == '\n')
        {
            std::cout << PROMPT2;
        }
        
        //Get next char, increment pos
        nextChar(c);
        pos++;
    
        // Insert newline character
        if (c == '\n')
            lexer->userInput[pos] = ' ';
        
        //Insert presented character
        else
            lexer->userInput[pos] = c;
        
        //Keep track of right parens
        if (c == '(')
            ++parencnt;
        
        //Keep track of left parens
        if (c == ')')
            parencnt--;
        
    } while (parencnt != 0);
    
} //readParens

// requires scanf to wait until keyword endfun is input
//parsing a fundef will not begin until keyword endfun
void BaMInterpreter::readFunctionDef()
{
    char c = ' ';
    int lines = 0;
    bool dirtyLittleFlag = false;
    
    do
    {
        if (c == '\n')
        {
            std::cout << PROMPT2;
            
            if ( (addSemiColon() && lines > 0) || (dirtyLittleFlag && lines > 0) )
            {
                lexer->userInput[pos] = ';';
                lexer->userInput[++pos] = ' ';
                
                if (dirtyLittleFlag)    dirtyLittleFlag = !dirtyLittleFlag;
            }
            
            lines++;
        }
        
        
        //Get next char, increment pos
        nextChar(c);
        pos++;
        
        //Add newline
        if (c == '\n')
            lexer->userInput[pos] = ' ';
        
        //Add presented character
        else
            lexer->userInput[pos] = c;
        
        
        //Remove list id from form last statement
        if(matches(pos - 6, 6, END_FUN))
        {
            if(lines > 0)
            {
                lexer->userInput[pos-8] = ' ';
            }
            
        }
        else if(matches(pos -2, 2, "if"))
        {
            waitEndIf();
            pos++;
            c = '\n';
            dirtyLittleFlag = true;
        }
        
        
    } while (pos < 7 || !matches(pos - 6, 6, END_FUN));
}

// requires scanf to wait until keyword endif is found.
// parsing of if stmt will begin once matching endif
void BaMInterpreter::waitEndIf()
{
    
    char c = ' ';
    int lines = 0;
    bool endif = false;
    bool dirtyLittleFlag = false;
    
    do
    {
        if (c == '\n')
        {
            std::cout << PROMPT2;
            
            if( (addSemiColon() && lines > 0) || (dirtyLittleFlag && lines > 0) )
            {
                lexer->userInput[pos] = ';';
                lexer->userInput[++pos] = ' ';
                if(dirtyLittleFlag) dirtyLittleFlag = !dirtyLittleFlag; //Flip the bits!
            }
            
            lines++;
        }
        
        
        //Get next char, increment pos
        nextChar(c);
        pos++;

        //Add newline
        if (c == '\n')
            lexer->userInput[pos] = ' ';
        
        //Add presented character
        else
            lexer->userInput[pos] = c;
        
        
        //Remove list id from form last statement
        if(matches(pos - 5, 5, "endif"))
        {
            //Strip semi colon from end
            if(lines > 0)
            {
                lexer->userInput[pos-7] = ' ';
            }
            
            endif = true;
        }
        else if(matches(pos -2, 2, "if"))
        {
            waitEndIf();
            pos++;
            c = '\n';
            dirtyLittleFlag = true;
        }
        
        
    } while (!endif);
    
} //waitEndIf

//Returning true means quit
//Returning false means reset and continue
bool BaMInterpreter::handleUtilities()
{
    pos++;
    
    //Pull the input buffer until end of input
    char c = 'a';

    
    while (!isspace(c) && c != '\0')
    {
        nextChar(c);
        lexer->userInput[pos] = c;
        pos++;
    }

//
//    const char* SHOW_GLOBAL = "$global_table";
//    const char* SHOW_GLOBAL_VARS = "$global_table.vars";
//    const char* SHOW_GLOBAL_FUNCS = "$global_table.functions";
    
    //Quits the program
    GlobalEnvironment* g = GlobalEnvironment::getInstance();
    
    //Returns false to terminate interpreter
    if (pos >= 6 && matches(pos-6, 5, QUIT))
    {
        return true;
    }
    
    //Presents the global table to the user
    else if( pos >= 14 && matches(pos-14, 13, SHOW_GLOBAL) )
    {
        g->printGlobals();
        return false;
    }
    
    //Presents global table to user with only variables
    else if( pos >= 19 && matches(pos-19, 18, SHOW_GLOBAL_VARS) )
    {
        g->printVariables();
        return false;
    }
    
    //Presents the global table to the user with only functions
    else if( pos >= 24 && matches(pos-24, 23, SHOW_GLOBAL_FUNCS) )
    {
        g->printFunctions();
        return false;
    }
    
    std::cout << "ERROR: invalid utility command" << std::endl;
    return false;
    
}

bool BaMInterpreter::addSemiColon()
{
    if (lexer->userInput[pos-1] == '\n' ||
        lexer->userInput[pos-1] == ' ' ||
        lexer->userInput[pos-1] == ';')
    {
        lexer->userInput[pos] = ' ';
        return false;
        
    }
    else if (lexer->userInput[pos-1] != ';')
    {
        return true;
    }
    
    return false;
}
