using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GrapplingHook2 : MonoBehaviour {

	public float Length = 8;

	[HideInInspector]
	public bool IsEnabled
	{
		get { return points.Any(); }
	}

	private readonly List<GameObject> points = new List<GameObject>();
	private LineRenderer line;
	private GameObject grapple;
	private GameObject previousGrapple;
	private float previousDistance = -1;
	private DistanceJoint2D joint;

	// Use this for initialization
	void Start () {
		
		line = new GameObject("Line").AddComponent<LineRenderer>();//instantie un line renderer
		line.SetVertexCount(2); //le nombre de point pour la ligne
		line.SetWidth(.025f, .025f);// la largeur de la ligne
		line.gameObject.SetActive(false);// désactive la ligne
		line.SetColors(Color.black, Color.black);// couleur
		line.GetComponent<Renderer>().material.color = Color.black;// couleur du matérial

		grapple = new GameObject("Grapple"); // instantie le game object grappin
		grapple.AddComponent<CircleCollider2D>().radius = .1f; //définie son rayon
		grapple.AddComponent<Rigidbody2D>(); //ajoute un rigidbody
		grapple.GetComponent<Rigidbody2D>().isKinematic = true;//Le met en kinématic

		previousGrapple = (GameObject)Instantiate(grapple);
		previousGrapple.name = "Previous Grapple";

		joint = gameObject.AddComponent<DistanceJoint2D>(); //ajoute un joint
		joint.enabled = false; //le désactive
	}
	
	// Update is called once per frame
	void Update()
	{
		if (IsEnabled) UpdateGrapple();
		else CheckForGrapple();
	}

	private void CheckForGrapple()
	{
		if (Input.GetMouseButtonDown(0)) // si appuie sur le bouton
		{
			var mousePosition = Input.mousePosition;
			mousePosition.z = -Camera.main.transform.position.z;
			var worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
			var grapplePoint = transform.position + (worldPosition - transform.position) * Length;// récupère la position de la souris et calcule le vector

			var hit = Physics2D.Linecast(transform.position, grapplePoint, ~(1 << 8));
			var distance = Vector3.Distance(transform.position, hit.point);
			if (hit.collider != null && distance <= Length) // si le grappin touche et que la distance est respecté
			{
				line.SetVertexCount(2);
				line.SetPosition(0, hit.point);
				line.SetPosition(1, transform.position);
				line.gameObject.SetActive(true); // active la ligne

				points.Add(CreateGrapplePoint(hit));

				grapple.transform.position = hit.point;
				SetParent(grapple.transform, hit.collider.transform);

				joint.enabled = true; // active le joint
				joint.connectedBody = grapple.GetComponent<Rigidbody2D>();
				joint.distance = Vector3.Distance(hit.point, transform.position);
				joint.maxDistanceOnly = true;
			}
		}
	}

	private GameObject CreateGrapplePoint(RaycastHit2D hit)
	{
		var p = new GameObject("GrapplePoint"); // créer un gameobject vide
		SetParent(p.transform, hit.collider.transform);
		p.transform.position = hit.point;
		return p;
	}

	private void UpdateGrapple()
	{
		UpdateLineDrawing();

		var hit = Physics2D.Linecast(transform.position, grapple.transform.position, ~(1 << 8));
		var hitPrev = Physics2D.Linecast(transform.position, previousGrapple.transform.position, ~(1 << 8));

		if (hit.collider.gameObject != grapple && hit.collider.gameObject != previousGrapple)
		{
			// if you lose line of sight on the grappling hook, then add a new point to wrap around

			points.Add(CreateGrapplePoint(hit));

			UpdateLineDrawing();

			previousGrapple.transform.position = grapple.transform.position;
			SetParent(previousGrapple.transform, grapple.transform.parent);
			grapple.transform.position = hit.point;
			SetParent(grapple.transform, hit.collider.transform);
			previousDistance = -1;

			joint.distance -= Vector3.Distance(grapple.transform.position, previousGrapple.transform.position);
		}
		else if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
		{
			// if you retract the grappling hook

			// jump off
			if (Input.GetKeyDown(KeyCode.Space) && transform.position.y < grapple.transform.position.y)
				GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 3);

			RetractRope();
		}
		else if (Vector3.Distance(grapple.transform.position, previousGrapple.transform.position) <= .1f)
		{
			RemoveLastCollider();
		}
		else
		{
			// always update the last points in the line to track player

			line.SetPosition(points.Count, transform.position);
			GetComponent<Rigidbody2D>().AddForce(Vector3.right * Input.GetAxisRaw("Horizontal_P1") * 25);
			joint.distance -= Input.GetAxisRaw("Vertical_P1") * Time.deltaTime;

			// if you can see previous point then unroll back to that point
			if (hitPrev.collider != null && hitPrev.transform == previousGrapple.transform)
				RemoveLastCollider();
		}

		UpdateDistance();
	}

	private void RetractRope()
	{
		joint.enabled = false;
		line.gameObject.SetActive(false);
		points.ForEach(Destroy);
		points.Clear();
		grapple.transform.position = new Vector3(0, 0, -1);
		previousGrapple.transform.position = new Vector3(0, 0, -1);
		previousDistance = -1;
	}

	private void RemoveLastCollider()
	{
		if (points.Count > 1)
		{
			Destroy(points[points.Count - 1]);
			points.RemoveAt(points.Count - 1);

			UpdateLineDrawing();

			joint.distance += Vector3.Distance(grapple.transform.position, previousGrapple.transform.position);
			grapple.transform.position = previousGrapple.transform.position;
			SetParent(grapple.transform, previousGrapple.transform.parent);
		}

		if (points.Count > 1)
			previousGrapple.transform.position = points.ElementAt(points.Count - 2).transform.position;
		else
			previousGrapple.transform.position = new Vector3(0, 0, -1);

		previousDistance = -1;
	}

	private void UpdateLineDrawing()
	{
		line.SetVertexCount(points.Count + 1);
		for (var i = 0; i < points.Count; i++)
			line.SetPosition(i, points[i].transform.position);
		line.SetPosition(points.Count, transform.position);
	}

	private void UpdateDistance()
	{
		if(points.Count == 0) return;

		var distance = 0f;

		for (var i = 1; i < points.Count; i++)
			distance += Vector3.Distance(points[i - 1].transform.position, points[i].transform.position);
		distance += Vector3.Distance(points[points.Count - 1].transform.position, transform.position);

		if (previousDistance > 0)
			joint.distance += previousDistance - distance;

		previousDistance = distance;

		if(distance > Length) RetractRope();
	}

	private void SetParent(Transform child, Transform parent)
	{
		child.SetParent(parent);
		if (parent != null)
			child.localScale = new Vector3(1 / parent.localScale.x, 1 / parent.localScale.y, 1 / parent.localScale.z);
	}
}
