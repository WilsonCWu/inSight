using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TiltControls : MonoBehaviour {
	[SerializeField]
	private GameObject speakerBase;
	[SerializeField]
	private GameObject lampBase;
	[SerializeField]
	private TextMeshProUGUI debugLog;
	[SerializeField]
	private firebaseScript firebaseRef;
	private float waitTime = 0.4f;
	private int frames = 10;
	private float perFrameWait;
	private bool isRunning = false;
	// Use this for initialization
	void Start () {
		perFrameWait = waitTime / frames;
	}
	
	// Update is called once per frame
	void Update () {
		debugLog.text = Camera.main.transform.rotation.eulerAngles.ToString();
		if (Camera.main.transform.rotation.eulerAngles.z >10 &&Camera.main.transform.rotation.eulerAngles.z < 350)
		{
			SettingVolume();
		}

	}
	private void SettingVolume()
	{
		if (!isRunning)
		{
			isRunning = true;
			if (Camera.main.transform.rotation.eulerAngles.z < 180)
			{
				StartCoroutine(SettingLeft());
			}
			else {

				StartCoroutine(SettingRight());
		}
		}
	}
	IEnumerator SettingLeft()
	{

		bool isTilted = true;
		for (int i = 0; i < frames; i++)
		{
			if (!(Camera.main.transform.rotation.eulerAngles.z > 10 && Camera.main.transform.rotation.eulerAngles.z < 180))
			{
				isTilted = false;
				break;
			}
			if (lampBase.gameObject.activeSelf)
				yield return new WaitForSeconds(0.01f);
			else {
				yield return new WaitForSeconds(perFrameWait);
			}
		}
		if (isTilted)
		{
			if (lampBase.gameObject.activeSelf)
			{
				firebaseRef.SetLampBrightness(false);
			}
			else if (speakerBase.gameObject.activeSelf){
				firebaseRef.SetSpeakerVolume(false);
			}
		}
		isRunning = false;
	}
	IEnumerator SettingRight()
	{

		bool isTilted = true;
		for (int i = 0; i < frames; i++)
		{
			if (!(Camera.main.transform.rotation.eulerAngles.z > 180 && Camera.main.transform.rotation.eulerAngles.z < 350))
			{
				isTilted = false;
				break;
			}
			if (lampBase.gameObject.activeSelf)
				yield return new WaitForSeconds(0.01f);
			else {
				yield return new WaitForSeconds(perFrameWait);
			}
		}
		if (isTilted)
		{
			if (lampBase.gameObject.activeSelf)
			{
				firebaseRef.SetLampBrightness(true);
			}
			else if (speakerBase.gameObject.activeSelf)
			{
				firebaseRef.SetSpeakerVolume(true);
			}
		}
		isRunning = false;
	}
}
