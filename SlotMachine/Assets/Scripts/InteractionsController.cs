using UnityEngine;
using System.Collections;

public class InteractionsController : MonoBehaviour 
{
	public GameObject handle;
	public GameObject knob;

	private bool handleClicked;
	private bool coinClicked;

	private GameObject selectedObject;

	// Use this for initialization
	void Start () 
	{
		handleClicked = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetMouseButtonUp(0))
		{
			handleClicked = false;
			coinClicked = false;

			//Cursor.visible = true;
		}
	}

	void FixedUpdate()
	{
		// Check if the mouse clicked on something and figure out what it was
		CheckClicked();

		MoveHandle();

		MoveCoin();
	}

	private void MoveCoin()
	{

		if(coinClicked)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit rayHit;

			if(Physics.Raycast(ray, out rayHit, 150))
			{
				selectedObject.transform.position = rayHit.point;
			}

		}
	}

	private void MoveHandle()
	{
		Quaternion updatedRot;

		float buffer = 10.0f;
		float mousePosDifference;

		if(handleClicked)
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
