using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	public InputField comPortInput;

	public void StartMainScene()
	{
		// Get the input from the text field
		string portNumber = comPortInput.text;

		// Open the required serial port
		SerialCommunication.SetSerialPortID(portNumber);
		// Load the main scene
		SceneManager.LoadScene("MainScene");
	}
}
