using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public static class MyFunctions
{
	public static T getAccessTo<T>(string myTag)
	{
		GameObject gameObj = GameObject.FindWithTag(myTag);

		if(gameObj != null)
		{
			return gameObj.GetComponent<T>();
		}
		else
		{
			Debug.Log("Returning Null For " + myTag);
			return default(T);
		}

	}
}
