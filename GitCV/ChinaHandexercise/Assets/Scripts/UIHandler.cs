using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour {

	Text cardsLeftText;
	Text playedCardText;
	Text winText;

	// Use this for initialization
	void Awake () {
		cardsLeftText = GameObject.Find("CardsLeftText").GetComponent<Text>();
		playedCardText = GameObject.Find("PlayedCardText").GetComponent<Text>();
		winText = GameObject.Find("WinText").GetComponent<Text>();
		winText.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void UpdateDeckText(int cardsLeft){
		cardsLeftText = GameObject.Find("CardsLeftText").GetComponent<Text>();
		cardsLeftText.text = "Cards left: " + cardsLeft;
	}

	public void UpdatePlayedCardText(string number, string suit){
		playedCardText.text = number + " of " + suit;
	}

	public void DisplayWinText(string whoIsPlaying)
	{
		if(whoIsPlaying == "player")
		{
			winText.text = "You win!";
		}
		else {
			winText.text = "AI wins! :(";
		}

		winText.gameObject.SetActive(true);

	}
}
