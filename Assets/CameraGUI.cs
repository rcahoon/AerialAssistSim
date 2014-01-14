using UnityEngine;
using System.Collections;

public class CameraGUI : MonoBehaviour
{
	public Camera[] camerasLeft;
	public Camera[] camerasRight;
	public string[] names;
	
	public Camera leftCamera, rightCamera;
	
	void CameraSelection(Camera[] cameras, ref Camera selected)
	{
		for(int i=0; i < cameras.Length; ++i)
		{
			bool value = cameras[i] == selected;
			if (GUILayout.Toggle(value, names[i]) && !value)
				selected = cameras[i];
			if (cameras[i])
				cameras[i].gameObject.SetActive(value);
		}
	}
	
	void OnGUI()
	{
		GUILayout.BeginHorizontal();
			GUILayout.Space(20);
			GUILayout.BeginVertical();
				CameraSelection(camerasLeft, ref leftCamera);
			GUILayout.EndVertical();
			GUILayout.Space(700);
			GUILayout.BeginVertical();
				CameraSelection(camerasRight, ref rightCamera);
			GUILayout.EndVertical();
		GUILayout.EndHorizontal();
		
		if (rightCamera)
		{
			if (leftCamera)
			{
				leftCamera.rect = new Rect(0f, 0f, 0.5f, 1f);
				rightCamera.rect = new Rect(0.5f, 0f, 0.5f, 1f);
			}
			else
			{
				rightCamera.rect = new Rect(0f, 0f, 1f, 1f);
			}
		}
		else
		{
			if (leftCamera)
			{
				leftCamera.rect = new Rect(0f, 0f, 1f, 1f);
			}
		}
	}
}
