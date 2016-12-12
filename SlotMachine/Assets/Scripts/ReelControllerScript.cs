using UnityEngine;
using System.Collections;
using System.Threading;

public class ReelControllerScript : MonoBehaviour 
{
	public GameObject[] reels = new GameObject[3];

	private float timeSinceLastStop = 0.0f;
	private float timeDelay;
	private bool stopReels;
	private bool reelsSpinning;
	private bool getResults;

	private int[] results;

	// Use this for initialization
	void Start () 
	{
		stopReels = false;
		reelsSpinning = false;
		getResults = false;
		results = new int[reels.Length];

		for(int i = 0; i < reels.Length; i++)
		{
			reels[i].AddComponent<ReelScript>();
		}

	}

	public void SpinReels()
	{
		for(int i = 0; i < reels.Length; i++)
		{
			reels[i].GetComponent<ReelScript>().SpinReel();
		}

		reelsSpinning = true;

		timeSinceLastStop = (Random.value * -0.5f) - 1.0f;
		StopReels();
	}

	public void StopReels()
	{
		stopReels = true;
	}

	public bool GetReelsSpinning()
	{
		return reelsSpinning;
	}

	// Update is called once per frame
	void Update () 
	{
		if(stopReels)
		{
			// if delayed long enough
			timeSinceLastStop += Time.deltaTime;
			timeDelay = (Random.value * 0.5f) + 0.2f;

			if(timeSinceLastStop >= timeDelay)
			{
				timeSinceLastStop = 0.0f;

				for(int i = 0; i < reels.Length; i++)
				{
					if(reels[i].GetComponent<ReelScript>().GetStopped() == false)
					{
						reels[i].GetComponent<ReelScript>().StopReel();
						break;
					}
				}

				// check if the wheels have all been stopped
				if(reels[reels.Length - 1].GetComponent<ReelScript>().GetStopped() == true)
				{
					stopReels = false;
					reelsSpinning = false;
					getResults = true;
				}
			}


		}
		else if(getResults)
		{
			GetResultsOfReels();
			getResults = false;
		}


	}

	private void GetResultsOfReels()
	{
		for(int i = 0; i < reels.Length; i++)
		{
			results[i] = reels[i].GetComponent<ReelScript>().GetResult();
			Debug.Log("Results for " + i + ": " + results[i]);
		}

	}
}
