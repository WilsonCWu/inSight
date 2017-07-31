using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breathe : MonoBehaviour
{

	[SerializeField]
	private AnimationCurve motionCurve;

	[SerializeField]
	[Range (0.0f, 10.0f)]
	private float motionTime;

	[SerializeField]
	private float distance;

	private float elapsedTime;

	private Vector3 goalPosition;
	private Vector3 startPosition;
	private Vector3 originalPosition;


	void Start ()
	{
		motionTime = motionTime + Random.Range (0, 1.0f);
		elapsedTime = Random.Range (0, motionTime);
		startPosition = transform.localPosition;
		goalPosition = startPosition;
		goalPosition.y = startPosition.y - distance / 2;
		originalPosition = transform.localPosition;
	}
    public void Reset()
    {
        startPosition = transform.localPosition;
        goalPosition = startPosition;
        goalPosition.y = startPosition.y - distance / 2;
        originalPosition = transform.localPosition;
        elapsedTime = 0;


    }
    
	void Update ()
	{
		elapsedTime += Time.deltaTime;

		float percentage = (elapsedTime / motionTime);   

		float curvedValue = motionCurve.Evaluate (percentage);
		Vector3 newPos = Vector3.Lerp (startPosition, goalPosition, curvedValue);
		transform.localPosition = newPos;

		if ((elapsedTime > motionTime)) {
			startPosition = transform.localPosition;
			goalPosition.y = originalPosition.y + distance / 2;
			distance = distance * -1;
			elapsedTime = 0;
		}

	}

}