using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {

	[System.Serializable]
	public class MoveSettings
	{
		public float forwardVel = 12;
		public float rotateVel = 100;
		public float distToGrounded = 0.1f;
		public LayerMask ground;
	}

	[System.Serializable]
	public class PhysSettings
	{
		public float downAccel = 0.75f;
	}

	[System.Serializable]
	public class InputSettings
	{
		public float inputDelay = 0.1f;
		public string FORWARD_AXIS = "Vertical";
		public string TURN_AXIS = "Horizontal";
		// public string JUMP_AXIS = "Jump";
	}

	public MoveSettings moveSetting = new MoveSettings();
	public PhysSettings physSetting = new PhysSettings();
	public InputSettings inputSetting = new InputSettings();

	Vector3 velocity = Vector3.zero;
	Quaternion targetRotation;
	Rigidbody rBody;
	float forwardInput, turnInput;
	// jumpInput

	public Quaternion TargetRotation
	{
		get {return targetRotation;}
	}

	bool Grounded()
	{
		return Physics.Raycast(transform.position, Vector3.down, moveSetting.distToGrounded, moveSetting.ground);
	}

//	Inventory
	public string reservoir;
	public string secondReservoir;
	private bool compare;
	private bool match;
	private GameObject opened;

	void Start()
	{
		targetRotation = transform.rotation;
		if (GetComponent<Rigidbody> ())
			rBody = GetComponent<Rigidbody> ();
		else
			Debug.LogError ("The character needs a rigid body.");

		forwardInput = turnInput = 0;
		// jumpInput
		compare = false;
		match = false;
		reservoir = secondReservoir = "empty";
	}

	void GetInput()
	{
		forwardInput = Input.GetAxis(inputSetting.FORWARD_AXIS);
		turnInput = Input.GetAxis(inputSetting.TURN_AXIS);
		// jumpInput = Input.GetAxisRaw(inputSetting.JUMP_AXIS);
	}

	void Update()
	{
		GetInput();
		Turn();

	}

	void FixedUpdate()
	{
		Run();
		// jump();

		rBody.velocity = transform.TransformDirection(velocity);
	}

	void Run()
	{
		if (Mathf.Abs(forwardInput) > inputSetting.inputDelay)
		{
			// move
			velocity.z = moveSetting.forwardVel * forwardInput;
		}
		else
			// zero velocity
			velocity.z = 0;
	}

	void Turn()
	{
		if (Mathf.Abs (turnInput) > inputSetting.inputDelay)
		{
			targetRotation *= Quaternion.AngleAxis (moveSetting.rotateVel * turnInput * Time.deltaTime, Vector3.up);
		}
		transform.rotation = targetRotation;
	}

	void unpackCrate(Collider other)
	{
		CrateController crateCon = other.GetComponent<CrateController> ();
		if (!compare) {
			reservoir = crateCon.crateContents;
			compare = true;
		} else if (compare) {
			secondReservoir = crateCon.crateContents;
			if (reservoir == secondReservoir) {
				Debug.Log ("Match!");
				match = true;
			} else if (reservoir != secondReservoir){
				Debug.Log ("Not a Match!");
				compare = false;
				reservoir = secondReservoir = "empty";
				opened = null;
			}
			Debug.Log (match);
			Debug.Log (compare);
		}
	}

	void Listener(Collider other)
	{
		if(other.gameObject.CompareTag("Crate")){
			if (Input.GetKeyDown ("q")) {
				if (other.gameObject != opened) {
					opened = other.gameObject;
					unpackCrate (other);
				} else if (other.gameObject == opened) {
					Debug.Log ("Crate already opened");
				}
			}
		}
	}


	void OnTriggerStay(Collider other)
	{
		Listener(other);
	}
}
