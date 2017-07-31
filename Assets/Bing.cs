using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
public class Bing : MonoBehaviour {
	[SerializeField]
	private name nameField;
	private float timer = 8f;
	// Use this for initialization
	void Start () {
        //Debug.Log("This started");
        StartCoroutine(GetText());
	}
	public void TurnOn()
	{
		gameObject.SetActive(true);
		timer = 8f;
	}
    IEnumerator GetText()
    {
		using (UnityWebRequest www = UnityWebRequest.Get("https://api.cognitive.microsoft.com/bing/v5.0/search?q=" + nameField.myName+"+wiki&count=1"))
        {
            www.SetRequestHeader("Ocp-Apim-Subscription-Key", "0c7f6f23f57c447680cf03dac3a1372a");
            yield return www.Send();
            JSON js = new JSON();
            js.serialized = www.downloadHandler.text;

            JSON js2 = js.ToJSON("webPages");
            JSON js3 = js2.ToArray<JSON>("value")[0];
            string js4 = js3.ToString("snippet");

			//Debug.Log(www.downloadHandler.text);
			//Debug.Log(js4);
			this.GetComponent<TextMeshPro>().text = js4;

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }
    }

	// Update is called once per frame
	void Update () {
		if (timer < 0)
		{
			gameObject.SetActive(false);
		}
		else {

		timer -= Time.deltaTime;
		}
	}
}
