using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionChecker : MonoBehaviour {
	[SerializeField]
	private RectTransform cursorRect;
	public Ray cursorRay;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		cursorRay = Camera.main.ScreenPointToRay(cursorRect.sizeDelta);
	}
}
