using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour {

    private Vector2 velocity;

    Transform[] players;
    float minX;
    float maxX;
    float minY;
    float maxY;

    Camera cam;

    Vector3 targetPos;
    Vector3 originalZ;

    [SerializeField]
    float smoothTime = 1;
    [SerializeField]
    bool bounds = true;

    Vector2 minCameraPos;
    Vector2 maxCameraPos;

    Vector3 bottomLeftPos;
    Vector2 UpperRightPos;

    private void Start()
    {
        cam = GetComponent<Camera>();
        minCameraPos = GameObject.Find("MinCamera").transform.position;
        maxCameraPos = GameObject.Find("MaxCamera").transform.position;

        originalZ = new Vector3(0, 0, -10);
        players = new Transform[4];
        for (int i = 0; i < GameObject.FindGameObjectsWithTag("Player").Length; i++)
        {
            players[i] = GameObject.FindGameObjectsWithTag("Player")[i].GetComponent<Transform>();
        }
    }

    private void FixedUpdate()
    {
        CameraCorner();
        CalculateBounds();
        MoveCamera();
    }

    void CameraCorner()
    {
        
    }

    void CalculateBounds()
    {
        minX = Mathf.Infinity;
        maxX = -Mathf.Infinity;
        minY = Mathf.Infinity;
        maxY = -Mathf.Infinity;
        foreach (Transform player in players)
        {
            if (player != null)
            {
                Vector2 tempPlayer = player.TransformDirection(player.position);

                //X Bounds
                if (tempPlayer.x < minX)
                    minX = tempPlayer.x;
                if (tempPlayer.x > maxX)
                    maxX = tempPlayer.x;

                //Y Bounds
                if (tempPlayer.y < minY)
                    minY = tempPlayer.y;
                if (tempPlayer.y > maxY)
                    maxY = tempPlayer.y;
            }
        }

        //Create a rectangle depending of players' position
        Vector2 tmpOrigin = new Vector2(minX, minY);
        Rect tmpRect = new Rect(tmpOrigin, new Vector2(Vector2.Distance(tmpOrigin, new Vector2(maxX, minY)), Vector2.Distance(tmpOrigin, new Vector2(minX, maxY))));
        targetPos = tmpRect.center;
        targetPos += originalZ;

        Debug.DrawLine(tmpOrigin, new Vector2(maxX,minY), Color.red, 3);
        Debug.DrawLine(tmpOrigin, new Vector2(minX, maxY), Color.red, 3);
        Debug.DrawLine(Vector3.zero, targetPos, Color.black, 3);
    }

    void MoveCamera()
    {
        float posX = Mathf.SmoothDamp(transform.position.x, targetPos.x, ref velocity.x, smoothTime);
        float posY = Mathf.SmoothDamp(transform.position.y, targetPos.y, ref velocity.y, smoothTime);
        transform.position = new Vector3(posX, posY, transform.position.z);

        if (bounds)
        {
            


            transform.position = new Vector3(Mathf.Clamp(transform.position.x, minCameraPos.x, maxCameraPos.x),
                Mathf.Clamp(transform.position.y, minCameraPos.y, maxCameraPos.y),
                Mathf.Clamp(transform.position.z, -10, -10));
        }
    }
}
