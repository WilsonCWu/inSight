using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionHistory : MonoBehaviour {
	[SerializeField]
	private Cursor cursor;
	[SerializeField]
	private GameObject testObject;
	private float dur = 2f;
	private float refreshRate = 0.05f;
	public int size;
	public Ray[] spawnRotArr;
	public int curIndex;
	// Use this for initialization
	void Start () {
		curIndex = 0;
		size = (int)(dur / refreshRate);
		spawnRotArr = new Ray[size];
		for (int i = 0; i < size; i++)
		{
			spawnRotArr[i] = cursor.ray;
		}
		StartCoroutine(SavePosData());
	}
	private IEnumerator SavePosData()
	{
		while (true)
		{
			spawnRotArr[curIndex] = cursor.ray;
			curIndex = (curIndex+1)%size;
			yield return new WaitForSeconds(refreshRate);
		}
	}
	// Update is called once per frame
	void Update () {
		testObject.transform.position = spawnRotArr[(curIndex + 1) % size].origin;
		testObject.transform.rotation = Quaternion.LookRotation(spawnRotArr[(curIndex + 1) % size].direction);

	}
}
