using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetPinTypeNode : Node
{
	public InputField	portInput;
	public Toggle		portValue;

	public override NodeType GetNodeType()
	{
		return NodeType.PIN_MODE;
	}

	public override bool Parse()
	{
		///
		/// If value is 0, mode is INPUT
		/// If value is 1, mode is OUTPUT
		///

		Parser parser = Parser.instance;

		// Get mode value
		int value = (portValue.isOn) ? 1 : 0;
		
		// Add pin time command
		parser.AddCommand(Parser.CMD_SET_MODE, this);
		// Add pin to set mode as argument
		parser.AddCommand(int.Parse(portInput.text), this);
		// Add mode value as argument
		parser.AddCommand(value, this);

		return true;
	}
}
