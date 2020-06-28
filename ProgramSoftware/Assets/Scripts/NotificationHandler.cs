using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NotificationHandler : MonoBehaviour
{
	public static NotificationHandler instance;

	public	GameObject	notificationObject;
	public	Text		notificationText;
	public	Image		notificationImage;

	private void Awake()
	{
		if (instance != null)
		{
			Debug.LogError("Cannot have more than one notification handler in scene");
			return;
		}

		instance = this;
	}


	/*
	 * Display a warning notification
	 * 
	 * aWarning	: Text for the warning
	 */
	public void NotifyWarning(string aWarning)
	{
		// Display a yellow notification with the text
		Notify(Color.yellow, aWarning);
		// Log the warning to the console
		Debug.LogWarning(aWarning);
	}

	/*
	 * Display a error notification
	 * 
	 * aError	: Text for the warning
	 */
	public void NotifyError(string aError)
	{
		// Display a red notification with the text
		Notify(Color.red, aError);
		// Log the error to the console
		Debug.LogError(aError);
	}

	/*
	 * Display a succes notification
	 * 
	 * aSucces	: Text for the notification
	 */
	public void NotifySucces(string aSucces)
	{
		// Display a green noticiation with the text
		Notify(Color.green, aSucces);
		// Log notification to the console
		Debug.Log(aSucces);
	}

	/*
	 * Display a notification
	 * 
	 * aBackgrounColour	: Colour of the notification banner
	 * aMessage			: Text for the notification
	 */
	private void Notify(Color aBackgroundColour, string aMessage)
	{
		// Set the colour for the banner
		notificationImage.color	= aBackgroundColour;
		// Set the text
		notificationText.text	= aMessage;

		// Start display and fade animation
		notificationObject.GetComponent<Animator>().SetTrigger("Fade");
	}
}
