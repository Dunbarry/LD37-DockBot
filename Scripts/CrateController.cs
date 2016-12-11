using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CrateController: MonoBehaviour {

	public GameObject CrateContentsText;
	public GameObject CrateActionText;
	private GameObject opener;

	public string crateContents;
//	var arr3 = new int[] { 1, 2, 3 };
	string[] gases = new string[] {"C", "N", "O", "P", "S", "Se"};

	private string crateAction;

	// Use this for initialization
	void Start () {
		crateContents = gases[Random.Range (0, 5)];
		crateAction = "Q";
	}

	void OnTriggerEnter(Collider other)
	{
		Debug.Log ("Collider Entered");
		CrateCommand (other);
		CrateApproach (other);
	}

	void OnTriggerStay(Collider other){
		CrateCommand (other);
	}

	void CrateCommand(Collider other)
	{
		if (Input.GetKeyDown ("q")) {
			Debug.Log ("Opening the crate!");
			InitCrateContentsText (crateContents);
		}
	}

	void CrateApproach(Collider other)
	{
		if (other.gameObject.CompareTag ("Player")) {
			GameObject opener = Instantiate (CrateActionText) as GameObject;
			RectTransform openerRect = opener.GetComponent<RectTransform> ();
			opener.transform.SetParent (transform.FindChild ("CrateCanvas"));
			openerRect.transform.localPosition = CrateContentsText.transform.localPosition;
			openerRect.transform.localScale = CrateContentsText.transform.localScale;
			openerRect.transform.localRotation = CrateContentsText.transform.localRotation;

			opener.GetComponent<Text> ().text = crateAction;
			opener.GetComponent<Animator> ().SetTrigger ("OnTriggerEnter");
			Destroy (opener.gameObject, 2);
		} else if (Input.GetKeyDown ("q")) {
			Destroy (opener.gameObject);
		}
	}

	void InitCrateContentsText(string text)
	{
		GameObject temp = Instantiate (CrateContentsText) as GameObject;
		RectTransform tempRect = temp.GetComponent<RectTransform> ();
		temp.transform.SetParent(transform.FindChild("CrateCanvas"));
		tempRect.transform.localPosition = CrateActionText.transform.localPosition;
		tempRect.transform.localScale = CrateActionText.transform.localScale;
		tempRect.transform.localRotation = CrateActionText.transform.localRotation;

		temp.GetComponent<Text>().text = text;
		temp.GetComponent<Animator> ().SetTrigger ("CrateCommand");
		Destroy (temp.gameObject, 2);
	}
}
