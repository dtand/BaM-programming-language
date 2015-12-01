//
//  ActivationRecord.h
//  BaMInterpreter
//
//  Created by Daniel Anderson on 11/19/15.
//  Copyright (c) 2015 Daniel Anderson. All rights reserved.
//

#ifndef __BaMInterpreter__ActivationRecord__
#define __BaMInterpreter__ActivationRecord__

#include <stdio.h>
#include "Variable.h"
#include <sstream>

class ActivationRecord
{

private:
    Enviornment *env;        //Enviornment exclusive to this record
    
    //static Enviornment *EP;                 //pointer to current enviornment, statically accessable
    ActivationRecord *staticLink;           //Static link to parent static env
    std::string name;
    
public:
    
    ActivationRecord(bool isGlobal, ActivationRecord *staticParent)
    {
        staticLink = nullptr;
        
        if(isGlobal)
        {
            env = new GlobalEnvironment();
            return;
        }
        
        else
        {
            env = new Enviornment();
            staticLink = staticParent;
        }
        
        //EP = env;
    }
    
    ~ActivationRecord()
   {
       delete env;
   }
    
    Variable findLocal(std::string key, bool isAssign)
    {
        
        try //Try to find local in current env
        {
            return env->getVar(key);
        }
        catch (std::exception e)    //If not found, look in parent
        {
            
            //Check for global env -> this is our base case
            if( ((GlobalEnvironment*)env) == GlobalEnvironment::getInstance() )
            {
                std::string err = "ERROR: Variable " + key + " does not exist";
                
                if (isAssign)
                {
                    return new Variable(true);  //Returns nan var
                }
                else
                {
                    throw err;
                }

            }
            else
                //Search parent
                return staticLink->findLocal(key, isAssign);
        }
        
    }
    
    void addLocal(Variable v)
    {
        env->addVar(v);
    }
    
    void updateLocal(std::string key, int value)
    {
        env->updateVar(key, value);
    }
    
    Function* findFunction(std::string key)
    {
        GlobalEnvironment *g = GlobalEnvironment::getInstance();
        return g->findFunction(key);
    }
    
    //Passes the function to the global enviornment to
    //check the string and create a corresponding function
    Function* createFunction(std::string functionName)
    {
        GlobalEnvironment* e = GlobalEnvironment::getInstance();
        return e->createFunction(functionName);
    }
    
    Enviornment* getEnviornment()
    {
        return env;
    }
    
    //Gives the scope a name and binds all formals
    void bindParams(Function* f, std::vector<Variable> params)
    {
        
        for(int i = 0; i < f->getFormals().size(); i++)
        {
            Variable param( f->getFormals()[i], params[i].getR() );
            env->addVar(param);
        }
        
        env->addVar(Variable(f->getReturnVar()));
    }
    
    
};

#endif /* defined(__BaMInterpreter__ActivationRecord__) */
