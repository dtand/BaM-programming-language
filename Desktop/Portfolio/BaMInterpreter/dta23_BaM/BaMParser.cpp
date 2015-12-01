//
//  BaMParser.cpp
//  BaMInterpreter
//
//  Created by Daniel Anderson on 10/23/15.
//  Copyright (c) 2015 Daniel Anderson. All rights reserved.
//

#include "BaMParser.h"
#include <sstream>
#include <vector>

//Starts parsing recursion
void BaMParser::begin()
{
    //Skip first and retrieve first char
    //from inut buffer
    lexer->pos++;
    lexer->getChar();
    
    //Set token to default
    lexer->nextToken = INT_LIT;
    
    try
    {
        //Begin parsing
        program();
    }
    catch (char const* error)
    {
        std::cout << error << std::endl;
    }
    catch (std::string error)
    {
        std::cout << error << std::endl;
    }
    
}

// <program> . <stmt_L> | <fundef>
void BaMParser::program()
{
    //std::cout << "Entering <program>" << std::endl;
    
    //Get the first token
    _parseLexeme();
    Variable v = Variable(true);
    
    if(lexer->nextToken == -1)
    {
        throw "ERROR: arbitrary nonsense was caught within the input stream";
    }
        
    if(lexer->nextToken == FUNC_START)
    {
        mode = STORING;
        _parseLexeme();
        fundef();
        mode = REGULAR;
    }
    else
    {
        //Perform parsing top down, and output
        //the final value of the line
        v = stmtL();
        
        //Returns statement value
        if(!v.isNan() && mode != STORING)
            std::cout << v.getL() << " is " << v.getR() << std::endl;
        
        //Returns nan if non-allocated var exists
        else if(v.isNan() && mode != STORING)
            std::cout << v.getR() << std::endl;
    }
    if (lexer->nextToken != END_INPUT_SYMBOL)
    {
        std::cout << "Syntax error, unexpected token at end of line" << std::endl;
    }
    
    //std::cout << "Exiting <program>" << std::endl;
}

// <fundef> . fundef <var> = <funname>( <param_L> ) <stmt_L> endfun
void BaMParser::fundef()
{
    //std::cout << "Entering <fundef>" << std::endl;
    
    //Parse variable for return value param
    std::string varName = lexer->lexeme;
    
    //Do some error checking
    if (lexer->nextToken != IDENT)
    {
        throw "ERROR: Incorrect type for function return value";
    }
    
    //Get the return variable for the function
    //then get next lexeme, should be '='
    Variable returnVar = Variable(varName, 0);

    _parseLexeme();
    
    if (lexer->nextToken != ASSIGN_OP)
    {
        throw "ERROR: Expected operator '=' before function name";
    }
    
    //Get next token should be identifier -> funname
    _parseLexeme();
    if (lexer->nextToken != IDENT)
    {
        throw "ERROR: Function name must be an identifier";
    }
    
    
    //Store the function name
    std::string functionName = lexer->lexeme;
    
    //Parses left paren
    _parseLexeme();

    //Sets next token for paramL
    _parseLexeme();
    
    //Parse the parameter list inside parens and return it
    std::vector<std::string> params = paramL();
    
    //Get the closing paren
    _parseLexeme();
    
    //Creates the funtion and adds it to global function list
    createFun(functionName, params, returnVar);
    
    
    //std::cout << "Exiting <fundef>" << std::endl;
    
}

void BaMParser::createFun(std::string functionName, std::vector<std::string> params, Variable returnVar)
{
    
    //Try function first
    Function test(functionName);
    test.setReturnVar(returnVar);
    
    //Set all formal params which are just strings that are binded
    //to pass-in params
    for(int i = 0; i < params.size(); i++)
    {
        test.addFormal(params[i]);
    }

    //If the test function passes, then we can go ahead and create the function
    if ( test.paramsRepeat() )
    {
        std::string err = "ERROR: Function " + functionName + " has duplicate parameters";
        throw err;
    }
    
    //After we have retrieved all corresponding values, add the function
    //to the global scope, which creates and returns a pointer to the function
    //now we can add all corresponding data
    Function* newFunction = globalScope->createFunction(functionName);
    
    //Give function return value
    newFunction->setReturnVar(returnVar);
    
    //Set all formal params which are just strings that are binded
    //to pass-in params
    for(int i = 0; i < params.size(); i++)
    {
        newFunction->addFormal(params[i]);
    }
    
    //Store code in the function
    _storeFunction(newFunction);
    
    //Run a quick test invocation for syntax errors
    _testFunction(newFunction);
    
    //Assuming no exception was thrown, go ahead and
    //output the name of the function in the interpreter
    std::cout << functionName << std::endl;
}

