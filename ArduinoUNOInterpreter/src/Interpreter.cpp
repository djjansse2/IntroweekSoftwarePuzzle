#include "Interpreter.h"

Interpreter::Interpreter() {}
Interpreter::~Interpreter() {}

Interpreter::Interpreter(int aAssembly[], int aSize)
{
	setAssembly(aAssembly, aSize);
}
/*
 * Stores the assembly code (commands)
 * 
 * aAssembly	: Array of commands and arguments (integers)
 * aSize		: Amount of integers in array
 */
void Interpreter::setAssembly(int aAssembly[], int aSize)
{
	// Resize the array
	this->_assembly = (int*) realloc(this->_assembly, aSize * sizeof(int));

	/*
	 * Copy over all values
	 */
	for (int i = 0; i < aSize + 1; i++)
	{
		this->_assembly[i] = aAssembly[i];
		// Serial.print(_assembly[i]);
	}

	// Store size
	this->_asmSize = aSize;

	// Reset command pointer
	this->_asmPointer = 0;

	// Reset the read register
	this->_readRegister = 0;
}

/*
 * Executes all comands in assembly array
 */
void Interpreter::runAssembly()
{
	if (_asmSize <= 0)
	{
		return;
	}

	//Reset pointer if necessary
	this->_asmPointer = 0;

	/*
	 * Loop through all commands and execute them
	 */
	while (this->_asmPointer < this->_asmSize)
	{
		runCommand(this->_assembly[this->_asmPointer]);
	}
}

/*
 * Executes a command
 * 
 * aCommand : Command to be executed (integer)
 */
void Interpreter::runCommand(int aCommand)
{
	switch (aCommand)
	{
	case WRITE_PIN:
		writePin();
		break;
	case READ_PIN:
		readPin();
		break;
	case JUMP:
		jump();
		break;
	case IF:
		ifFalse();
		break;
	case SET_PIN_MODE:
		setPinMode();
		break;
		
	default:
		break;
	}

	if (_asmPointer >= _asmSize)
	{
		// Reset the command pointer
		_asmPointer = 0;
	}
	else
	{
		// Increase the command pointer
		_asmPointer++;
	}
}

void Interpreter::writePin()
{
	// Write the LED pin with the next integer in the
	// array as argument
	int pin = _assembly[++_asmPointer];
	int value = _assembly[++_asmPointer];
	digitalWrite(pin, value);
}

void Interpreter::readPin()
{
	int pin = _assembly[++_asmPointer];
	_readRegister = digitalRead(pin);
}

void Interpreter::jump()
{
	_asmPointer = _assembly[++_asmPointer - 1];
}

void Interpreter::ifFalse()
{
	++_asmPointer;
	if (!_readRegister)
		_asmPointer = _assembly[_asmPointer] - 1;
}

void Interpreter::setPinMode()
{
	int pin = _assembly[++_asmPointer];
	int value = _assembly[++_asmPointer];
	pinMode(pin, value);
}