using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour {
	[SerializeField]
	private RectTransform rect;
	[SerializeField]
	private LineRenderer lineRenderer;
	[SerializeField]
	private GameObject speakerBase;
	[SerializeField]
	private GameObject lampBase;
	private RectTransform myRect;
	public GameObject activeItem;
	public Ray ray
	{
		get
		{
			return Camera.main.ScreenPointToRay(new Vector3(endMarker.x + 300, endMarker.y + 300, endMarker.z));
		}
	}

	private Collider oldCollider;
	private float timeOldCollider = 0;
	public Vector3 endMarker;
	private float speed = 1000F;
	private float startTime;
	private float journeyLength;
	void Start()
	{
		myRect = transform.GetComponent<RectTransform>();
		SetCursorPos(0.5f, 0.5f);
	}
	void Update()
	{
		float distCovered = (Time.time - startTime) * speed;
		float fracJourney = distCovered / journeyLength;
		transform.position = Vector3.Lerp(transform.position, endMarker, fracJourney);
		//collider stuff
		RaycastHit hit;
		Ray outRay = new Ray(Camera.main.transform.position,Camera.main.transform.forward);
		if (Physics.Raycast(outRay, out hit)){
			if (hit.collider != null)
			{
				string tempName = hit.collider.gameObject.GetComponent<name>().myName;
				if (tempName == "loudspeaker")
				{
					speakerBase.SetActive(true);
					lampBase.SetActive(false);
				}
				else if (tempName == "lamp")
				{
					speakerBase.SetActive(false);
					lampBase.SetActive(true);
				}
				else {
					if (oldCollider == null)
					{
						oldCollider = hit.collider;
					}
					else if (oldCollider == hit.collider)
					{
						timeOldCollider += Time.deltaTime;
						if (timeOldCollider > 1.5f)
						{
							hit.collider.transform.GetChild(0).GetComponent<Bing>().TurnOn();
						}
					}
					else {
						oldCollider = null;
						timeOldCollider = 0;
					}
				}
			}
		}
	}
	public void SetCursorPos(float x, float y)
	{
		endMarker = new Vector3(rect.sizeDelta.x * x - 300, rect.sizeDelta.y * (1 - y) - 300,0);
		startTime = Time.time;
		journeyLength = Vector3.Distance(transform.position, endMarker);
		//lineRenderer.SetPosition(0, cursorRay.origin);
		//lineRenderer.SetPosition(1,cursorRay.origin + cursorRay.direction*5);
		//Debug.Log(cursorRay.direction.magnitude);
	}
}
