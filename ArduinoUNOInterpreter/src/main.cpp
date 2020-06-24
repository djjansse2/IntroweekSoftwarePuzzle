#include <Arduino.h>

#include <TaskScheduler.h>

#include "Interpreter.h"

#define END_PROGRAM		199
#define PROGRAM_SUCCESS	198

#define STATE_PIN		12
#define RESET_DELAY 	10
#define RESET_BLINK 	100

static Interpreter* myInterpreter = new Interpreter();

bool taskReadPort();
void taskRunAssembly();

int buffer[100];
int bufferPointer = 0;	

////////// DEBUGGING VARIABLES ///////////
bool doesResetDelay = true;
	
void setup()
{
	Serial.begin(9600);

	pinMode(STATE_PIN, OUTPUT);

	int timer = 0;

	while (doesResetDelay && timer < RESET_DELAY * 1000)
	{
		digitalWrite(STATE_PIN, HIGH);
		delay(RESET_BLINK);
		digitalWrite(STATE_PIN, LOW);
		delay(RESET_BLINK);

		timer += 2 * RESET_BLINK;
	}

	digitalWrite(STATE_PIN, HIGH);
	while (!taskReadPort());
	Serial.print((char)PROGRAM_SUCCESS);
	digitalWrite(STATE_PIN, LOW);
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