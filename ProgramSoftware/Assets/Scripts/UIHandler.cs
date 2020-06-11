using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
	public void AddNode(string aNodeName)
	{
		string path = "Nodes/" + aNodeName;
		Debug.Log(path);
		GetComponent<User>().DragNode((GameObject)Instantiate(Resources.Load(path)));
	}
}
