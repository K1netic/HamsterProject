using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeScript1 : MonoBehaviour {
	[SerializeField]
	Vector3 startPosition;
	[SerializeField]
	Vector3 endPosition;
	[SerializeField]
	GameObject rope;
	public GameObject connectedObject;

	private float Size;

	private int SegmentCount;
	LineRenderer lr;


	private List<GameObject> ropeSegments;
	private GameObject section;

	// Use this for initialization
	void Start () {
		lr = GetComponent<LineRenderer>();
		lr.startColor = Color.blue;
		lr.endColor = Color.blue;
		Size = rope.GetComponent<SpriteRenderer>().bounds.size.y -(0.1f*rope.GetComponent<SpriteRenderer>().bounds.size.y) ;
		SegmentCount = (int)Mathf.Round(Vector3.Distance(startPosition, endPosition) / Size);
		print (Size);
		print (SegmentCount); 
		
		ropeSegments = new List<GameObject>();
		lr.positionCount =SegmentCount;

		for (var i = 0; i < SegmentCount; i++) {
			
			section = (GameObject)Instantiate(rope);

			if (i == 0)section.GetComponent<Rigidbody2D> ().isKinematic = true;
			if(i != 0) section.GetComponent<HingeJoint2D>().connectedBody = ropeSegments[i - 1].GetComponent<Rigidbody2D>();

			section.transform.SetParent(transform);
			section.transform.localPosition = Vector3.LerpUnclamped(startPosition,endPosition, ((Size * i)/(Vector3.Distance(startPosition, endPosition))));
			ropeSegments.Add(section);
			lr.SetPosition (i, section.transform.position);

		}
	}

	void Update(){
		lr.positionCount =SegmentCount;
		int i;
		for (i = 0; i < ropeSegments.Count; i++)
		{
			lr.SetPosition (i, ropeSegments [i].transform.position);
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(transform.position + startPosition, transform.position + endPosition);
	}
}
