using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SetLabelData : MonoBehaviour {
	public TextMeshPro[] textboxes;
	public GameObject label;
	private float time = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!transform.GetChild(0).gameObject.activeSelf)
		{
			time += Time.deltaTime;
			if (time > 10)
			{
				transform.gameObject.SetActive(false);
			}
		}
	}
	public void SetInfo(string s)
	{
		
		foreach (TextMeshPro i in textboxes)
		{
			i.text = s;
			//Debug.Log(i.renderer.bounds.size.x);
			label.transform.localScale = new Vector3(label.transform.localScale.x + (s.Length-8)*1.5f , label.transform.localScale.y, label.transform.localScale.z);
		}
	}
}
