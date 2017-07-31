using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TagSpawner : MonoBehaviour {
	[SerializeField]
	private Camera mainCamera;
	[SerializeField]
	private GameObject textObject;
	[SerializeField]
	private Cursor cursor;
	[SerializeField]
	private PositionHistory posHistory;
	public GameObject speakerObject;
	public GameObject lampObject;
	[SerializeField]
	private firebaseScript firebaseRef;
	private int counter = 0;
	private List<GameObject> itemList;

	// Use this for initialization
	void Start () {
		itemList = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	private IEnumerator SpawnEveryInterval(int seconds)
	{
		while (true)
		{
			yield return new WaitForSeconds(seconds);
			SpawnNewText(1, "newText: " + counter);
			counter++;
		}
	}
	public void DefaultSpawn()
	{
		SpawnNewText(1, "Item " + counter++);
	}
	public void DefaultSpeakerSpawn()
	{
		SpawnSpeaker(1, 3);
	}
	public void DefaultLampSpawn()
	{
		SpawnLamp(1, true);
	}
	public void SpawnNewText(float dist, string text)
	{
		checkExists(text);

		if (text == "loudspeaker")
		{
			SpawnSpeaker(dist, firebaseRef.speakerVol);
		}
		else if (text == "lamp")
		{
			SpawnLamp(dist, firebaseRef.lampIsOn);
		}
		else {
			if (text != "loudspeaker" && text != "lamp")
			{

				Vector3 spawnPos = posHistory.spawnRotArr[(posHistory.curIndex + 1) % posHistory.size].origin + posHistory.spawnRotArr[(posHistory.curIndex + 1) % posHistory.size].direction * dist + Vector3.up * 0.3f;
				Vector3 tempRot = mainCamera.transform.rotation.eulerAngles;
				tempRot = new Vector3(tempRot.x, tempRot.y, 0);
				Quaternion spawnRot = Quaternion.Euler(tempRot);
				GameObject temp = Instantiate(textObject, spawnPos, spawnRot);
				temp.GetComponent<SetLabelData>().SetInfo(text);
				temp.GetComponent<name>().myName = text;
				itemList.Add(temp);
			}
		}
	}
	public void SpawnSpeaker(float dist, int vol)
	{
		Vector3 spawnPos = posHistory.spawnRotArr[(posHistory.curIndex + 1) % posHistory.size].origin + posHistory.spawnRotArr[(posHistory.curIndex + 1) % posHistory.size].direction * dist + Vector3.up * 0.3f;
		//Vector3 spawnPos = mainCamera.transform.position + cursor.ray.direction * dist + Vector3.up * 0.3f;
		Vector3 tempRot = mainCamera.transform.rotation.eulerAngles;
		tempRot = new Vector3(tempRot.x, tempRot.y, 0);
		Quaternion spawnRot = Quaternion.Euler(tempRot);
		speakerObject.transform.position = spawnPos;
		speakerObject.transform.rotation = spawnRot;
		speakerObject.SetActive(true);
		//GameObject temp = Instantiate(speakerObject, spawnPos, spawnRot);
		speakerObject.GetComponent<VolumeSetter>().setVolume(vol);
	}
	public void SpawnLamp(float dist, bool isOn)
	{
		Vector3 spawnPos = posHistory.spawnRotArr[(posHistory.curIndex + 1) % posHistory.size].origin + posHistory.spawnRotArr[(posHistory.curIndex + 1) % posHistory.size].direction * dist + Vector3.up * 0.3f;
		//Vector3 spawnPos = mainCamera.transform.position + cursor.ray.direction * dist + Vector3.up * 0.3f;
		Vector3 tempRot = mainCamera.transform.rotation.eulerAngles;
		tempRot = new Vector3(tempRot.x, tempRot.y, 0);
		Quaternion spawnRot = Quaternion.Euler(tempRot);
		lampObject.transform.position = spawnPos;
		lampObject.transform.rotation = spawnRot;
		lampObject.SetActive(true);
		lampObject.GetComponent<LampSetter>().set(isOn);
	}
	private void checkExists(string name1)
	{
		foreach (GameObject i in itemList)
		{
			Debug.Log("checking: " + i.GetComponent<name>().myName);
			if (i.GetComponent<name>().myName == name1)
			{
				GameObject temp = i;
				itemList.Remove(i);
				GameObject.Destroy(temp);
				return;
			}
		}
	}
}
