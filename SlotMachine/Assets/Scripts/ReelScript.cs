using UnityEngine;
using System.Collections;

public class ReelScript : MonoBehaviour 
{

	private Rigidbody rb;
	private bool isStopped;

	// Use this for initialization
	void Start () 
	{
		isStopped = true;
		rb = GetComponent<Rigidbody>();
	}

	public void SpinReel()
	{
		rb.angularVelocity = new Vector3(-10.0f, 0.0f, 0.0f);
		isStopped = false;
	}

	public void StopReel()
	{
		float x;
		isStopped = true;

		Quaternion rotation = Quaternion.identity;

		// stops the reel from spinning
		rb.angularVelocity = new Vector3(0.0f, 0.0f, 0.0f);

		// then adjust the reel slightly so it lines up nice
		x = rb.rotation.eulerAngles.x;

		rotation.eulerAngles = new Vector3(((int)(x / 18.0f) * 18.0f), rb.transform.eulerAngles.y, rb.transform.eulerAngles.z);


		rb.transform.rotation = rotation;

		//rb.transform.rotation.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
		// i think incorrect?
		//rb.transform.eulerAngles = new Vector3(((int)(x/18) * 18.0f), rb.transform.eulerAngles.y, rb.transform.eulerAngles.z);
	}

	public bool GetStopped()
	{
		return isStopped;
	}

	// returns what the reel is stopped on
	// note: the image on the reel is on there twice so have to account for backside properly
	public int GetResult()
	{
		Vector3 rotation = rb.rotation.eulerAngles;
		//Debug.Log(rotation);

		// not putting in exactly 180.0f due to number being potentially slightly off 
		// by some super small amount.... like 0.000001. 
		if(rotation.y > 179.0f && rotation.y < 181.0f)
		{
			rotation.Set(-1 * (rotation.x - 180.0f), 0.0f, 90.0f);
		}
		else if(rotation.y > 89.0f && rotation.y < 91.0f)
		{
			rotation.Set((rotation.x - 360.0f), 0.0f, 90.0f);
		}

		if(rotation.x < 0.0f)
		{
			rotation.x += 360.0f;
		}

		//Debug.Log(rotation);

		for(int i = 0; i < 20; i++)
		{
			if(rotation.x >= (i * 18) - 9 && rotation.x < (i * 18) + 9)
			{
				return i % 10;
			}
		}

		return 0;
	}

	// Update is called once per frame
	void Update () 
	{
	
	}
}
