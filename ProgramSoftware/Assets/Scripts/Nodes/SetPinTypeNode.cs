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

		int value = (portValue.isOn) ? 1 : 0;
		
		parser.AddCommand(Parser.CMD_SET_MODE, this);
		parser.AddCommand(int.Parse(portInput.text), this);
		parser.AddCommand(value, this);

		return true;
	}
}
