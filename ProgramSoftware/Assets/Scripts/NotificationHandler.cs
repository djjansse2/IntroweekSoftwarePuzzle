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

	public void NotifyWarning(string aWarning)
	{
		Notify(Color.yellow, aWarning);
		Debug.LogWarning(aWarning);
	}

	public void NotifyError(string aError)
	{
		Notify(Color.red, aError);
		Debug.LogError(aError);
	}

	private void Notify(Color aBackgroundColour, string aMessage)
	{
		notificationImage.color	= aBackgroundColour;
		notificationText.text	= aMessage;

		notificationObject.GetComponent<Animator>().SetTrigger("Fade");
	}
}
