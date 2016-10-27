using UnityEngine;
using System.Collections;

public class ParticleTest : MonoBehaviour {

	ParticleSystem part;
	bool partenabled;

	// Use this for initialization
	void Start () {
		part = GetComponent<ParticleSystem>();
		partenabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(partenabled)
		{
		emitParticles();
		}
	}

	void emitParticles(){
		part.Emit(100);
		partenabled = false;
	}
}