void BaMParser::_storeFunction(Function* function)
{
    std::string space = " ";
    
    //I have break statement!
    while (lexer->nextToken != FUNC_END)
    {
        //Check our values stored into the function for accuracy
        //std::cout << "STORING: " << "'" << lexer->lexeme << "'";
        //std::cout << " in function " << function->getL() << std::endl;;
        
        //Adds lexeme to code then a space to seperate identifiers
        function->insertIntoCode(lexer->lexeme);
        function->insertIntoCode(space);
        
        //Parse next lexeme for the code
        lexer->lex();
    }
    
    function->insertIntoCode("$");
    
    //std::cout << "STORING: '$' in function " << function->getL();
}


//Generates code for a test implementation of the function
void BaMParser::_testFunction(Function* function)
{
    std::stringstream stream;
    stream << function->getL();
    stream << "(";
    
    //Test params will values of i, makes it easy to test the
    //testable, heh!
    for(int i = 0; i < function->getFormals().size(); i++)
    {
        stream << i;    //streamt the value
        
        if(i != function->getFormals().size()-1)
            stream << ", ";
        else
            stream << ")";
    }
    
    //Stream EOL
    stream << "$";
    
    //Get the code and copy it into funInv
    const char* code = stream.str().c_str();
    char funInv[strlen(code)];
    strcpy(funInv, code);
    strcpy(lexer->userInput, code);
    
    //reset lexer
    lexer->pos = 0;
    //lexer->code = funInv;
    
    
    //Run the code, cross fingers
    lexer->getChar();
    program();
    
}

// <stmt_L> . <if_stmt> {; <if_stmt> } | <if_stmt> {\n <if_stmt> }
Variable BaMParser::stmtL()
{
    //std::cout << "Entering <stmt_L>" << std::endl;
    
    //Parse if statement
    Variable v = ifStmt();

    //Continue parsting if statements while multi-line op appears
    while (lexer->nextToken == MULT_LINE_OP)
    {
        _parseLexeme();
        
        //Check for end keywords -> break if found
        if(lexer->nextToken == FUNC_END || lexer->nextToken == IF_END)
            break;
        
        v = ifStmt();
    }
    
    //std::cout << "Exiting <stmt_L>" << std::endl;
    return v;
}

// <if_stmt> . if( <expr> ) <stmt_L> endif | <stmt>
Variable BaMParser::ifStmt()
{
    //std::cout << "Entering <if_stmt>" << std::endl;
    
    Variable v(true);   //Make a nan var
    
    //We are for sure starting an if statment
    if(lexer->nextToken == IF_START)
    {
        _parseLexeme();
        
        //Error expected token left paren
        if(lexer->nextToken != LEFT_PAREN)
        {
            std::cout << "error in <if_stmt> expected token: '" << '(' << "'";
        }
        
        //Get beginning of expr to eval and set to v
        _parseLexeme();
        v = expr();
        
        //Check for closing brace
        if(lexer->nextToken != RIGHT_PAREN)
        {
            std::cout << "error in <if_stmt> expected token: '" << ')' << "'";
        }
    
        _parseLexeme();
        
        //Process if statement semantics are as follows:
        //0 and negatives are false, everything else is true
        //Parse expression inside parens and evaluate it
        int eval = v.getR();
        
        //Evaluates true
        if(eval > 0)
            v = stmtL();
        
        //Evaluates false
        else
            _skipBlock();
        
        if(lexer->nextToken != IF_END)
        {
            std::cout << "error in <if_stmt> expected token: '" << "endif" << "'";
        }
        
        _parseLexeme();
        
    }
    
    //Skip if statment move to regular statment
    else
    {
        //Get the next token
        //_parseLexeme();
        v = stmt();
    }
    
    //std::cout << "Exiting <if_stmt>" << std::endl;
    return v;
}

void BaMParser::_skipBlock()
{
    while (lexer->nextToken != IF_END)
    {
        _parseLexeme();
    }
}

