//
//  main.cpp
//  BaMInterpreter
//
//  Created by Daniel Anderson on 10/23/15.
//  Copyright (c) 2015 Daniel Anderson. All rights reserved.
//

#include <iostream>
#include "BaMInterpreter.h"
GlobalEnvironment* GlobalEnvironment::instance = nullptr;

int main(int argc, const char * argv[])
{
    BaMInterpreter interpreter;
    interpreter.begin();
    
    return 0;
}