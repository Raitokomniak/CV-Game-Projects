using UnityEngine;
using System.Collections;

public class AnimationHandler : MonoBehaviour {

	bool animationFinished;

	// Use this for initialization
	void Awake () {
		animationFinished = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AnimateDeckDraw(GameObject cardObject, Vector3 positionToGo)
	{
		animationFinished = false;
		StartCoroutine(DeckDraw(cardObject, positionToGo));
	}


	public bool Animate(GameObject cardObject, Vector3 positionToGo)
	{
		if(!animationFinished)
		{
			StartCoroutine(DeckDraw(cardObject, positionToGo));
			return false;
		}
		else {
		return true;
		}
	}

	IEnumerator DeckDraw(GameObject cardObject, Vector3 positionToGo)
	{

		//Vector3.Lerp(cardObject.transform.position, positionToGo, Time.deltaTime);

		//Vector3.MoveTowards(cardObject.transform.position, positionToGo, Time.deltaTime);
		yield return new WaitForSeconds(6f);

		//animationFinished = true;
	}
}
