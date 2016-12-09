using UnityEngine;
using System.Collections;

public class HandleScript : MonoBehaviour 
{
	public GameObject handle;

	private bool handleClicked;

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
			//Cursor.visible = true;
		}
	}

	void FixedUpdate()
	{
		Quaternion updatedRot;

		float mousePosDifference;
		float buffer = 10.0f;

		CheckClicked();

		if(handleClicked)
		{


			mousePosDifference = GetMousePosDifference();

			if(mousePosDifference > buffer)
			{
				if(handle.transform.rotation.x > -0.5f)
				{
					//Debug.Log(handle.transform.rotation.x);

					updatedRot = new Quaternion(	handle.transform.rotation.x - 0.025f, 
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
					updatedRot = new Quaternion(	handle.transform.rotation.x + 0.025f, 
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
				Vector3 rayVector = rayHit.point;

				if(rayHit.transform.gameObject.tag == "Knob")
				{
					handleClicked = true;
					//Cursor.visible = false;
				}
			}
		}
	}

	private float GetMousePosDifference()
	{
		return Camera.main.WorldToScreenPoint(this.transform.position).y - Input.mousePosition.y;
	}

	private bool MouseBelowKnob()
	{
		
		if(Camera.main.WorldToScreenPoint(this.transform.position).y > Input.mousePosition.y)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}
