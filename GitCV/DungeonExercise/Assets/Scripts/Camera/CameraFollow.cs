using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {


	//Camera follow player
	public Transform target;
	public float smoothing = 5f;
	public float range = 16f;

	//Player visibility
	public float obstacleAlpha = .1f;


	//Camera follow player
	PlayerMovement playermovement;
	Vector3 offset;

	//Player visibility
	int environmentMask;
	int floorMask;
	Vector3 playerposition;
	Collider store = null;
	bool obstacleFlag;

	Color transparent;// = new Color(.5f, .5f, .5f, .5f);
	Color original;// = new Color(.5f, .5f, .5f, 1f);

	GameObject toRender;






	void Start () {
		environmentMask = LayerMask.GetMask("EnvironmentObstacle");
		floorMask = LayerMask.GetMask("Floors");
		target = GameObject.Find("Player").transform;
		offset = transform.position - target.position;
		}
	




	void LateUpdate () {
		Vector3 targetCamPos = target.position + offset; //Sets camera target position
		transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing); //Transitions to targetposition
		//CheckPlayerVisibility(); //Checks target visibility
		}







	/// <summary>
	/// Checks if there are object in the camera's line of view
	/// </summary>
	void CheckPlayerVisibility()
	{
		
		playerposition = GameObject.Find("Player").transform.position; //Finds player's position
		Vector3 centerScreen = new Vector3(Screen.width/2, Screen.height/2, Camera.main.nearClipPlane); //Screen center (= player position)
		Ray visibilityRay = Camera.main.ScreenPointToRay(centerScreen);
		RaycastHit rayHit;
		Debug.DrawLine(transform.position, playerposition); //Ray line for debug



		if (Physics.Raycast(visibilityRay, out rayHit, range, environmentMask)) //if visibility ray hits object in environment
		{
			//Color rayHitcolor = rayHit.collider.gameObject.GetComponent<Renderer>().material.color;

			if (store != null && store != rayHit.collider) {
				RestoreEnvironmentObstacleAlpha(store.gameObject);
				obstacleFlag = false;
				}

			if(rayHit.collider != null && obstacleFlag != true) {
				store = rayHit.collider;

				//STORES OBJECT'S COLOR VALUES FOR ALPHA CHANGE
				float redValue = store.gameObject.GetComponent<Renderer>().material.color.r;
				float greenValue = store.gameObject.GetComponent<Renderer>().material.color.g;
				float blueValue = store.gameObject.GetComponent<Renderer>().material.color.b;

				//STORES OBJECT'S COLOR FOR RESTORATION
				original = store.gameObject.GetComponent<Renderer>().material.color;

				obstacleFlag = true;

				//CHANGE ALPHA TO THE HIT COLLIDER
				ChangeEnvironmentObstacleAlpha(store, redValue, greenValue, blueValue); //change alpha to the collider that was hit
			}
		}
		else {
			if(store != null)
			{
				RestoreEnvironmentObstacleAlpha(store.gameObject);
				obstacleFlag = false;
			}
		}
	}



	/// <summary>
	/// Makes camera obstacles transparent
	/// </summary>
	/// 
	void ChangeEnvironmentObstacleAlpha(Collider col, float r, float g, float b)
	{
		/*Material mat = col.gameObject.GetComponent<SkinnedMeshRenderer>().material;
		//mat.SetFloat("_Mode", 3f);
		mat.SetInt("_SrcBLend",(int)UnityEngine.Rendering.BlendMode.SrcAlpha);
		mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
		mat.SetInt("_ZWrite", 0);
		mat.renderQueue = 3000;*/

		//Material mat = col.gameObject.GetComponent<Renderer>().materials[0];
		//Material mat2 = col.GetComponent<SkinnedMeshRenderer>().sharedMaterials[0];
		//mat2.SetFloat("_Mode", 2);

		toRender = col.gameObject;
		Renderer rend = toRender.GetComponent<Renderer>();
		transparent =  new Color(r, g, b, obstacleAlpha);
		var startColor = transparent;
		var endColor = original;
		rend.material.color = Color.Lerp(endColor, startColor, 2f);
	}

	/// <summary>
	/// Restores the environment obstacle alpha.
	/// </summary>

	void RestoreEnvironmentObstacleAlpha(GameObject affected)
	{
		Renderer rend = affected.GetComponent<Renderer>();
		var startColor = transparent;
		var endColor = original;
		rend.material.color = Color.Lerp(startColor, endColor, 2f);
	}



}
