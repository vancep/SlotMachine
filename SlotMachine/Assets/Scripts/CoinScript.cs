using UnityEngine;
using System.Collections;

public class CoinScript : MonoBehaviour 
{

	private Rigidbody rb;
	// Use this for initialization
	void Start () 
	{
		rb = this.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	void FixedUpdate()
	{
		
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag("Outer Coin Detector"))
		{
			Debug.Log("Rotated Coin For Entry");
			rb.transform.eulerAngles = new Vector3(90.0f, 0.0f, 0.0f);
			rb.freezeRotation = true;
		}
		else if(other.gameObject.CompareTag("Inner Coin Detector"))
		{
			Debug.Log("ouch");
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.gameObject.CompareTag("Outer Coin Detector"))
		{
			rb.freezeRotation = false;
		}
	}
}
