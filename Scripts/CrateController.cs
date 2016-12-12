using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CrateController: MonoBehaviour {

	public GameObject CrateContentsText;
	public GameObject CrateActionText;
	private GameObject opener;

	public string crateContents;
//	Set the contents of the crates
//	string[] gases = new string[] {"N", "Ar", "O", "P", "K", "He"};

	private string crateAction;

	// Use this for initialization
	void Start () {
//		crateContents = gases[Random.Range (0, 5)];
		crateAction = "Q";
	}

	void OnTriggerEnter(Collider other)
	{
		//Show the player how to open a crate, listen for if they do
		CrateCommand (other);
		CrateApproach (other);
	}

	void OnTriggerStay(Collider other){
//		Listen for if they do
		CrateCommand (other);
	}

	void CrateCommand(Collider other)
	{
		if (Input.GetKeyDown ("q")) {
//Release the Kraken! I mean...crate
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
			Destroy (opener.gameObject, 0);
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
