using UnityEngine;
using System.Collections;
using System.Threading;

public class SpawnCoins : MonoBehaviour {

	public GameObject coin;

	private int coinsToDrop = 0;
	private float timeSinceLastDrop = 0.0f;
	private bool canDrop = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{

		if(!canDrop)
		{
			timeSinceLastDrop += Time.deltaTime;
			if(timeSinceLastDrop >= 0.3)
			{
				timeSinceLastDrop = 0;
				canDrop = true;
			}
		}
		else if(coinsToDrop > 0)
		{
			canDrop = false;
			coinsToDrop--;

			DropCoin();
		}

	}

	public void AddCoinsToDrop(int amount)
	{
		Debug.Log("Adding " + amount + " coins.");
		if(amount >= 0)
		{
			coinsToDrop += amount;
		}

	}

	private void DropCoin()
	{
		Instantiate(coin, this.transform.position, new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
	}
}
