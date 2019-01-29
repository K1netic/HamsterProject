using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseRenderer : MonoBehaviour {

	[SerializeField]
	MeshRenderer MyRenderer;
	[SerializeField]
	float Emmission = 1;
	[SerializeField]
	Vector4 SpeedAnim = new Vector4 (0f,0f,0f,1f);
	[SerializeField]
	Color ColorEffect = Color.white;
	[SerializeField]
	Vector2 MainTexOffSet = new Vector2 (0f, 0f);
	[SerializeField]
	GameObject player;



	// Use this for initialization
	void Start () {
		MyRenderer.sortingOrder = 10;
	}

	void Update (){
		transform.position = player.transform.position;

		MyRenderer.material.SetFloat ("_Emission", Emmission );
		MyRenderer.material.SetVector ("_SpeedMainTexUVNoiseZW", SpeedAnim );
		MyRenderer.material.SetColor ("_TintColor", ColorEffect );
		MyRenderer.material.SetTextureOffset ("_MainTex", MainTexOffSet);

	}

}
