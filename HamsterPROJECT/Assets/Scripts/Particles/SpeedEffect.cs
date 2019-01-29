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
		transform.rotation = Quaternion.FromToRotation(Vector3.up, playerDirection);
		MyRenderer.material.SetColor ("_TintColor", ColorEffect );

		Emmission = Mathf.Clamp((playerSpeed * 6 /65), 1f, 5f);
		MyRenderer.material.SetFloat ("_Emission", Emmission );

		SpeedAnim =new Vector4(0,0,0, Mathf.Clamp((playerSpeed * 6 /65), 1f, 6f));
		MyRenderer.material.SetVector ("_SpeedMainTexUVNoiseZW", SpeedAnim );

		SpeedScale = Mathf.Clamp((playerSpeed * 3 /65), 1.2f, 1.35f);
		transform.localScale = new Vector2(SpeedScale,SpeedScale); ;

		MainTexOffSet =new Vector2 (0, (playerSpeed * 0.4f /65)-0.6f );
		MyRenderer.material.SetTextureOffset ("_MainTex", MainTexOffSet);

	}

}
