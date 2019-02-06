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
    public GameObject projectile;

	GameObject node;
	private float Size;
	private int SegmentCount;
	LineRenderer lr;
    Texture ropeText;
    float lineWidth;
    private List<GameObject> ropeSegments;
	private GameObject section;
    float timeBeforeDestroy;

	// Use this for initialization
	void Start () {
        //S'il y a une erreur ici s'assurer que le prefab "Balancing" est bien dans la scène
        balanceData = GameObject.Find("Balancing").GetComponent<Balancing>();

        lineWidth = balanceData.lineWidth;
        timeBeforeDestroy = balanceData.timeRopeCut;

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

            if (i == 0) section.GetComponent<HingeJoint2D>().connectedBody = projectile.GetComponent<Rigidbody2D>();
            if (i != 0) section.GetComponent<HingeJoint2D>().connectedBody = ropeSegments[i - 1].GetComponent<Rigidbody2D>();

			section.transform.SetParent(transform);
			section.transform.localPosition = Vector3.LerpUnclamped(startPosition,endPosition, ((Size * i)/(Vector3.Distance(startPosition, endPosition))));
			ropeSegments.Add(section);
			lr.SetPosition (i, section.transform.position);
		}

        Invoke("Destroy", timeBeforeDestroy);
	}

	void Update(){
		lr.positionCount =SegmentCount;
        //Gère la déformation de la texture selon la taille de la corde
        float scaleX = Vector3.Distance(lr.GetPosition(0), lr.GetPosition(ropeSegments.Count-1));
        lr.material.mainTextureScale = new Vector2(scaleX, 1f);
        int i;
		for (i = 0; i < ropeSegments.Count; i++)
		{
			lr.SetPosition (i, ropeSegments [i].transform.position);
		}
	}

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
