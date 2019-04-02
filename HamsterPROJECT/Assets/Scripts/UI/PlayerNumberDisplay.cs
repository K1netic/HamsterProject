using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNumberDisplay : MonoBehaviour {

    [SerializeField]
    Sprite P1;
    [SerializeField]
    Sprite P2;
    [SerializeField]
    Sprite P3;
    [SerializeField]
    Sprite P4;

    GameObject player;
    SpriteRenderer render;

    private void Start()
    {
        transform.localPosition = new Vector2(0, 1.75f);

        render = GetComponent<SpriteRenderer>();
        player = transform.parent.gameObject;

        switch (player.GetComponent<PlayerMovement>().playerNumber)
        {
            case "_P1":
                render.sprite = P1;
                break;
            case "_P2":
                render.sprite = P2;
                break;
            case "_P3":
                render.sprite = P3;
                break;
            case "_P4":
                render.sprite = P4;
                break;
            default:
                break;
        }

        switch (player.GetComponent<SpriteRenderer>().sprite.name)
        {
            case "0":
                render.color = new Color(.784f, .451f, .173f);
                break;
            case "1":
                render.color = new Color(.596f, .31f, .624f);
                break;
            case "2":
                render.color = new Color(0.310f, 0.624f, 0.318f);
                break;
            case "3":
                render.color = new Color(.847f, .761f, .271f);
                break;
            case "4":
                render.color = new Color(.216f, .384f, .529f);
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        if (MatchStart.gameHasStarted && render.enabled)
        {
            Destroy(gameObject);
        }
    }
}
