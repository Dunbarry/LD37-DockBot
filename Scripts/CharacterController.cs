using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
	public  int cratesRemaining;
	public string reservoir;
	public string secondReservoir;
	public bool won;
	private bool match;
	private bool compare;
	public GameObject lastCrate;
	public GameObject currentCrate;

	public Text FRtext;
	public Text SRtext;
	public Text Arrow;

//	UI
	private GameObject winner;
	private string storeReservoirOne;
	private string storeReservoirTwo;

	void Start()
	{
//		Movement
		targetRotation = transform.rotation;
		if (GetComponent<Rigidbody> ())
			rBody = GetComponent<Rigidbody> ();
		else
			Debug.LogError ("The character needs a rigid body.");
		// jumpInput
		forwardInput = turnInput = 0;

//		Inventory
		compare = false;
		match = false;
		reservoir = secondReservoir = "--";
		cratesRemaining = 12;

		setScoreBoard ();
		won = false;
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
//		statusCheck ("Shipment sorted!");

	}

	void FixedUpdate()
	{
		Run();
		// jump();

		rBody.velocity = transform.TransformDirection(velocity);
	}

	void Run()
	{
		if (Mathf.Abs (forwardInput) > inputSetting.inputDelay) {
			// move
			velocity.z = moveSetting.forwardVel * forwardInput;
		} else {
			// zero velocity
			velocity.z = 0;
		}
		transform.GetComponent<Animator> ().SetTrigger ("Run");
	}

	void Turn()
	{
		if (Mathf.Abs (turnInput) > inputSetting.inputDelay)
		{
			targetRotation *= Quaternion.AngleAxis (moveSetting.rotateVel * turnInput * Time.deltaTime, Vector3.up);
		}
		transform.rotation = targetRotation;
	}

//	Checking crate contents & Filling Reservoirs!
	void inventoryReset()
	{
		match = false;
		compare = false;
		reservoir = secondReservoir = "--";
		setScoreBoard ();
//		Debug.Log("Inventory Reset");
	}

	void mismatch()
	{
		match = false;
		compare = false;
		FRtext.text = storeReservoirOne;
		SRtext.text = storeReservoirTwo;
		textToRed ();
		reservoir = secondReservoir = "--";
	}

	void unpackCrate(Collider other)
	{
		transform.GetComponent<Animator> ().SetTrigger ("UnpackCrate");
		CrateController crateCon = other.GetComponent<CrateController> ();
		if (compare == false) {
			reservoir = storeReservoirOne = crateCon.crateContents;
			setScoreBoard ();
			compare = true; //Check the next crate against this one.
		} else {
			secondReservoir = storeReservoirTwo = crateCon.crateContents;
			setScoreBoard ();
			if (reservoir == secondReservoir) {
//				Debug.Log ("Match!");
				match = true;
				textToGreen ();
			} else if (reservoir != secondReservoir){
//				Debug.Log ("Not a Match!");
				mismatch ();
			}
		}
	}

	void Listener(Collider other)
	{
		if (!won) {
			if (match == false) {
				if (other.gameObject.CompareTag ("Crate")) {
					if (Input.GetKeyDown ("q")) {
						if (other.gameObject != currentCrate) {
							lastCrate = currentCrate;
							currentCrate = other.gameObject;
							unpackCrate (other);
						}
					}
				} else if (other.gameObject == currentCrate) {
//					Debug.Log ("Crate already opened");
				}
			} else if (match == true) {
//				Debug.Log ("Deliver your elements first!");

			}
		}
	}

//Delivering Elements! ~~~~~~~~~~~~~~~
	void winCondition()
	{
		cratesRemaining = cratesRemaining - 2;
		Debug.Log ("Crates remaining: "+ cratesRemaining);
		if (cratesRemaining == 0) {
			won = true;
			Debug.Log ("You Win!");
		} else if (cratesRemaining < 0)
			cratesRemaining = 0;
	}

	void Delivery(Collider other)
	{
		if (!won) {
			if (other.gameObject.CompareTag ("Distributor")) {
				if (Input.GetKeyDown ("q")) {
					if (match) {
						Destroy (lastCrate);
						Destroy (currentCrate);
						inventoryReset ();
						winCondition ();
					}
				}
			}
		}
	}

	void OnTriggerStay(Collider other)
	{
		Listener(other);
		Delivery (other);
	}

//	Scoreboard
	void setScoreBoard(){
		FRtext.text = reservoir;
		SRtext.text = secondReservoir;
		Arrow.text = " ";

		FRtext.color = Color.blue;
		SRtext.color = Color.blue;
		Arrow.color = Color.blue;
	}

	void textToRed()
	{
		FRtext.color = Color.red;
		SRtext.color = Color.red;
	}

	void textToGreen()
	{
		FRtext.color = Color.green;
		SRtext.color = Color.green;
		Arrow.text = "==>";
		Arrow.color = Color.green;
	}
}
