using UnityEngine;
using System.Collections;

[RequireComponent(typeof(HingeJoint))]
public class Wheel : MonoBehaviour
{
	private Vector3 neutralPosition;
  private Quaternion neutralRotation;
  public float previousAngle;
  
  static float Angle360(Vector3 v1, Vector3 v2, Vector3 n)
  {
    //  Acute angle [0,180]
    float angle = Vector3.Angle(v1,v2);

    //  -Acute angle [180,-179]
    float sign = Mathf.Sign(Vector3.Dot(n, Vector3.Cross(v1, v2)));
    return angle * sign;
  }
  
  private float GetAngle()
  {
    // return GetComponent<HingeJoint>().angle;
    Vector3 offset = transform.localRotation * Vector3.forward;
    Vector3 normal = neutralRotation * GetComponent<HingeJoint>().axis;
    offset = Vector3.ProjectOnPlane(offset, normal);
    return Angle360(offset, Vector3.forward, normal);
  }
	
	void Awake ()
	{
		neutralPosition = transform.localPosition;
    neutralRotation = transform.localRotation;
    previousAngle = GetAngle();
	}
  
  void FixedUpdate()
  {
    float angle = GetAngle();
    float delta = angle - previousAngle;
    while (delta >= 180) delta -= 360;
    while (delta < -180) delta += 360;
    Angle += delta;
    previousAngle = angle;
  }
  
  void Update()
  {
    //transform.localPosition = neutralPosition;
    //transform.localEulerAngles = Vector3.Project(transform.localEulerAngles, neutralRotation * GetComponent<HingeJoint>().axis);
  }
  
  public float Angle;
  
  public int Encoder
  {
    get
    {
      return (int)Angle;
    }
  }
	
	public void RunJoint (float speed)
	{
		float targetVel = speed;
		
		// set joint motor parameters
		JointMotor myMotor = GetComponent<HingeJoint>().motor;
		myMotor.targetVelocity = targetVel;
		GetComponent<HingeJoint>().motor = myMotor;
	}
}
