using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerHandler : MonoBehaviour {
	
	GameHandler gameHandler;
	UIHandler ui;
	AnimationHandler animations;

	public bool isPlayer;

	Vector3 handPosition;
	public List<GameObject> cardObjects;
	public List<GameObject> handCards;
	GameObject cardObject;

	public List<GameObject> hiddenSlots;
	public List<GameObject> hiddenCards;

	public List<GameObject> handSlots;
	public List<bool> handSlotsFilled;

	Vector3 slotToGoTo;

	public int handSlotIndex;
	public int handCount;

	// Use this for initialization
	void Awake () {
		gameHandler = GameObject.Find("GameController").GetComponent<GameHandler>();
		ui = GameObject.Find("GameController").GetComponent<UIHandler>();
		animations = GameObject.Find("GameController").GetComponent<AnimationHandler>();
		isPlayer = true;
		cardObjects = new List<GameObject>();
		handSlotIndex = 0;
		handCount = 0;
	} 


	public void Initialize(){
		for(int i = 0; i < 3; i++)
		{
			CreateHiddenSlot(i);
			DrawCard(true, true, null);
		}
		for(int i = 0; i < 3; i++)
		{
			//CreateHandSlot();
			DrawCard(true, false, null);
		}

	}

	public void AddToHand(){
		handCount++;
	}

	public void RemoveFromHand(Card card){
		handCards.Remove(card.gameObject);
		EmptyHandSlot(card.indexInHand);
		handCount--;
	}


	public void CreateHandSlot()
	{
		float y = 0f;

		if(isPlayer) y = -3.5f;
		else y = 3.8f;

		GameObject handSlot = Instantiate(Resources.Load("HandSlot"), new Vector3(-3.2f, y, 0), transform.rotation) as GameObject;

		for(int i = 0; i < handSlots.Count; i++)
		{
			if(handSlots[i] == null)
			{
				handSlots[i] = handSlot;
				handSlots[i].transform.position += new Vector3(.7f + i, 0, 0);
				break;
			}
		}

		//handSlotsFilled.Add(false);
		//ShiftHandSlots();


	}

	public void CreateHiddenSlot(float index)
	{
		float y = 0f;

		if(isPlayer) y = -1.5f;
		else y = 2f;

		GameObject hiddenSlot = Instantiate(Resources.Load("HiddenSlot"), new Vector3(-1f + index, y, 0), transform.rotation) as GameObject;
		hiddenSlots.Add(hiddenSlot);
	}

	public void EmptyHandSlot(int givenIndexInHand)
	{
		//handSlotsFilled[givenIndexInHand] = false;

		Destroy(handSlots[givenIndexInHand]);
		//handSlots.RemoveAt(givenIndexInHand);
		handSlots[givenIndexInHand] = null;
		ShiftHandSlots();

	}

	public void ShiftHandSlots()
	{
		for(int i = 0; i < handCount; i++)
		{
			if(handSlots[i] != null)
			{
				float y = handSlots[i].transform.position.y;
			}
		}
		float maxX = 9.2f;
		float relativeX = maxX / handSlots.Count;

		//Debug.Log(maxX + " is divided by " + handSlots.Count + " and result is " + relativeX);

		//handSlots[handSlots.Count - 1].transform.position = new Vector3(-3.2f + handSlots.Count, y, 0f);

		/*if(handSlots.Count > 7)
		{
			handSlots[0].transform.position -= new Vector3(-5f, 0, 0);
		}*/


		/*if(handSlots.Count > 3)
		{
			for(int i = 0; i < handSlots.Count; i++)
			{
				if(handSlots[i] != null)
				{
					handSlots[i].transform.position += new Vector3(-3.2f + handSlots.Count, y, 0f);
				}
			}
		}*/
			//for(int i = 0; i < handSlots.Count; i++)
			//{

				//handCards[i].transform.position = handSlots[i].transform.position;

		UpdateSpriteOrder();

			//}


	}


	public void UpdateSpriteOrder(){
		for(int i = 0; i < cardObjects.Count; i++)
		{
			if(cardObjects[i] != null && !cardObjects[i].GetComponent<CardController>().played)
			{
				cardObjects[i].GetComponent<SpriteRenderer>().sortingOrder = cardObjects[i].GetComponent<Card>().indexInHand;
				cardObjects[i].GetComponent<DragCard>().initialPos = handSlots[i].transform.position;
				cardObjects[i].transform.position = handSlots[i].transform.position;
			}
		}

	}


	public void PickUp(Card card)
	{
		int removedIndex = hiddenCards.IndexOf(card.gameObject);
		hiddenSlots.RemoveAt(removedIndex);
		hiddenCards.Remove(card.gameObject);
		for(int i = 0; i < hiddenCards.Count; i++)
		{
			hiddenCards[i].GetComponent<Card>().SetHiddenRevealable(true, false);
		}

		DrawCard(false, false, card);
		card.SetSprite();
	}



	public void DrawCard(bool isNew, bool isHidden, Card card){


		Card newCard = new Card();

		//Case 1: If drawn from deck, remove that card from deck
		if(isNew || isHidden)
		{
			int randomCard = Random.Range(0, gameHandler.deck.Count);
			newCard = gameHandler.deck[randomCard].GetComponent<Card>();
			Destroy(gameHandler.deck[randomCard]);
			gameHandler.deck.RemoveAt(randomCard);
			ui.UpdateDeckText(gameHandler.deck.Count);
		}


		//Case 2: If picked up from played pile
		else {
			newCard = card;
			cardObject = card.gameObject;
		}
			
		//Initialize hand slot index
		int handSlotIndex = 0;


		//Case 1: If drawing from deck or picking up, check all hand slots if any of them is empty and position card to that hand slot
		if(isNew && !isHidden || !isNew)
		{
			
			handSlots.Add(null);

			for(int i = 0; i < handSlots.Count; i++) //Hand slot count is always 3 in the beginning
			{
				
				if(handSlots[i] == null)
				{
					CreateHandSlot();
					slotToGoTo = handSlots[i].transform.position;
					handSlotIndex = i;
					if(!isNew) card.NewIndexInHand(handSlotIndex);  //If picked up, reinstate the index in hand
					break;

				}
				else {

				}

			}


		}


		//Case 2: If card is hidden, set slot to go as the next hidden slot
		else if(isHidden)
		{
			for(int i = 0; i < hiddenSlots.Count; i++)
			{
				slotToGoTo = hiddenSlots[i].transform.position;
			}
		}


		//Case 1: If drawn from deck, instantiate new card
		if(isNew || isHidden)
		{
			Vector3 deckPosition = GameObject.Find("Deck").transform.position;

			cardObject = Instantiate(Resources.Load("Card"), deckPosition, this.transform.rotation) as GameObject;

			cardObject.GetComponent<Card>().SetStats(newCard.number, newCard.suit, newCard.sprite, handSlotIndex, isHidden);
			cardObject.GetComponent<Card>().SetOwner(this.gameObject);
			cardObject.GetComponent<DragCard>().initialPos = deckPosition;

			//animations.Animate(gameObject, slotToGoTo);
		}


		//Case 2: If picked up, position existing card back to hand slot
		else {
			cardObject.transform.position = handSlots[card.indexInHand].transform.position;
		}


		//If not hidden, position to hand slot
		if(!isHidden)
		{
			AddToHand();
			cardObject.GetComponent<DragCard>().initialPos = handSlots[handSlotIndex].transform.position;
			handCards.Add(cardObject);

		}

		//If hidden, position to hidden slot and change sprite
		else {
			cardObject.GetComponent<DragCard>().initialPos = slotToGoTo;
			cardObject.GetComponent<SpriteRenderer>().sprite = gameHandler.cardBack;
			hiddenCards.Add(cardObject);
		}

		if(cardObject.GetComponent<Card>().owner == GameObject.Find("AIController"))
		{
			cardObject.GetComponent<SpriteRenderer>().sprite = gameHandler.cardBack;
		}
	}
}
