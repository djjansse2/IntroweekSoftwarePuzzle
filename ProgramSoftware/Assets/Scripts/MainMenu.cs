using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	public InputField comPortInput;

	public void StartMainScene()
	{
		string portNumber = comPortInput.text;

		SerialCommunication.SetSerialPortID(portNumber);
		SceneManager.LoadScene("MainScene");
	}
}
