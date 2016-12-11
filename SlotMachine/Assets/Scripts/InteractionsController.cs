using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InteractionsController : MonoBehaviour 
{
	public GameObject handle;
	public GameObject knob;
	public GameObject invisibleSlope;
	public GameObject coinSlot;
	public GameObject[] creditLights = new GameObject[3];
	public GameObject creditsObj;

	private bool handleClicked;
	private bool coinClicked;

	private int numCredits = 0;

	private GameObject selectedObject;

	private ReelControllerScript reelController;

	// Use this for initialization
	void Start () 
	{
		reelController = MyFunctions.getAccessTo<ReelControllerScript>("Reel Controller");

		handleClicked = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetMouseButtonUp(0))
		{
			handleClicked = false;

			if(coinClicked)
			{
				DisableCoin();
			}

			//Cursor.visible = true;
		}
	}

	void FixedUpdate()
	{
		// Check if the mouse clicked on something and figure out what it was
		CheckClicked();

		MoveHandle();

		if(coinClicked)
		{
			MoveCoin();
		}

	}

	private void DisableCoin()
	{
		coinClicked = false;
		Rigidbody rb = selectedObject.GetComponent<Rigidbody>();
		rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
		rb.angularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
		selectedObject.layer = 0;
		invisibleSlope.GetComponent<MeshCollider>().enabled = false;
		selectedObject.GetComponent<Rigidbody>().useGravity = true;
	}

	private void MoveCoin()
	{
		//float yHeight; 

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit rayHit;

		if(Physics.Raycast(ray, out rayHit, 150))
		{
			//Vector3 rayVector = rayHit.point;

			selectedObject.transform.position = new Vector3(rayHit.point.x, rayHit.point.y + 0.5f, rayHit.point.z);

			if(rayHit.transform.gameObject.tag == "Inner Coin Detector")
			{
				if(numCredits < 1000000)
				{
					DropCoin();
				}
			}
			/* Cool stuff but ultimately ended up not being neccessary
			yHeight = rayHit.point.y + 4.0f;
			Debug.Log(yHeight);
			//Debug.Log(rayVector);
			//selectedObject.transform.position = rayVector;

			float mTop = (rayVector.y - Camera.main.transform.position.y);
			float mx = mTop / (rayVector.x - Camera.main.transform.position.x);
			float bx = rayVector.y - (mx * rayVector.x);
			float mz = mTop / (rayVector.z - Camera.main.transform.position.z);;
			float bz = rayVector.y - (mz * rayVector.z);
			float xCord = (yHeight - bx)/mx;
			float zCord = (yHeight - bz)/mz;

			selectedObject.transform.position = new Vector3(xCord, yHeight, zCord);
			*/
		}
	}

	private void DropCoin()
	{
		DisableCoin();

		// don't let coin be picked up
		selectedObject.layer = 2;

		// move coin over slot
		selectedObject.transform.position = new Vector3(coinSlot.transform.position.x, coinSlot.transform.position.y + 1.0f, coinSlot.transform.position.z);

		// allow coin to fall through slot
		selectedObject.GetComponent<MeshCollider>().enabled = false;

		// increment credits
		IncrementCredits();
	}

	private void IncrementCredits()
	{
		numCredits++;
		UpdateCreditsText();
	}

	private void DecrementCredits()
	{
		numCredits--;
		UpdateCreditsText();
	}

	private void UpdateCreditsText()
	{
		creditsObj.GetComponent<TextMesh>().text = "Credits: " + numCredits;	
	}

	// might want to change rotation stuff to use eulerangles instead of weird quaternions
	private void MoveHandle()
	{
		Quaternion updatedRot;

		float buffer = 10.0f;
		float mousePosDifference;


		if(handleClicked)
		{
			if(!reelController.GetReelsSpinning())
			{
				//Debug.Log("Moving Handle");
				mousePosDifference = GetMousePosDifference();

				if(mousePosDifference > buffer)
				{
					if(handle.transform.rotation.x > -0.5f)
					{
						//Debug.Log(handle.transform.rotation.x);

						updatedRot = new Quaternion(handle.transform.rotation.x - 0.025f, 
							handle.transform.rotation.y, 
							handle.transform.rotation.z, 
							handle.transform.rotation.w);

						handle.transform.rotation = updatedRot;
					}

				}
				else if(mousePosDifference < -1.0f * buffer)
				{
					if(handle.transform.rotation.x < 0.0f)
					{
						updatedRot = new Quaternion(handle.transform.rotation.x + 0.025f, 
							handle.transform.rotation.y, 
							handle.transform.rotation.z, 
							handle.transform.rotation.w);

						handle.transform.rotation = updatedRot;
					}

				}
				else
				{
					// do nothing for now
				}
			}
		}
		else // move handle back towards default position
		{
			if(handle.transform.rotation.x < 0.0f)
			{
				updatedRot = new Quaternion(	handle.transform.rotation.x + 0.01f, 
					handle.transform.rotation.y, 
					handle.transform.rotation.z, 
					handle.transform.rotation.w);

				handle.transform.rotation = updatedRot;
			}
		}

		// check if handle is at bottom position
		if (handle.transform.rotation.x < -0.45f && handleClicked == true)
		{
			if(numCredits > 0)
			{
				DecrementCredits();
				reelController.SpinReels();
				handleClicked = false;
			}
		}
	}

	private void CheckClicked()
	{
		if(Input.GetButton("Fire1"))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit rayHit;

			if(Physics.Raycast(ray, out rayHit, 150))
			{
				//Vector3 rayVector = rayHit.point;

				if(rayHit.transform.gameObject.tag == "Knob")
				{
					//Debug.Log("Handle Grabbed");
					handleClicked = true;
					//Cursor.visible = false;
				}
				else if(rayHit.transform.gameObject.tag == "Coin")
				{
					coinClicked = true;
					selectedObject = rayHit.transform.gameObject;
					selectedObject.layer = 2; // makes the object now be ignored by the raycast so the raycast can see through it
					selectedObject.GetComponent<Rigidbody>().useGravity = false;
					invisibleSlope.GetComponent<MeshCollider>().enabled = true; // helps the coin move up to the coin slot
				}
			}
		}
	}

	private float GetMousePosDifference()
	{
		return Camera.main.WorldToScreenPoint(knob.transform.position).y - Input.mousePosition.y;
	}

	private bool MouseBelowKnob()
	{

		if(Camera.main.WorldToScreenPoint(knob.transform.position).y > Input.mousePosition.y)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}
