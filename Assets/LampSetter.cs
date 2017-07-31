using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampSetter : MonoBehaviour {
	[SerializeField]
	private MeshRenderer lamp;
	
	// Update is called once per frame
	void Update () {
		
	}
	public void set(bool isOn)
	{
		if (isOn) lampOn();
		else lampOff();
	}
	public void lampOn()
	{
		lamp.material.color = new Color(1,1,0.75f);
	}
	public void lampOff()
	{
		lamp.material.color = new Color(0.4f,0.4f,0.4f);
	}
}
