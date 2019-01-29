using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedEffect : MonoBehaviour {

	[SerializeField]
	MeshRenderer MyRenderer;

	float Emmission = 10;
	Vector4 SpeedAnim = new Vector4 (0f,0f,0f,1f);
	Color ColorEffect = Color.white;
	Vector2 MainTexOffSet = new Vector2 (0f, 0.1f);

	[SerializeField]
	Color Color1;
	[SerializeField]
	Color Color2;
	[SerializeField]
	Color Color3;
	[SerializeField]
	Color Color4;
	[SerializeField]
	Color Color5;

	[SerializeField]
	GameObject player;

	float SpeedScale;

    [HideInInspector]
    public Vector2 playerDirection;
    [HideInInspector]
    public float playerSpeed;


	// Use this for initialization
	void Start () {
		MyRenderer.sortingOrder = 10;
        switch (player.GetComponent<SpriteRenderer>().sprite.name)
        {
            case "Perso1":
			ColorEffect = Color1;
                break;
            case "Perso2":
			ColorEffect = Color2;
                break;
            case "Perso3":
			ColorEffect = Color3;
                break;
            case "Perso4":
			ColorEffect = Color4;
                break;
            case "Perso5":
			ColorEffect = Color5;
                break;
            default:
                print("Default case switch start Hook.cs");
                break;
        }
    }

	void Update (){


		transform.position = player.transform.position;
		transform.rotation = Quaternion.FromToRotation(Vector3.up, playerDirection);
		MyRenderer.material.SetColor ("_TintColor", ColorEffect );

		Emmission = Mathf.Clamp((playerSpeed * 6 /65), 1f, 3f);
		MyRenderer.material.SetFloat ("_Emission", Emmission );

		SpeedAnim =new Vector4(0,0,0, Mathf.Clamp((playerSpeed * 6 /65), 1f, 2f));
		MyRenderer.material.SetVector ("_SpeedMainTexUVNoiseZW", SpeedAnim );

		SpeedScale = Mathf.Clamp((playerSpeed * 3 /65), 1.2f, 1.35f);
		transform.localScale = new Vector2(SpeedScale,SpeedScale); ;

		MainTexOffSet =new Vector2 (0, (playerSpeed * 0.4f /65)-0.35f );
		MyRenderer.material.SetTextureOffset ("_MainTex", MainTexOffSet);

	}

}