// <stmt> . <var> = <expr>  | <expr>
// use lookahead(2) hack
Variable BaMParser::stmt()
{
    //std::cout << "Entering <stmt> -> uses lookahead(2)" << std::endl;
    
    Variable v(true);
    
    //Use lookahead(2) to check for equal
    if(lexer->delimLookahead() == '=')
    {
        std::string name = lexer->lexeme;
        
        //Get scope to store variables
        ActivationRecord* scope = scopes.top();
        
        //Look for variable and tell function we need
        //to skip throw on exception
        v = scope->findLocal(name, true);
        
        //Check for proper ident
        if (lexer->nextToken != IDENT)
        {
            throw "ERROR: incorrect lhs for assignment";
        }
        
        
        //Get =
        _parseLexeme();
        
        //We can assume variable does not exists, thus
        //we must create one w/ default value of zero
        if(v.isNan())
        {
            scope->addLocal(Variable(name, 0));
            v = scope->findLocal(name, false);
        }
        
        if(lexer->nextToken != ASSIGN_OP)
        {
            std::cout << "Syntax error, expected expression after '='" << std::endl;
            return Variable(true);
        }
        
        _parseLexeme();                //Get the next var or expr
        Variable rhs = expr();         //Get expr
        
        //Apply sematics, this comprises of copying value from
        //rhs over to the lhs
        v.setR(rhs.getR());
        scope->updateLocal(v.getL(), v.getR());
        
    }
    
    //Assume expression -> move back because we used lookahead
    else
    {
        v = expr();
    }
    
    
    //std::cout << "Exiting <stmt>" << std::endl;
    return v;
}

// <expr_L> . <expr> {, <expr>}
std::vector<Variable> BaMParser::exprL()
{
    //std::cout << "Entering <expr_L>" << std::endl;
    
    std::vector<Variable> expressions;
    Variable e = expr();
    expressions.push_back(e);

    while (lexer->nextToken == COMMA)
    {
        _parseLexeme();
        Variable e2 = expr();
        expressions.push_back(e2);
    }
    
    //std::cout << "Exiting <expr_L>" << std::endl;
    return expressions;
}

// <expr> . <term> {(+|-) <term>}
Variable BaMParser::expr()
{
    
    //std::cout << "Entering <expr>" << std::endl;
    Variable v = term();
    
    //Because we used lookahead, go ahead and jump to
    //while loop ie -> we already have the term
    while (lexer->nextToken == ADD_OP || lexer->nextToken == SUB_OP)
    {
        
        //Go ahead and put decision switch here not
        //so applying semantics later is setup
        switch (lexer->nextToken)
        {
            case ADD_OP: _parseLexeme(); v += term(); break;
                
            case SUB_OP: _parseLexeme(); v -= term(); break;
                
            default: break;
        }
        
    }
    
    
    //std::cout << "Exiting <expr>" << std::endl;
    return v;
}

// <term> . <fact> {(*|/) <fact>}
Variable BaMParser::term()
{
    //std::cout << "Entering <term>" << std::endl;
    
    //Parse fact
    Variable v = fact();
    
    while (lexer->nextToken == MULT_OP || lexer->nextToken == DIV_OP)
    {
        
        //Go ahead and put decision switch here not
        //so applying semantics later is setup
        switch (lexer->nextToken)
        {
            case MULT_OP: _parseLexeme(); v *= fact(); break;
                
            case DIV_OP: _parseLexeme(); v /= fact(); break;
                
            default: break;
        }
        
    }
    
    //std::cout << "Exiting <term>" << std::endl;
    
    return v;
}

// <fact> . <num> | <fun_inv>
Variable BaMParser::fact()
{
    //std::cout << "Entering <fact>" << std::endl;
   
    //Parse int lit, apply semantics
    if(lexer->nextToken == INT_LIT)
    {
        return num();
    }
    
    //Assume function inv
    else
    {
        return funInv();
    }
    
    //std::cout << "Exiting <fact>" << std::endl;
}

// <param_L> . <var> {, <var>}
std::vector<std::string> BaMParser::paramL()
{
    //std::cout << "Entering <paramL>" << std::endl;
    
    //Holds the list of parameters to be returned
    std::vector<std::string> params;
    
    //Parse var / identifier and push onto params
    params.push_back(lexer->lexeme);
    
    //Get potential comma
    _parseLexeme();
    
    //While theres a comma, parse an additional var
    while (lexer->nextToken == COMMA)
    {
        _parseLexeme();
        params.push_back(lexer->lexeme);
        _parseLexeme();
        
    }
    
    //std::cout << "Exiting <paramL>" << std::endl;
    return params;
}

