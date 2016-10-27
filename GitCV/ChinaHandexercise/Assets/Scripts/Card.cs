using UnityEngine;
using System.Collections;

public class Card : MonoBehaviour {

	public int number;
	public string suit;
	public Sprite sprite;
	public Vector3 handPosition;
	public int indexInHand;
	public bool isHidden;
	public bool isRevealable;
	public bool isDiscarded;
	public GameObject owner;

	// Use this for initialization
	void Awake () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetSprite(){
		var fileNumber = "";

		switch(number)
		{
		case 11:
			fileNumber = "jack";
			break;
		case 12:
			fileNumber = "queen";
			break;
		case 13:
			fileNumber = "king";
			break;
		case 14:
			fileNumber = "ace";
			break;
		default:
			fileNumber = number.ToString();
			break;
		}

		sprite = Resources.Load<Sprite>("Images/" + fileNumber + "_of_" + suit);
		this.GetComponent<SpriteRenderer>().sprite = sprite;
	}

	public void SetStats(int givenNumber, string givenSuit, Sprite givenSprite, int givenIndexInHand, bool givenHiddenStatus){
		number = givenNumber;
		suit = givenSuit;
		sprite = givenSprite;
		indexInHand = givenIndexInHand;
		isHidden = givenHiddenStatus;
		SetSprite();
	}

	public void SetOwner(GameObject givenOwner)
	{
		owner = givenOwner;
	}

	public void NewIndexInHand(int givenIndex)
	{
		indexInHand = givenIndex;
	}

	public void SetHiddenRevealable(bool givenHiddenStatus, bool givenRevealableStatus)
	{
		isHidden = givenHiddenStatus;
		isRevealable = givenRevealableStatus;
	}
}
