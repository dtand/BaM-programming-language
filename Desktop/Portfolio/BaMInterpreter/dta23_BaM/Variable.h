//
//  Variable.h
//  BaMInterpreter
//
//  Created by Daniel Anderson on 11/19/15.
//  Copyright (c) 2015 Daniel Anderson. All rights reserved.
//

#ifndef __BaMInterpreter__Variable__
#define __BaMInterpreter__Variable__

#include <stdio.h>
#include <sstream>
#include <iostream>
#include <vector>
#include <unordered_map>
#include <iomanip>

//-- Digit counter code is not mine, I found it at the
//-- following site:
// http://stackoverflow.com/questions/1489830/efficient-way-to-determine-number-of-digits-in-an-integer
class DigitCounter
{
public:
    static int numDigits(int32_t x)
    {
        if (x == 0) return 1;
        if (x < 0) return numDigits(-x) + 1;
        
        if (x >= 10000) {
            if (x >= 10000000) {
                if (x >= 100000000) {
                    if (x >= 1000000000)
                        return 10;
                    return 9;
                }
                return 8;
            }
            if (x >= 100000) {
                if (x >= 1000000)
                    return 7;
                return 6;
            }
            return 5;
        }
        if (x >= 100) {
            if (x >= 1000)
                return 4;
            return 3;
        }
        if (x >= 10)
            return 2;
        return 1;
    }
};


class Variable
{
    
private:
    int rValue;
    
protected:
    std::string lValue;
    bool nan;
    
public:
    Variable(){rValue=0;}
    Variable(std::string key, int value)
    {
        rValue = value;
        lValue = key;
        nan = false;
    }
    
    Variable(bool makeNan)
    {
        nan = true;
    }
    
    //Accessors
    bool isNan(){ return nan; }
    int getR(){ return rValue; }
    std::string getL(){ return lValue; }
    
    //Setters
    void setR(int r){ rValue = r; }
    void setL(std::string l){ lValue = l; }
    void setNan(bool isNan){ nan = isNan; }
    
    Variable operator+(const Variable& rhs)
    {
        return Variable("nan", rValue + rhs.rValue);
    }
    
    void operator+=(const Variable rhs)
    {
        rValue += rhs.rValue;
    }
    
    Variable operator-(const Variable rhs)
    {
        return Variable("nan", rValue + rhs.rValue);
    }
    
    void operator -=(const Variable rhs)
    {
        rValue -= rhs.rValue;
    }
    
    Variable operator*(const Variable& rhs)
    {
        return Variable("nan", rValue * rhs.rValue);
    }
    
    void operator*=(const Variable rhs)
    {
        rValue *= rhs.rValue;
    }
    
    Variable operator/(const Variable rhs)
    {
        return Variable("nan", rValue / rhs.rValue);
    }
    
    void operator /=(const Variable rhs)
    {
        rValue /= rhs.rValue;
    }
    
    void operator =(const Variable rhs)
    {
        rValue = rhs.rValue;
        lValue = rhs.lValue;
        nan = rhs.nan;
    }
    
};

class Function : public Variable
{
    static const int codeSize = 4000;
    
    //Code exclusive to this function
    char code[codeSize];
    
    //Formal parameters that must be bound
    std::vector<std::string> formals;
    
    //Return value for function
    Variable returnVar;
    
    int pos;
    
public:
    Function(){}
    
    Function(std::string functionName)
    {
        lValue = functionName;
    }
    Function(Variable pReturnVar)
    {
        returnVar = pReturnVar;
        pos = 0;
    }
    
    void reset()
    {
        formals.clear();
        pos = 0;
    }
    
    void addFormal(std::string s){ formals.push_back(s); }
    
    //Returns true if any parameters are repeatd
    bool paramsRepeat()
    {
        int i;
        for(i = 0; i < formals.size(); i++)
        {   if( returnVar.getL() == formals.at(i) ) return true;   }
        
        for(i = 0; i < formals.size(); i++)
        {
            for(int j = i+1; j < formals.size(); j++)
            {
                if ( formals.at(i) == formals.at(j) )
                {
                    return true;
                }
            }
        }
        
        return false;
    }
    
