using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WriteNode : Node
{
	public InputField	portInput;
	public Toggle		portValue;

	public override NodeType GetNodeType()
	{
		return NodeType.PIN_MODE;
	}

	public override bool Parse()
	{
		Parser parser = Parser.instance;

		// Get port write value
		int value = (portValue.isOn) ? 1 : 0;
		
		// Add write command
		parser.AddCommand(Parser.CMD_WRITE, this);
		// Add pin to write to as argument
		parser.AddCommand(int.Parse(portInput.text), this);
		// Add value to write as argument
		parser.AddCommand(value, this);

		return true;
	}
}
