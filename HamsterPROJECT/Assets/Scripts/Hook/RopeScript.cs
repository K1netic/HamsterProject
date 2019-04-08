using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeScript : MonoBehaviour {

    Balancing balanceData;

    [HideInInspector]
	public Vector3 startPosition;
    [HideInInspector]
    public Vector3 endPosition;
    [HideInInspector]
    public Color color;
    [HideInInspector]
    public GameObject firstConnectedBody;
    [HideInInspector]
    public bool connectedToPlayer;
    [HideInInspector]
    public Hook hookScript;

	GameObject node;
	private float Size;
	private int SegmentCount;
	LineRenderer lr;
    Texture ropeText;
    float lineWidth;
    public List<GameObject> ropeSegments;
	private GameObject section;
    float timeBeforeDestroy;
    float cutRopeSpeed;

    // Use this for initialization
    void Start () {
        //S'il y a une erreur ici s'assurer que le prefab "Balancing" est bien dans la scène
        balanceData = GameObject.Find("Balancing").GetComponent<Balancing>();

        lineWidth = balanceData.lineWidth;
        timeBeforeDestroy = balanceData.timeRopeCut;
        cutRopeSpeed = balanceData.cutRopeSpeed;

        node = Resources.Load<GameObject>("Prefabs/Node");
		lr = GetComponent<LineRenderer>();
		Size = node.GetComponent<SpriteRenderer>().bounds.size.y -(0.1f*node.GetComponent<SpriteRenderer>().bounds.size.y) ;

        lr.GetComponent<Renderer>().material.shader = Shader.Find("Particles/Alpha Blended");
        lr.GetComponent<Renderer>().material.color = Color.black;// couleur du matérial
        ropeText = Resources.Load<Texture>("ArrowSprites/Rope");

        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;
        lr.material.SetTexture("_MainTex", ropeText);

        lr.startColor = color;
        lr.endColor = color;
        ropeSegments = new List<GameObject>();

        SegmentCount = (int)Mathf.Round(Vector3.Distance(startPosition, endPosition) / Size);
		lr.positionCount =SegmentCount;

		for (var i = 0; i < SegmentCount; i++) {
			
			section = (GameObject)Instantiate(node);

            if (i == 0) section.GetComponent<HingeJoint2D>().connectedBody = firstConnectedBody.GetComponent<Rigidbody2D>();
            if (i != 0) section.GetComponent<HingeJoint2D>().connectedBody = ropeSegments[i - 1].GetComponent<Rigidbody2D>();

			section.transform.SetParent(transform);
			section.transform.localPosition = Vector3.LerpUnclamped(startPosition,endPosition, ((Size * i)/(Vector3.Distance(startPosition, endPosition))));
			ropeSegments.Add(section);
			lr.SetPosition (i, section.transform.position);
            if (connectedToPlayer)
                section.GetComponent<Rigidbody2D>().mass = 0;
		}
        if(!connectedToPlayer)
            Invoke("Destroy", timeBeforeDestroy);
    }

	void Update(){
        //Gère la déformation de la texture selon la taille de la corde
        float scaleX = Vector3.Distance(lr.GetPosition(0), lr.GetPosition(ropeSegments.Count-1));
        lr.material.mainTextureScale = new Vector2(scaleX, 1f);

        if (connectedToPlayer)
        {
            ropeSegments[0].transform.position = Vector3.MoveTowards(ropeSegments[0].transform.position, firstConnectedBody.transform.position, cutRopeSpeed);
            for (int j = 1; j < ropeSegments.Count; j++)
            {
                ropeSegments[j].transform.position = Vector3.MoveTowards(ropeSegments[j].transform.position, ropeSegments[j].GetComponent<HingeJoint2D>().connectedBody.transform.position, cutRopeSpeed);
            }
            if(ropeSegments[0].transform.position == firstConnectedBody.transform.position)
            {
                ropeSegments[1].GetComponent<HingeJoint2D>().connectedBody = firstConnectedBody.GetComponent<Rigidbody2D>();

                ropeSegments.RemoveAt(0);
            }
            if (ropeSegments.Count <= 1)
            {
                hookScript.ropeCut = false;
                Destroy(gameObject);
            }
            for (int i = 0; i < ropeSegments.Count; i++)
            {
                lr.SetPosition(i, ropeSegments[i].transform.position);
            }
            for (int i = ropeSegments.Count; i< lr.positionCount; i++)
            {
                lr.SetPosition(i, lr.GetPosition(ropeSegments.Count - 1));
            }
        }
        else
        {
            for (int i = 0; i < ropeSegments.Count; i++)
            {
                lr.SetPosition(i, ropeSegments[i].transform.position);
            }
        }
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
