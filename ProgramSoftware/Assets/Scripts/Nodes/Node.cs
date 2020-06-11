using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Node	previousNode;
	public Node	nextNode;

	public virtual void Parse() {}
}
