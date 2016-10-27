using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameHandler : MonoBehaviour {

	UIHandler ui;
	CardController cardController;
	PlayerHandler player;
	PlayerHandler ai;
	AIHandler aiHandler;

	AnimationHandler animations;

	public PlayerHandler whoIsPlaying;

	public List<GameObject> deck;
	Card card;

	public List<Card> playArea;
	public List<GameObject> playAreaObjects;
	Vector3 discardPilePosition;

	public Vector3 leftHandSlot;
	public Sprite cardBack;

	float maxHandSlotArea;

	string[] suits = new string[4];

	public bool playersTurn;
	public bool cardSelected;

	// Use this for initialization
	void Awake () {
		ui = GetComponent<UIHandler>();
		animations = GetComponent<AnimationHandler>();
		player = GameObject.Find("PlayerController").GetComponent<PlayerHandler>();
		ai = GameObject.Find("AIController").GetComponent<PlayerHandler>();
		aiHandler = GameObject.Find("AIController").GetComponent<AIHandler>();

		player.isPlayer = true;
		ai.isPlayer = false;

		cardBack = Resources.Load<Sprite>("Images/card_back");
		discardPilePosition = GameObject.Find("DiscardPile").transform.position;

		Initialize();
	}

	void Initialize()
	{
		suits[0] = "clubs";
		suits[1] = "hearts";
		suits[2] = "spades";
		suits[3] = "diamonds";

		BuildDeck();

		player.Initialize();
		ai.Initialize();

		playersTurn = true;
		whoIsPlaying = player;
	}
		
	public void AddToPlayArea(Card card, GameObject cardObject)
	{
		playArea.Add(card);
		playAreaObjects.Add(cardObject);
		whoIsPlaying.RemoveFromHand(card);
		CheckDiscard(card.number);

		ChangeTurn(card, false);
	}


	void CheckDiscard(int number){
			
		if(number == 10)
		{
			StartCoroutine("DiscardAnimation");
		}

		else if(playArea.Count >= 4 
			&& number == playArea[playArea.Count - 1].number 
			&& number == playArea[playArea.Count - 2].number
			&& number == playArea[playArea.Count - 3].number
			&& number == playArea[playArea.Count - 4].number)
		{
			StartCoroutine("DiscardAnimation");
		}
	}

	IEnumerator DiscardAnimation(){
		yield return new WaitForSeconds(1);
		for(int i = 0; i < playAreaObjects.Count; i++)
		{
			playAreaObjects[i].transform.position = discardPilePosition;
			playAreaObjects[i].GetComponent<SpriteRenderer>().sprite = cardBack;
			playAreaObjects[i].GetComponent<Card>().isDiscarded = true;
		}

		playArea.Clear();
		playAreaObjects.Clear();
	}





	public void PickUp(Card card, bool isPlayed, PlayerHandler whoIsPlaying)
	{
		if(isPlayed)
		{
			playArea.Remove(card);
			playAreaObjects.Remove(card.gameObject);
			card.SetOwner(whoIsPlaying.gameObject);
			Debug.Log(whoIsPlaying + " is picking up " + card.number + " of " + card.suit);
			whoIsPlaying.DrawCard(false, false, card);

			if(playArea.Count == 0) 
			{
				playAreaObjects.Clear();
				ChangeTurn(card, true);
			}

		}
		else{
			
			whoIsPlaying.PickUp(card);
			//card.SetOwner(whoIsPlaying.gameObject);
		}
	}
		
	public void ClearPlayArea(){
		
	}


	void BuildDeck()
	{
		deck = new List<GameObject>();

		for(int i = 0; i < 4; i++)
		{
			for (int j = 2; j < 15; j++)
			{
				GameObject cardObjectInDeck = new GameObject();
				card = cardObjectInDeck.AddComponent<Card>();

				card.number = j;
				card.suit = suits[i];
				card.sprite = Resources.Load<Sprite>("Images/" + card.number + "_of_" + card.suit);
				deck.Add(cardObjectInDeck);
			}
		}
	}

	bool CantPlayAnythingElse(Card card){
		int compare = card.number;

		if(card.number != 2 && card.number != 10)
		{
			if(whoIsPlaying == player){
				
				for (int i = 0; i < whoIsPlaying.handCards.Count; i++)
				{
					if (whoIsPlaying.handCards[i].GetComponent<Card>().number == compare)
					{
						return false;
					}
				}
			}

			return true;
		}
		else {
			return false;
		}
	}

	public void SwitchWhoIsPlaying(){

		if(whoIsPlaying == player)
		{
			playersTurn = false;
			whoIsPlaying = ai;
			aiHandler.StartTurn();
		}
		else
		{
			whoIsPlaying = player;
			playersTurn = true;
			Debug.Log("players turn");
		}

	}

	public void ChangeTurn(Card card, bool isPickup){

		if(whoIsPlaying.handCount != 0 && isPickup)
		{
			SwitchWhoIsPlaying();
		}

		//If player has cards in hand and is not picking up, draw a card
		else if(whoIsPlaying.handCount != 0 && !isPickup)
		{
			whoIsPlaying.UpdateSpriteOrder();

			if(whoIsPlaying.handCount < 3 && deck.Count > 0)
			{
				whoIsPlaying.DrawCard(true, false, null);
			}

			//Change turn only if player didnt play 2, 10 or has cards of same value in hand
			if(CantPlayAnythingElse(card))
			{
				SwitchWhoIsPlaying();
			}
		}

		//If player has no cards in hand and hidden cards left, set hidden cards revealable
		else if(whoIsPlaying.handCount == 0 && whoIsPlaying.hiddenSlots.Count != 0)
		{
			for(int i = 0; i < whoIsPlaying.hiddenSlots.Count; i++)
			{
				whoIsPlaying.hiddenCards[i].GetComponent<Card>().SetHiddenRevealable(false, true);
			}

			SwitchWhoIsPlaying();
		}

		//If player has more than 0 cards and hidden cards left, set hidden cards unrevealable
		else if(whoIsPlaying.handCount > 0 && whoIsPlaying.hiddenSlots.Count != 0)
		{
			for(int i = 0; i < player.hiddenSlots.Count; i++)
			{
				whoIsPlaying.hiddenCards[i].GetComponent<Card>().SetHiddenRevealable(true, false);
			}
			SwitchWhoIsPlaying();
		}


		//When player is out of hand cards and hidden cards, player wins.
		else {
			if(whoIsPlaying == player)
			{
				ui.DisplayWinText("player");
			}
			else {
				ui.DisplayWinText("ai");
			}
		}

	}

	IEnumerator TurnTimer(){
		yield return new WaitForSeconds(.5f);
		playersTurn = true;
	}
		
}
