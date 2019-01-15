using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MultipleTargetsCamera : MonoBehaviour {

    List<Transform> targets;

    [SerializeField]
    float smoothTime = .5f;
    [SerializeField]
    float minZoom = 35f;
    [SerializeField]
    float maxZoom = 15f;

    Vector2 velocity;

    Camera cam;

    float boundsWidth;
    Vector2 targetPos;

    private void Start()
    {
        targets = new List<Transform>();
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            targets.Add(player.GetComponent<Transform>());
        }
        if (targets.Count == 0)
            return;

        GetBoundsValue();
        Move();
        Zoom();
    }

    void GetBoundsValue()
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        boundsWidth = bounds.size.x;

        if (targets.Count == 1)
        {
            targetPos = targets[0].position;
        }
        else
        {
            targetPos = bounds.center;
        }
    }

    void Move()
    {
        transform.position = Vector2.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
        transform.position += new Vector3(0, 0, -10);
    }

    void Zoom()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, boundsWidth / 30f);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newZoom, Time.deltaTime);
    }
}
