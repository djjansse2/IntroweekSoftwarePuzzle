#ifndef Interpreter_h
#define Interpreter_h

#include "Arduino.h"

#define WRITE_PIN	100
#define READ_PIN	101
#define JUMP		102
#define IF			103
#define END_IF		104

class Interpreter
{
public:
			Interpreter(int[], int);
			Interpreter();

protected:
			~Interpreter();

public:
	void	setAssembly(int[], int);
	void	runAssembly();

private:
	void	runCommand(int);

	void	writePin();
	void	readPin();
	void	jump();
	void	ifFalse();

private:
	int*	_assembly;
	int		_asmSize;
	int		_asmPointer;

	bool	_readRegister;

    bool    _isRunning;
};

#endif