// <fun_inv> . <var>( <expr_L> ) | <var>
Variable BaMParser::funInv()
{
    
    //std::cout << "Entering <funInv>" << std::endl;
    
    //Store the lexeme before lexing in case we have
    //an actual function invocation
    std::string key = lexer->lexeme;
    
    //Gets the function name
    Variable funName = var();
    Variable returnVal(true);

    
    std::vector<Variable> params;
    
    //Check for fun invocation
    if(lexer->nextToken == LEFT_PAREN)
    {
        
        _parseLexeme();
        
        //Parse expression list
        params = exprL();
        
        //Check for closing parameter
        if(lexer->nextToken != RIGHT_PAREN)
        {
            std::cout << "error in <fun_inv> expected token: '" << ')' << "'";
        }
        
        //Invoke the function by name and bind formals with pass ins
        returnVal = invokeFunction(funName.getL(), params);
        funName = returnVal;
        returnVal.setNan(true);
        
        //Get follow token after invoking function
        lexer->getChar();
        _parseLexeme();
    
        
    }

    
    //std::cout << "Exiting <funInv>" << std::endl;
    return funName;
}

Variable BaMParser::invokeFunction(std::string functionName, std::vector<Variable> params)
{
    //Attempt to retrive function
    Function *thisFunction = nullptr;
    thisFunction = globalScope->findFunction(functionName);
    
    
    //Throw error if number of params don't match expected
    if (thisFunction->getFormals().size() != params.size()  )
    {
        std::stringstream stream;
        stream << "ERROR: Expected number of parameters is ";
        stream << thisFunction->getFormals().size();
        stream << " and " << params.size() << " were provided";
        std::string err = stream.str();
        throw err;
    }
    
    //Change lexers code to function code
    lexer->switchPointer(thisFunction->getCode());
    
    //Create new ARI with global scope as static parent, and push to runtime stack
    ActivationRecord *newRecord = new ActivationRecord(false, globalScope);
    scopes.push(newRecord);
    
    //Before running the code, the variables must be bound
    newRecord->bindParams(thisFunction, params);
    
    
    //Run statement list with new scope
    lexer->getChar();
    _parseLexeme();
    Variable v = stmtL();   
    
    //Get the return variable to be returned from invocation
    Variable returnVar = newRecord->findLocal(thisFunction->getReturnVar().getL(), false);
    returnVar.setNan(true);
    
    //Deallocate record after
    delete newRecord;
    scopes.pop();
    
    lexer->resetPointerState();
    
    //Return function value
    return returnVar;
}

// <funname> . [a-z,_]*
// at this point we need to locate our function
// and run its stored code
Variable BaMParser::funName()
{
    //std::cout << "Entering <funname>" << std::endl;
    Variable v(true);
    
    if(mode != STORING)
        v = Variable(lexer->lexeme, 0);

    
    _parseLexeme();
    //std::cout << "Exiting <funname>" << std::endl;
    return v;
}

// <var> . [a-z,_]*
// at this point we found a variable and
// we need to retrieve it and apply adequit semantics
Variable BaMParser::var()
{
   
    //std::cout << "Entering <var>" << std::endl;
    std::string lValue = lexer->lexeme;
    Variable v(true);
    
    _parseLexeme();
    
    //Checks if this is not a function invocation, in which
    //case we assume a regular variable
    if(lexer->nextToken != LEFT_PAREN)
    {
        v = scopes.top()->findLocal(lValue, false);
        v.setNan(true);
    }
    
    //We have a function invocation, we should save the lValue, but
    //continue to return a nan var
    else if(lexer->nextToken == LEFT_PAREN)
    {
        v.setL(lValue);
    }
    
    
    //std::cout << "Exiting <var>" << std::endl;
    return v;
}

// <num> . [0-9]*
Variable BaMParser::num()
{
    //std::cout << "Entering <num>" << std::endl;
    Variable v("nan", atoi(lexer->lexeme));
    v.setNan(true);
    _parseLexeme();
    //std::cout << "Entering <num>" << std::endl;
    return v;
}


void BaMParser::_parseLexeme()
{
        lexer->lex();
}








