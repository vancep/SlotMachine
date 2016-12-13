using UnityEngine;
using System.Collections;
using System.Threading;

public class CoinScript : MonoBehaviour 
{
	private float timeToDestroy;
	private float life = 2.0f; // 1 sec = 1.0f
	private bool destroy = false;

	private Rigidbody rb;
	// Use this for initialization
	void Start () 
	{
		rb = this.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () 
	{

		if(destroy)
		{
			timeToDestroy += Time.deltaTime;
			Debug.Log(timeToDestroy);

			if(timeToDestroy >= life)
			{
				DestroyImmediate(this.gameObject);
			}
		}
	}

	void FixedUpdate()
	{
		
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag("Outer Coin Detector"))
		{
			//Debug.Log("Rotated Coin For Entry");
			rb.transform.eulerAngles = new Vector3(90.0f, 0.0f, 0.0f);
			rb.freezeRotation = true;
		}
		else if(other.gameObject.CompareTag("Inner Coin Detector"))
		{
			//Debug.Log("ouch");
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.gameObject.CompareTag("Outer Coin Detector"))
		{
			rb.freezeRotation = false;
		}
	}

	public void SetToDestroy()
	{
		Debug.Log("Coin set to be destroyed");
		destroy = true;
		timeToDestroy = 0.0f;
	}
}
