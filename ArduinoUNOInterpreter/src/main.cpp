#include <Arduino.h>

#include <TaskScheduler.h>

#include "Interpreter.h"

#define END_PROGRAM	199

static Interpreter* myInterpreter = new Interpreter();

bool taskReadPort();
void taskRunAssembly();

int buffer[100];
int bufferPointer = 0;	
	
void setup()
{
	Serial.begin(9600);

	pinMode(2, OUTPUT);
	pinMode(3, OUTPUT);
	pinMode(4, INPUT);

	while (!taskReadPort());
	Serial.print('a');
}

void loop()
{
	taskRunAssembly();
}

bool taskReadPort()
{
	while (Serial.available())
	{
		int readByte = Serial.read();

		if (readByte != END_PROGRAM)
		{
			buffer[bufferPointer] = readByte;
			bufferPointer++;
		}
		else
		{
			myInterpreter->setAssembly(buffer, bufferPointer - 1);

			bufferPointer = 0;
			return true;
		}
	}

	return false;
}

void taskRunAssembly()
{
	myInterpreter->runAssembly();
}