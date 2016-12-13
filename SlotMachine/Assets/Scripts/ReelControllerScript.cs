using UnityEngine;
using System.Collections;
using System.Threading;

public class ReelControllerScript : MonoBehaviour 
{
	public GameObject[] reels = new GameObject[3];
	public GameObject coinSpawner;

	public int heartPayout;
	public int kingPayout;
	public int oneCherryPayout;
	public int twoCherriesPayout;
	public int threeCherriesPayout;
	public int clubPayout;
	public int queenPayout;
	public int acePayout;
	public int spadePayout;
	public int sevenPayout;
	public int diamondPayout;
	public int jackPayout;

	private float timeSinceLastStop = 0.0f;
	private float timeDelay;

	private bool stopReels;
	private bool reelsSpinning;
	private int payout;
	private int[] results;

	enum Symbols {Queen, Ace, Spade, Seven, Diamond, Jack, Heart, King, Cherry, Club};

	// Use this for initialization
	void Start () 
	{
		payout = 0;
		stopReels = false;
		reelsSpinning = false;
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
			WorkOnStoppingReels();
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
				payout += acePayout;
				break;
			case (int)Symbols.Cherry:
				payout += threeCherriesPayout;
				break;
			case (int)Symbols.Club:
				payout += clubPayout;
				break;
			case (int)Symbols.Diamond:
				payout += diamondPayout;
				break;
			case (int)Symbols.Heart:
				payout += heartPayout;
				break;
			case (int)Symbols.Jack:
				payout += jackPayout;
				break;
			case (int)Symbols.King:
				payout += kingPayout;
				break;
			case (int)Symbols.Queen:
				payout += queenPayout;
				break;
			case (int)Symbols.Seven:
				payout += sevenPayout;
				break;
			case (int)Symbols.Spade:
				payout += spadePayout;
				break;
			}
		}
		else
		{
			numCherries = ResultsNumCherries();

			switch(numCherries)
			{
			case 1:
				payout += oneCherryPayout;
				break;
			case 2:
				payout += twoCherriesPayout;
				break;
				// unless more reels are added, only these two options are possible since 3 Cherries would be caught by the function ResultsAllSame
			}
		}

		if(payout > 0)
		{
			DispensePayout();
		}
	}
		
	private void DispensePayout()
	{
		coinSpawner.GetComponent<SpawnCoins>().AddCoinsToDrop(payout);

		payout = 0;
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
