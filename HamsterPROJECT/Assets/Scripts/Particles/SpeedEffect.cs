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

	Color ColorEffectA;
	//Orange
	Color Color1= new Color(.9215686f, 0.7294118f, 0.345098f);
	//Pink
	Color Color2= new Color(0.9960784f, 0.5686275f, 0.7568628f);
	//Blue
	Color Color3 = new Color(0.2313726f, 0.572549f, 0.9882353f);
	//Green
	Color Color4= new Color(0.4627451f, 0.7372549f, 0.2862745f);
	//Red
	Color Color5= new Color(0.9098039f, 0.1176471f, 0.3176471f);

	[SerializeField]
	GameObject player;

	float SpeedScale;
//	float t;

    [HideInInspector]
    public Vector2 playerDirection;
    [HideInInspector]
    public float playerSpeed;

	[SerializeField] Material speedEffect;

	// Use this for initialization
	void Start () {

		MyRenderer.material = speedEffect;

		MyRenderer.sortingOrder = 10;
        switch (player.GetComponent<SpriteRenderer>().sprite.name)
        {
            case "0":
			ColorEffect = Color1;
                break;
            case "1":
			ColorEffect = Color2;
                break;
            case "2":
			ColorEffect = Color3;
                break;
            case "3":
			ColorEffect = Color4;
                break;
            case "4":
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


//		t = Mathf.Clamp((playerSpeed * 1 /120), 0f, 1f);
		ColorEffectA =ColorEffect;
		MyRenderer.material.SetColor ("_TintColor", ColorEffectA );

		Emmission = Mathf.Clamp((playerSpeed * 6 /65), 1f, 8f);
		MyRenderer.material.SetFloat ("_Emission", Emmission );

		SpeedAnim =new Vector4(0,0,0, Mathf.Clamp((playerSpeed * 6 /65), 1f, 1f));
		MyRenderer.material.SetVector ("_SpeedMainTexUVNoiseZW", SpeedAnim );

		SpeedScale = Mathf.Clamp((playerSpeed * 3 /65), 1.2f, 1.35f);
		transform.localScale = new Vector2(SpeedScale,SpeedScale); ;

		MainTexOffSet =new Vector2 (0, (playerSpeed * 0.4f /65)-0.73f );
		MyRenderer.material.SetTextureOffset ("_MainTex", MainTexOffSet);

	}

}
