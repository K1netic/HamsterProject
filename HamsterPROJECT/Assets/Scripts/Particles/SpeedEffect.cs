using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedEffect : MonoBehaviour {

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
                ColorEffect = new Color(.784f, .451f, .173f);
                break;
            case "Perso2":
                ColorEffect = new Color(.596f, .31f, .624f);
                break;
            case "Perso3":
                ColorEffect = new Color(0.310f, 0.624f, 0.318f);
                break;
            case "Perso4":
                ColorEffect = new Color(.847f, .761f, .271f);
                break;
            case "Perso5":
                ColorEffect = new Color(.216f, .384f, .529f);
                break;
            default:
                print("Default case switch start Hook.cs");
                break;
        }
    }

	void Update (){
		transform.position = player.transform.position;

		MyRenderer.material.SetFloat ("_Emission", Emmission );
		MyRenderer.material.SetVector ("_SpeedMainTexUVNoiseZW", SpeedAnim );
		MyRenderer.material.SetColor ("_TintColor", ColorEffect );
		MyRenderer.material.SetTextureOffset ("_MainTex", MainTexOffSet);

	}

}
