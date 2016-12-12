using UnityEngine;
using System.Collections;
using System.Threading;

public class ReelControllerScript : MonoBehaviour 
{
	public GameObject[] reels = new GameObject[3];

	private float timeSinceLastStop = 0.0f;
	private float timeDelay;

	private float timeSinceLastDispense = 0.0f;
	private float dispenseDelay = 0.1f;

	private bool stopReels;
	private bool reelsSpinning;
	private int payout;
	private int[] results;
	private InteractionsController interactionsController;

	enum Symbols {Queen, Ace, Spade, Seven, Diamond, Jack, Heart, King, Cherry, Club};

	// Use this for initialization
	void Start () 
	{
		payout = 0;
		stopReels = false;
		reelsSpinning = false;
		results = new int[reels.Length];

		interactionsController = MyFunctions.getAccessTo<InteractionsController>("Interaction Controller");

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
			WorkOnStoppingReels();
		}

		if(payout > 0)
		{
			DispensePayout();
		}

	}

	private void CheckResults()
	{
		int numCherries = 0;

		GetResultsOfReels();

		if(ResultsAllSame())
		{
			switch(results[0])
			{
			case (int)Symbols.Ace: 
				payout += 50;
				break;
			case (int)Symbols.Cherry:
				payout += 50;
				break;
			case (int)Symbols.Club:
				payout += 35;
				break;
			case (int)Symbols.Diamond:
				payout += 35;
				break;
			case (int)Symbols.Heart:
				payout += 35;
				break;
			case (int)Symbols.Jack:
				payout += 25;
				break;
			case (int)Symbols.King:
				payout += 45;
				break;
			case (int)Symbols.Queen:
				payout += 35;
				break;
			case (int)Symbols.Seven:
				payout += 200;
				break;
			case (int)Symbols.Spade:
				payout += 35;
				break;
			}
		}
		else
		{
			numCherries = ResultsNumCherries();

			switch(numCherries)
			{
			case 1:
				payout += 1;
				break;
			case 2:
				payout += 5;
				break;
				// unless more reels are added, only these two options are possible since 3 Cherries would be caught by the function ResultsAllSame
			}
		}
	}
		
	private void DispensePayout()
	{
		// if delayed long enough
		timeSinceLastDispense += Time.deltaTime;

		if(timeSinceLastDispense >= dispenseDelay)
		{
			timeSinceLastDispense = 0.0f;

			interactionsController.IncrementCredits();

			payout--;
		}
	}

	private void GetResultsOfReels()
	{
		// get what 
		for(int i = 0; i < reels.Length; i++)
		{
			results[i] = reels[i].GetComponent<ReelScript>().GetResult();
			//Debug.Log("Results for " + i + ": " + results[i]);
		}

	}

	private bool ResultsAllSame()
	{
		for(int i = 1; i < results.Length; i++)
		{
			if(results[i] != results[i-1])
			{
				Debug.Log("Not All The Same");
				return false;
			}
		}

		Debug.Log("All The Same");
		return true;
	}

	private int ResultsNumCherries()
	{
		int numCherries = 0;

		for(int i = 0; i < results.Length; i++)
		{
			if(results[i] == (int)Symbols.Cherry)
			{
				numCherries++;
			}
		}

		Debug.Log("Cherries: " + numCherries);
		return numCherries;
	}

	private void WorkOnStoppingReels()
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
				CheckResults();
			}
		}

	}
}
