using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {
	
	public Transform respawnPoint;
	public string message;
	public GUIStyle messageStyle;
	bool messageShowing = false;
	
	void OnGUI()
	{
		if (messageShowing)
		{
			GUILayout.Label(message, messageStyle);
		}
	}
	
	void OnTriggerEnter(Collider c)
	{
		if (c.tag == "Ball")
		{
			StartCoroutine(RespawnBall(c));
		}
	}
	
	IEnumerator RespawnBall(Collider c)
	{
		messageShowing = true;
		yield return new WaitForSeconds(2f);
		
		messageShowing = false;
		
		c.transform.position = respawnPoint.position;
		c.rigidbody.velocity = Vector3.zero;
		c.rigidbody.angularVelocity = Vector3.zero;
	}
}
