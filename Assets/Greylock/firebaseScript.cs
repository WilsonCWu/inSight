using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
public class VolumeData
{
	public int volume;


	public VolumeData(int vol)
	{
		this.volume = vol;
	}

	public Dictionary<string, System.Object> ToDictionary()
	{
		Dictionary<string, System.Object> result = new Dictionary<string, System.Object>();
		result["volume"] = volume;

		return result;
	}
}
public class LampData
{
	public bool isOn;


	public LampData(bool isOn)
	{
		this.isOn = isOn;
	}

	public Dictionary<string, System.Object> ToDictionary()
	{
		Dictionary<string, System.Object> result = new Dictionary<string, System.Object>();
		result["isOn"] = isOn ? 1: 0;

		return result;
	}
}
public class firebaseScript : MonoBehaviour {
	[SerializeField]
	private TextMeshProUGUI readyText;
	[SerializeField]
	private TagSpawner tagSpawner;
	private bool ready = false;
	[SerializeField]
	private Cursor cursor;
	private DatabaseReference reference;
	public int speakerVol;
	public bool lampIsOn;
	// Use this for initialization
	void Start () {
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://greylock-ee11e.firebaseio.com/");
		reference = FirebaseDatabase.DefaultInstance.RootReference;
		FirebaseDatabase.DefaultInstance
		  .GetReference("loudspeaker")
		  .GetValueAsync().ContinueWith(task =>
		  {
			  if (task.IsFaulted)
			  {
				  // Handle the error...
			  }
			  else if (task.IsCompleted)
			  {
				  DataSnapshot snapshot = task.Result;
				  speakerVol = int.Parse(snapshot.Child("volume").Value.ToString()); 
			  }
		  });
		FirebaseDatabase.DefaultInstance
		  .GetReference("lamp")
		  .GetValueAsync().ContinueWith(task =>
		  {
			  if (task.IsFaulted)
			  {
				  // Handle the error...
			  }
			  else if (task.IsCompleted)
			  {
				  DataSnapshot snapshot = task.Result;
				lampIsOn = int.Parse(snapshot.Child("isOn").Value.ToString()) != 0;
			  }
		  });

		FirebaseDatabase.DefaultInstance
		.GetReference("label")
		.ValueChanged += HandleLabelChanged;
		
		FirebaseDatabase.DefaultInstance
		.GetReference("location")
		.ValueChanged += HandleLocationChanged;
		//StartCoroutine(tester());
	}
	IEnumerator tester()
	{
		int i = 0;
		while (true)
		{
			tagSpawner.lampObject.GetComponent<LampSetter>().set(i%2 == 0);
			tagSpawner.speakerObject.GetComponent<VolumeSetter>().setVolume(i);
			i = (i + 1) % 4;
			yield return new WaitForSeconds(1);
		}
	}
	void HandleLabelChanged(object sender, ValueChangedEventArgs args)
	{
		if (args.DatabaseError != null)
		{
			Debug.LogError(args.DatabaseError.Message);
			return;
		}
		if (!ready)
		{
			ready = true;
			readyText.text = "";
		}
		else {
			DataSnapshot snapshot = args.Snapshot;
			Debug.Log(snapshot.Value);
			if (snapshot.Value.ToString() != "unknown")
			{
				tagSpawner.SpawnNewText(1f, snapshot.Value.ToString());
			}
		}
	}
	void HandleLocationChanged(object sender, ValueChangedEventArgs args)
	{
		if (args.DatabaseError != null)
		{
			Debug.LogError(args.DatabaseError.Message);
			return;
		}
		if (!ready)
		{
			ready = true;
			readyText.text = "";
		}
		else {
			DataSnapshot snapshot = args.Snapshot;
			cursor.SetCursorPos(float.Parse(snapshot.Child("x").Value.ToString()),float.Parse(snapshot.Child("y").Value.ToString()));

		}
	}
	public void SetSpeakerVolume(bool volIsUp)
	{
		FirebaseDatabase.DefaultInstance
		  .GetReference("loudspeaker")
		  .GetValueAsync().ContinueWith(task =>
		  {
			  if (task.IsFaulted)
			  {
				  // Handle the error...
			  }
			  else if (task.IsCompleted)
			  {
				 	DataSnapshot snapshot = task.Result;
				  int newVol = int.Parse(snapshot.Child("volume").Value.ToString()) + (volIsUp ? 1 : -1);

				if (newVol > 3) newVol = 3;
				  if (newVol < 0) newVol = 0;
				  speakerVol = newVol;
				VolumeData entry = new VolumeData(newVol);
					Dictionary<string, System.Object> entryValues = entry.ToDictionary();
				Dictionary<string, System.Object> childUpdates = new Dictionary<string, System.Object>();
				childUpdates["/loudspeaker"] = entryValues;
				reference.UpdateChildrenAsync(childUpdates);
				  tagSpawner.speakerObject.GetComponent<VolumeSetter>().setVolume(newVol);
				//string newVol = JsonUtility.ToJson
			  }
		  });
	}
	public void SetLampBrightness(bool isOn)
	{
		LampData entry = new LampData(isOn);
		Dictionary<string, System.Object> entryValues = entry.ToDictionary();
		Dictionary<string, System.Object> childUpdates = new Dictionary<string, System.Object>();
		childUpdates["/lamp"] = entryValues;
		reference.UpdateChildrenAsync(childUpdates);
		tagSpawner.lampObject.GetComponent<LampSetter>().set(isOn);

	}
}
