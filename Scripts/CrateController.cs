using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CrateController: MonoBehaviour {

	public GameObject CrateContentsText;
//	public GameObject CrateContentsText;

	public string crateContents;
//	var arr3 = new int[] { 1, 2, 3 };
	string[] gases = new string[] {"C", "N", "O", "P", "S", "Se"};

	// Use this for initialization
	void Start () {
		crateContents = gases[Random.Range (0, 5)];
	}

	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter(Collider other)
	{
		Debug.Log ("Collider Entered");
		CrateCommand (other);
	}

	void OnTriggerStay(Collider other){
		CrateCommand (other);
	}

	void CrateCommand(Collider other)
	{
//		if (other.gameObject.CompareTag ("Player")) {
//			GameObject opener = Instantiate (
		if(Input.GetKeyDown("e")){
			Debug.Log("Opening the crate!");
			InitCrateContentsText (crateContents);
		}
	}

	void InitCrateContentsText(string text)
	{
		GameObject temp = Instantiate (CrateContentsText) as GameObject;
		RectTransform tempRect = temp.GetComponent<RectTransform> ();
		temp.transform.SetParent(transform.FindChild("CrateCanvas"));
		tempRect.transform.localPosition = CrateContentsText.transform.localPosition;
		tempRect.transform.localScale = CrateContentsText.transform.localScale;
		tempRect.transform.localRotation = CrateContentsText.transform.localRotation;

		temp.GetComponent<Text>().text = text;
		temp.GetComponent<Animator> ().SetTrigger ("CrateCommand");
		Destroy (temp.gameObject, 2);
	}
}
