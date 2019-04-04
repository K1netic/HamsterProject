using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeParticlesManager : MonoBehaviour {

	//Particles GameObject
	[SerializeField]
	ParticleSystem flameLife;
	[SerializeField]
	ParticleSystem smoke;
	[SerializeField]
	ParticleSystem ember;

	//Test
	float testViePerso;
	float VieMax = 100;

	//Player Variables
	[SerializeField]
	GameObject player;
	[HideInInspector]
	public float playerHP;

	//PARTICLES VARIABLES
	//Flame
	ParticleSystem.MainModule flameLifeMain;
	ParticleSystem.EmissionModule flameLifeEmission;
	float burstIntervalFlame;
	float flameSizeMin;
	float flameSizeMax;

	//Smoke
	ParticleSystem.EmissionModule smokeEmission;
	ParticleSystem.MainModule smokeMain;
//	ParticleSystem.ColorOverLifetimeModule smokeColorOLT;
	[SerializeField]
	Gradient SmokeGrandient;
	[SerializeField]
	Color colorStart;
	[SerializeField]
	Color colorEnd;
	float smokeColor;
	float smokeEOT;
	float smokeSizeMin;
	float smokeSizeMax;


	//Ember
	ParticleSystem.EmissionModule emberEmission;
	ParticleSystem.MainModule emberMain;
	ParticleSystem.Burst emberBurst = new ParticleSystem.Burst(0f,15,30,50,0.010f);
	float emberSizeMin;
	float emberSizeMax;
	float emberSpeedMin;
	float emberSpeedMax;
	float burstIntervalEmber;
	short emberNbMin;
	short emberNbMax;

	// Use this for initialization
	void Start () {
		//Flame
		flameLifeMain = flameLife.GetComponent<ParticleSystem> ().main;
		flameLifeEmission = flameLife.GetComponent<ParticleSystem> ().emission;

		//Smoke
		smokeEmission = smoke.GetComponent<ParticleSystem> ().emission;
		smokeMain = smoke.GetComponent<ParticleSystem> ().main;
//		smokeColorOLT = smoke.GetComponent<ParticleSystem> ().colorOverLifetime;

		//Ember
		emberMain = ember.GetComponent<ParticleSystem> ().main;
		emberEmission = ember.GetComponent<ParticleSystem> ().emission;

	}
	
	// Update is called once per frame
	void Update () {
		testViePerso = VieMax - playerHP;


		//WIP VARIABLES X LIFE
		burstIntervalFlame = testViePerso * 30 /100f;
		flameSizeMin = testViePerso * 0.25f / 100f;
		flameSizeMax = testViePerso * 0.4f / 100f;

		smokeEOT = testViePerso * 30 / 100f;
		smokeSizeMin = testViePerso * 1 / 100f;
		smokeSizeMax = testViePerso * 2 / 100f;
		smokeColor = testViePerso * 1 / 100f;;

		emberSizeMin = testViePerso * 0.1f / 100f;
		emberSizeMax = testViePerso * 0.15f / 100f;
		emberSpeedMin = testViePerso * 10 / 100f;
		emberSpeedMax = testViePerso * 18 / 100f;
		burstIntervalEmber = testViePerso * 30 / 100f;
		emberNbMin = (short)(testViePerso * 15 / 100f);
		emberNbMax = (short)( testViePerso * 30 / 100f);

		//Flame
		flameLifeMain.startSize = Random.Range(flameSizeMin,flameSizeMax);
		flameLifeEmission.SetBurst (0, new ParticleSystem.Burst(0f,1,1,50,burstIntervalFlame));

		//Smoke
		smokeEmission.rateOverTime =  smokeEOT;
		smokeMain.startSize = Random.Range(smokeSizeMin,smokeSizeMax);
		smokeMain.startColor =Color.Lerp(colorStart, colorEnd, smokeColor);
	

		//Ember
		emberMain.startSize = Random.Range(emberSizeMin,emberSizeMax);
		emberMain.startSpeed = Random.Range(emberSpeedMin,emberSpeedMax);
		emberEmission.SetBurst (0, new ParticleSystem.Burst(0f,emberNbMin,emberNbMax,50,burstIntervalEmber));


		//smokeColorOLT.color = SmokeGrandient;
		//Gradient grad = new Gradient ();
		//grad.SetKeys( new GradientColorKey[] {new GradientColorKey(Color.blue, 0.0f), new GradientColorKey(Color.red, 1.0f)}, new GradientAlphaKey[]{new GradientAlphaKey(1.0f,0.0f), new GradientAlphaKey(0.0f,1.0f)});
	}
}
