using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
                render.color = new Color(.9215686f, 0.7294118f, 0.345098f);
                break;
            case "1":
                render.color = new Color(0.8705882f, 0.282353f, 0.5137255f);
                break;
            case "2":
                render.color = new Color(0.2313726f, 0.572549f, 0.9882353f);
                break;
            case "3":
                render.color = new Color(0.4627451f, 0.7372549f, 0.2862745f);
                break;
            case "4":
                render.color = new Color(0.9098039f, 0.1176471f, 0.3176471f);
                break;
            default:
                break;
        }
    }

    void OnEnable()
    {
        render = GetComponent<SpriteRenderer>();

        if (SceneManager.GetActiveScene().name == "Menu")
            render.enabled = false;
    }

    private void Update()
    {
        if (MatchStart.gameHasStarted && render.enabled)
        {
            render.enabled = false;
        }
        else if (!MatchStart.gameHasStarted)
        {
            render.enabled = true;
        }
    }
}
