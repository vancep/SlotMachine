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
			if(timeSinceLastDrop >= 0.1f)
			{
				timeSinceLastDrop = 0.0f;
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
		Instantiate(coin, new Vector3((this.transform.position.x - 1.5f) + (Random.value * 3.0f), this.transform.position.y, this.transform.position.z), coin.transform.rotation);
	}
}
