CC = g++
CFLAGS  = -g -Wall

default: bam

bam:
	$(CC) -o bam main.cpp  BaMInterpreter.cpp BaMParser.cpp LexicalAnalyzer.cpp Variable.cpp ActivationRecord.cpp
	

