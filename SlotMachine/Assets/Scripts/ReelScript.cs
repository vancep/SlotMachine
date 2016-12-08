using UnityEngine;
using System.Collections;

public class ReelScript : MonoBehaviour {

	private Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		rb.angularVelocity = new Vector3(-10.0f, 0.0f, 0.0f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
