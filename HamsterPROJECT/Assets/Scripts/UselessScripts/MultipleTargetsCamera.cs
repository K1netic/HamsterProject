using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MultipleTargetsCamera : MonoBehaviour {

    public List<Transform> targets;

    Camera cam;

    //Moving
    [SerializeField]
    float smoothTimeMovement = .5f;
    Vector2 velocity;
    Vector2 minPos;
    Vector2 maxPos;
    Vector2 minCam;
    Vector2 maxCam;
    float halfHeightCam;
    float halfWidthCam;
    Vector2 targetPos;

    //Zooming
    [SerializeField]
    float zoomOffset;
    [SerializeField]
    float smoothTimeZoom = .5f;
    [SerializeField]
    float minZoom = 30f;
    [SerializeField]
    float maxZoom = 15f;
    float velocityZoom;
    float boundsWidth;
    //float zoomMaxLimitX;
    //float zoomMinLimitX;
    float zoomMaxLimitY;
    float zoomMinLimitY;
    //bool zoomViaX;

    Vector3 gizmoCenter;
    Vector3 gizmoSize;

    private void Start()
    {
        targets = new List<Transform>();
        cam = GetComponent<Camera>();
        minPos = GameObject.Find("MinCamera").GetComponent<Transform>().position;
        maxPos = GameObject.Find("MaxCamera").GetComponent<Transform>().position;

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            targets.Add(player.GetComponent<Transform>());
        }

        //zoomMaxLimitX = maxPos.x - minPos.x;
        //zoomMinLimitX = (zoomMaxLimitX * maxZoom) / minZoom;
        zoomMaxLimitY = maxPos.y - minPos.y;
        zoomMinLimitY = (zoomMaxLimitY * maxZoom) / minZoom;
    }

    private void LateUpdate()
    {
        GetBoundsValue();
        Move();
        Zoom();
    }

    void GetBoundsValue()
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            if(targets[i] != null)
                bounds.Encapsulate(targets[i].position);
        }
        if(bounds.size.x > bounds.size.y)
        {
            boundsWidth = bounds.size.x;
            //zoomViaX = true;
        }
        else
        {
            boundsWidth = bounds.size.y;
            //zoomViaX = false;
        } 

        gizmoSize = bounds.size;

        if (targets.Count == 1)
        {
            targetPos = targets[0].position;
        }
        else
        {
            targetPos = bounds.center;
            gizmoCenter = bounds.center;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(gizmoCenter, gizmoSize);
    }

    void Move()
    {
        minCam = cam.ViewportToWorldPoint(new Vector3(0, 0, 0));
        maxCam = cam.ViewportToWorldPoint(new Vector3(1, 1, 0));
        halfHeightCam = (cam.transform.position.y - minCam.y);
        halfWidthCam = (cam.transform.position.x - minCam.x);

        if ((targetPos.y - halfHeightCam) < minPos.y)
            targetPos.y = minPos.y + halfHeightCam;
        if ((targetPos.y + halfHeightCam) > maxPos.y)
            targetPos.y = maxPos.y - halfHeightCam;
        if ((targetPos.x - halfWidthCam) < minPos.x)
            targetPos.x = minPos.x + halfWidthCam;
        if ((targetPos.x + halfWidthCam) > maxPos.x)
            targetPos.x = maxPos.x - halfWidthCam;

        transform.position = Vector2.SmoothDamp(transform.position, targetPos, ref velocity, smoothTimeMovement);
        transform.position += new Vector3(0, 0, -10);
    }

    void Zoom()
    {
        float newZoom;
        /*if (zoomViaX)
            newZoom = (boundsWidth * maxZoom) / zoomMinLimitX + zoomOffset;
        else*/
            newZoom = (boundsWidth * maxZoom) / zoomMinLimitY + zoomOffset;

        if (newZoom < maxZoom)
            newZoom = maxZoom;
        else if (newZoom > minZoom)
            newZoom = minZoom;

        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, newZoom, ref velocityZoom, smoothTimeZoom);
    }
}