    //Setters
    void setReturnVar(Variable pReturnVar){ returnVar = pReturnVar; }
    void setFormals(std::vector<std::string> pFormals)
    { for(std::string s : formals) formals.push_back((s)); }
    void insertIntoCode(std::string lexeme)
    {   for(char ch : lexeme) code[pos++] = ch;   }
    
    //Getters
    Variable getReturnVar(){ return returnVar; }
    std::vector<std::string> getFormals(){ return formals; }
    char* getCode(){ return code; }
    
};

class Enviornment
{
protected:
    std::unordered_map<std::string, Variable> variables;
    std::vector<std::string> variableKeys;
    
public:
    int getR(std::string key)
    {
        return variables.at(key).getR();
    }
    Variable getVar(std::string key)
    {
        return variables.at(key);
    }
    void addVar(Variable var)
    {
        var.setNan(false);
        variables[var.getL()] = var;
        variableKeys.push_back(var.getL());
    }
    void updateVar(std::string key, int value)
    {
        variables[key].setR(value);
    }
    
};

class GlobalEnvironment : public Enviornment
{
    
private:
    static GlobalEnvironment* instance;
    std::unordered_map<std::string, Function*> functions;
    std::vector<std::string> functionKeys;
    
public:
    GlobalEnvironment()
    {
        if(instance != nullptr)
        {
            throw "ERROR: Cannot instantiate additional sigleton of type <GlobalEnviornment>";
        }
        
        instance = this;
    }
    
    ~GlobalEnvironment()
    {
        for (int i = 0; i < functionKeys.size(); i++)
        {
            delete functions[functionKeys[i]];
        }
    }
    
    static GlobalEnvironment* getInstance(){     return instance;    }
    
    Function* findFunction(std::string key)
    {
        //If function does not exists, throw exception
        //up to highest level recursion
        if(functions.count(key) <= 0)
        {
            std::string err = "ERROR: no function exists with name " + key;
            throw err;
        }
        
        return functions[key];
        
    }
    
    Function* createFunction(std::string functionName)
    {
        
        //Check if fun exists
        if (functions.count(functionName) > 0)
        {
            std::string err = "ERROR: function already exists with name: " + functionName;
            throw err;
        }
        
        functionKeys.push_back(functionName);
        functions[functionName] = new Function(functionName);
        return functions[functionName];
    }
    
    void printGlobals()
    {
        printVariables();
        printFunctions();
        
    }
    
    void printVariables()
    {
        std::cout << std::endl;
        std::cout << std::endl;
        std::cout << "VARABLE" << std::setw(20) << "VALUE" << std::endl;
        std::cout << "-----------------------------------------------" << std::endl;
        
        for(int i = 0; i < variableKeys.size(); i++)
        {
            int value = variables[variableKeys[i]].getR();
            int offset = ( 7 - (int)variableKeys[i].length() ) - ( 5 - DigitCounter::numDigits(value) );
            
            std::cout << variableKeys[i] << std::setw(20 + offset) << value << std::endl;
            std::cout << "-----------------------------------------------" << std::endl;
        }
    }
    
    void printFunctions()
    {
        std::cout << std::endl;
        std::cout << std::endl;
        std::cout << "FUNCTIONS" <<  std::setw(24) << "PARAMETERS" << std::endl;
        std::cout << "-----------------------------------------------" << std::endl;
        
        for(int i = 0; i < functionKeys.size(); i++)
        {
            Function *f = functions[functionKeys[i]];
            
            std::stringstream stream;
            stream << f->getReturnVar().getL() << ", ";
            
            int size = (int) f->getFormals().size();
            
            for(int j = 0; j < size; j++)
            {
                stream << f->getFormals()[j];
                
                if(j != size-1)
                    stream << ", ";
                
            }
            
            int offset = ( 9 - (int)functionKeys[i].length() ) - ( 10 - (int) stream.str().length() );
            
            std::cout << functionKeys[i] << std::setw(24 + offset) << stream.str() << std::endl;;
            std::cout << "-----------------------------------------------" << std::endl;
            
            //Clears the stream
            stream.clear();
            stream.str(std::string());
        
        }
    }

};

#endif /* defined(__BaMInterpreter__Variable__) */
