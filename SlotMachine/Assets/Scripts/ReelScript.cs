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

		rb.angularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
		isStopped = true;

		x = rb.rotation.eulerAngles.x;
			
		rb.transform.eulerAngles = new Vector3(((int)(x/18) * 18.0f), rb.transform.eulerAngles.y, rb.transform.eulerAngles.z);
	}

	public bool GetStopped()
	{
		return isStopped;
	}

	// Update is called once per frame
	void Update () 
	{
	
	}
}
