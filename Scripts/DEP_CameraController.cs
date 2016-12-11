using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public Transform target;
	public float lookSmooth = .09f;
	public Vector3 offsetFromTarget = new Vector3(0, 4, -6);
	public float xTilt = 10;

	Vector3 destination = Vector3.zero;
	CharacterController charController;
	float rotateVel = 0;


	// Use this for initialization
	void Start () {
		SetCameraTarget (target);
	}

	// Update is called once per frame
	void Update () {

	}

	void SetCameraTarget(Transform t){
		target = t;

		if (target != null) {
			if (target.GetComponent<CharacterController> ()) {
				charController = target.GetComponent<CharacterController> ();
			} else
				Debug.LogError ("Target needs a character controller");
		}
		else
			Debug.LogError ("Camera needs a target");
	}

	void LateUpdate(){
		MoveToTarget ();
		LookAtTartget ();
	}

	void MoveToTarget(){
		destination = charController.TargetRotation * offsetFromTarget;
		destination += target.position;
		transform.position = destination;
	}

	void LookAtTartget(){
		float eulerYAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, target.eulerAngles.y, ref rotateVel, lookSmooth);
		transform.rotation = Quaternion.Euler (transform.eulerAngles.x, eulerYAngle, 0);
	}
}
