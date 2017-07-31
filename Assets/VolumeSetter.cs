using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeSetter : MonoBehaviour
{
	[SerializeField]
	private GameObject[] volArray;
	public void setVolume(int vol)
	{
		if (vol < 0) vol = 0;
		else if (vol > 3) vol = 3;
		for (int i = 0; i <= vol; i++)
		{
			volArray[i].SetActive(true);
		}
		for (int i = vol + 1; i <= 3; i++)
		{
			volArray[i].SetActive(false);
		}
	}
	// Use this for initialization
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{

	}
}