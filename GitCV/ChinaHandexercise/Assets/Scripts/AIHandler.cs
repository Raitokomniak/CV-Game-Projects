using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIHandler : MonoBehaviour {

	int difficulty;

	GameHandler gameHandler;
	PlayerHandler ai;
	bool ready;

	// Use this for initialization
	void Awake () {
		gameHandler = GameObject.Find("GameController").GetComponent<GameHandler>();
		ai = GetComponent<PlayerHandler>();
		difficulty = 0;
	} 

	public void StartTurn(){
		StartCoroutine("TurnLag");
	}

	IEnumerator TurnLag(){
		yield return new WaitForSeconds(1);
		Play();
	}


	void Play(){
		
		List<int> playableNumbers = new List<int>();
		int lastPlayed = (gameHandler.playArea.Count - 1);
		Card card = new Card();

		//If ai's hand is empty but hidden cards remain, pick up a random hidden card
		if(ai.handCards.Count == 0 && ai.hiddenCards.Count != 0)
		{
			int randomHidden = Random.Range(0, ai.hiddenCards.Count);
			ai.hiddenCards[randomHidden].GetComponent<CardController>().PickedUp();
		}

		//Check ai's hand cards for any playable cards
		for(int i = 0; i < ai.handCards.Count; i++) {

			card = ai.handCards[i].GetComponent<Card>();

			if (ai.handCards[i].GetComponent<CardController>().IsPlayable(card))
			{
				playableNumbers.Add(card.number);
			}
		}


		//If no playable numbers were found, pick up
		if(playableNumbers.Count == 0)
		{
			while(gameHandler.playArea.Count != 0)
			{
				gameHandler.playArea[gameHandler.playArea.Count - 1].GetComponent<CardController>().PickedUp();
			}
		}

		//If playable cards are found, sort them in order, find the lowest
		else {
			playableNumbers.Sort();
			Card playedCard = new Card();

			for(int i = 0; i < ai.handCards.Count; i++)
			{
				if(ai.handCards[i].GetComponent<Card>().number == playableNumbers[0])
				{
					playedCard = ai.handCards[i].GetComponent<Card>();
					ai.handCards[i].GetComponent<CardController>().Played();
					i = ai.handCards.Count;
					break;
				}
			}

			//If played card was 2 or 10, start turn over again
			if(playedCard.number == 2 || playedCard.number == 10)
			{
				StartTurn();
			}
			else {

			}
		}
	}

	void SmartToPlay(){
		
	}

}
