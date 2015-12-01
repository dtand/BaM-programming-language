//
//  BaMParser.h
//  BaMVariableserpreter
//
//  Created by Daniel Anderson on 10/23/15.
//  Copyright (c) 2015 Daniel Anderson. All rights reserved.
//

#ifndef __BaMVariableserpreter__BaMParser__
#define __BaMVariableserpreter__BaMParser__


#include <stdio.h>
#include "LexicalAnalyzer.h"
#include <stack>


/*********************************
 Parsing Class
 **********************************
 The following EBNF is used to parse the BaM grammar:
 
 <program>  -> <stmt_L> | <fundef>     // this is essentially the main
 <fundef>   -> fundef <var> = <funname>( <param_L> ) <stmt_L> endfun
 <stmt_L>   -> <if_stmt> {; <if_stmt> } | <if_stmt> {\n <if_stmt> }
 <if_stmt>  -> if( <expr> ) <stmt_L> endif | <stmt>
 <stmt>     -> <var> = <expr>  | <expr>
 <expr_L>   -> <expr> {, <expr>}
 <expr>     -> <term> {(+|-) <term>}
 <term>     -> <fact> {(*|/) <fact>}
 <fact>     -> <num> | <fun_inv>
 <param_L>  -> <var> {, <var>}
 <fun_inv>  -> <var>( <expr_L> ) | <var>   //use left factor hack in var tokenizer
 <funname>  -> [a-z,_]*
 <var>      -> [a-z,_]*
 <num>      -> [0-9]*
 
 *********************************/
class BaMParser
{
    enum Mode {REGULAR, STORING};
    
private:
    LexicalAnalyzer *lexer;
    ActivationRecord* globalScope;
    std::stack<ActivationRecord*> scopes;
    
    //For function storage
    Mode mode;
    Function currentFunction;
    
    void _parseLexeme();
    void _storeFunction(Function* function);
    void _testFunction(Function* function);
    void _skipBlock();
    
public:
    BaMParser(LexicalAnalyzer *pLexer)
    {
        mode = REGULAR;
        lexer = pLexer;
        scopes.push(new ActivationRecord(true, nullptr));
        globalScope = scopes.top();
    }
    
    ~BaMParser()
    {
        while (!scopes.empty())
        {
            delete scopes.top();
            scopes.pop();
        }
    }
    
    //begins parsing routine
    void begin();
    
    // <program>  -> <stmt_L> | <fundef>
    void program();
    
    // <fundef>   -> fundef <var> = <funname>( <param_L> ) <stmt_L> endfun
    void fundef();
    void createFun(std::string functionName,            //Used to create functions
                   std::vector<std::string> params,     //after they have been parsed
                   Variable returnVar);                 //inside fundef
    
    // <stmt_L>   -> <if_stmt> {; <if_stmt> } | <if_stmt> {\n <if_stmt> }
    Variable stmtL();
    
    // <if_stmt>  -> if( <expr> ) <stmt_L> endif | <stmt>
    Variable ifStmt();
    
    // <stmt>     -> <var> = <expr>  | <expr>
    Variable stmt();
    
    // <expr_L>   -> <expr> {, <expr>}
    std::vector<Variable> exprL();
    
    // <expr>     -> <term> {(+|-) <term>}
    Variable expr();
    
    // <term>     -> <fact> {(*|/) <fact>}
    Variable term();
    
    // <fact>     -> <num> | <fun_inv>
    Variable fact();
    
    // <param_L>  -> <var> {, <var>}
    std::vector<std::string> paramL();
    
    // <fun_inv>  -> <var>( <expr_L> ) | <var>
    Variable funInv();
    
    // <funname>  -> [a-z,_]*
    Variable funName();
    Variable invokeFunction(std::string functionName, std::vector<Variable> params);
    
    // <var>      -> [a-z,_]*
    Variable var();
    
    //Specialized function for assignments
    Variable varForAssign();
    
    // <num>      -> [0-9]*
    Variable num();

    
    
};

#endif /* defined(__BaMVariableserpreter__BaMParser__) */





