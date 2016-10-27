using UnityEngine;
using System.Collections;

public class DragCard : MonoBehaviour {
	Card card;

	PlayerHandler player;
	GameHandler gameHandler;
	public Vector3 initialPos;
	public Quaternion initialRot;
	Vector3 initialScale;
	CardController cardController;
	bool isDragging;
	bool isOverPlayArea;
	public bool overSprite;
	float rotation;

	void Awake(){
		card = GetComponent<Card>();
		cardController = GetComponent<CardController>();

		if(!card.isHidden) cardController.draggable = true;
		else cardController.draggable = false;

		gameHandler = GameObject.Find("GameController").GetComponent<GameHandler>();
		player = GameObject.Find("PlayerController").GetComponent<PlayerHandler>();
		initialScale = transform.localScale;
		initialPos = transform.position;
		initialRot = transform.rotation;
		rotation = .20f;
	}

	void Update () 
	{
		//Check to play the card
		if(Input.GetKeyUp(KeyCode.Mouse0) && isOverPlayArea && cardController.IsPlayable(card))
			{
				isDragging = false;
				cardController.Played();
				isOverPlayArea = false;
			}

		//Cards are draggable only if it's the player's turn, player owns it, or they're not hidden or discarded
		if(gameHandler.playersTurn || !card.isHidden || !card.isDiscarded || card.owner == GameObject.Find("PlayerController"))
		{
			cardController.draggable = true;
		}
		else {
			cardController.draggable = false;
		}

		Vector2 mousePosition = Camera.main.ScreenToWorldPoint( Input.mousePosition );
		overSprite = this.GetComponent<SpriteRenderer>().bounds.Contains( mousePosition );


		if (overSprite 
			&& !isDragging 
			&& !gameHandler.cardSelected 
			&& !card.isDiscarded)
		{

			//Enables the last card to be played
			if(player.handCount > 1 || player.hiddenSlots.Count > 0) gameHandler.cardSelected = true;


			//Highlights a hovered card in hand
			if(!cardController.played 
				&& !card.isHidden 
				&& !card.isDiscarded 
				&& card.owner == GameObject.Find("PlayerController"))
			{
				GetComponent<SpriteRenderer>().sortingOrder = 100;

				if(transform.position.y > 0) transform.position = new Vector3(transform.position.x, 1.5f, 0);
				else transform.position = new Vector3(transform.position.x, -2.5f, 0);

				transform.localScale = new Vector3(.6f, .6f, 0);
			}


			//Card is dragged from hand
			if (Input.GetKey(KeyCode.Mouse0) 
				&& cardController.draggable 
				&& !cardController.played 
				&& !card.isHidden 
				&& !card.isDiscarded 
				&& card.owner == GameObject.Find("PlayerController"))
			{
				transform.localScale = initialScale;
				transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
					Camera.main.ScreenToWorldPoint(Input.mousePosition).y,
					0.0f);
			}

		}


		else if(!overSprite && !cardController.played)
		{
			transform.position = initialPos;
			transform.localScale = initialScale;
			gameHandler.cardSelected = false;
			isOverPlayArea = false;
		}


		//Played cards / hidden cards pickup:
		if(overSprite && gameHandler.cardSelected && !card.isDiscarded && gameHandler.playersTurn)
		{
			if(Input.GetKeyDown(KeyCode.Mouse0))
			{
				if(cardController.played || card.isRevealable)
				{
					cardController.PickedUp();
				}
			}
		} 
	}

	void OnTriggerEnter2D(Collider2D c){

		if(c.gameObject.name == "PlayArea" && !cardController.played)
		{
			if(Input.GetKey(KeyCode.Mouse0))
			{
				isOverPlayArea = true;
			}
		}
	}

	void OnTriggerExit2D(Collider2D c)
	{
		if(c.gameObject.name == "PlayArea" && !cardController.played)
		{
			if(Input.GetKey(KeyCode.Mouse0))
			{
				GetComponent<SpriteRenderer>().sortingOrder = 0;
				player.UpdateSpriteOrder();
				isOverPlayArea = false;
			}
		}
	}





	public void DetermineRotation(){

		if(gameHandler.playArea.Count % 2 == 0)
		{
			rotation = -.70f;
		}
		else {
			rotation = .20f;
		}

		transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z + 12, rotation);
	}
}
