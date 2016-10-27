using UnityEngine;
using System.Collections;

public class CardController : MonoBehaviour {
	Card card;
	GameHandler gameHandler;
	UIHandler ui;

	public bool draggable;
	public bool played;

	string number = "";
	string suit = "";

	Quaternion initialRot;

	// Use this for initialization
	void Awake () {
		card = GetComponent<Card>();
		gameHandler = GameObject.Find("GameController").GetComponent<GameHandler>();
		ui = GameObject.Find("GameController").GetComponent<UIHandler>();
		initialRot = GetComponent<DragCard>().initialRot;
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void Played() {
		GetComponent<SpriteRenderer>().sortingOrder = gameHandler.playArea.Count;
		transform.position = GameObject.Find("PlayArea").transform.position;
		GetComponent<DragCard>().DetermineRotation();
		GetComponent<Card>().SetSprite();

		played = true;
		gameHandler.AddToPlayArea(GetComponent<Card>(), this.gameObject);

		FormatText();
		ui.UpdatePlayedCardText(number, suit);
	}

	public void PickedUp(){

		transform.rotation = initialRot;
		card.isRevealable = false;
		gameHandler.PickUp(card, played, gameHandler.whoIsPlaying);
		played = false;
	}


	void FormatText(){
		switch(card.number)
		{
		case 11:
			number = "Jack";
			break;
		case 12:
			number = "Queen";
			break;
		case 13:
			number = "King";
			break;
		case 14:
			number = "Ace";
			break;
		default:
			number = card.number.ToString();
			break;
		}

		switch(card.suit)
		{
		case "spades":
			suit = "Spades";
			break;
		case "diamonds":
			suit = "Diamonds";
			break;
		case "clubs":
			suit = "Clubs";
			break;
		case "hearts":
			suit = "Hearts";
			break;
		}
	}

	public bool IsPlayable(Card draggedCard){
		int lastPlayed = 0;
		if(gameHandler.playArea.Count != 0)
		{
			lastPlayed = gameHandler.playArea.Count;
			int compare = gameHandler.playArea[lastPlayed - 1].number;

			if(compare == 2)
			{
				return true;
			}
			//Can play 11 or higher only if compared number is 7 or higher or 2
			else if(compare < 7 && draggedCard.number >= 11)
			{
				return false;
			}
			//Can play card if number is higher than compare or played card number is 2 or 10
			else if(draggedCard.number >= compare || draggedCard.number == 2 || draggedCard.number == 10)
			{
				GetComponent<SpriteRenderer>().sortingOrder = gameHandler.playArea[lastPlayed - 1].gameObject.GetComponent<SpriteRenderer>().sortingOrder + 2;
				return true;
			}
			else {
				return false;
			}
		}

		else {
			return true;
		}

	}
}
