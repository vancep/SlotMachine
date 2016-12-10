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

	public static float DistToObjectBelow(GameObject obj)
	{
		Ray ray = new Ray();
		ray.origin = obj.transform.position;
		ray.direction = new Vector3(0.0f, -1.0f, 0.0f);

		RaycastHit rayHit;

		if(Physics.Raycast(ray, out rayHit, 150))
		{
			//Vector3 rayVector = rayHit.point;

			Debug.Log(rayHit.transform.gameObject.tag);

			return obj.transform.position.y - rayHit.transform.position.y;
		}

		return 1.0f;
	}
		
	public static float HeightOfObjectBelow(GameObject obj)
	{
		Ray ray = new Ray();
		ray.origin = obj.transform.position;
		ray.direction = new Vector3(0.0f, -1.0f, 0.0f);

		RaycastHit rayHit;

		if(Physics.Raycast(ray, out rayHit, 150))
		{
			Vector3 rayVector = rayHit.point;

			//Debug.Log(rayHit.point.y + rayHit.transform.gameObject.tag);

			return rayVector.y;
		}


		return 0.0f;
	}
}
