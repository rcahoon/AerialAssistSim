using UnityEngine;
using System.Collections;

public class RespawnBall : MonoBehaviour {

	public Transform[] balls;
	public Transform redRespawnPoint;
	public Transform blueRespawnPoint;
	
	void OnTriggerEnter(Collider c)
	{
		
	}
	
	void Update ()
	{
		foreach(var ball in balls)
		{
			if (ball.position.y < transform.position.y)
			{
				var respawnPoint = ball.name.ToLower().Contains("red") ? redRespawnPoint : blueRespawnPoint;
				ball.transform.position = respawnPoint.position;
				ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
				ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
			}
		}
	}
}
