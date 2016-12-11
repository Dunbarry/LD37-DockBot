using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DistributorController : MonoBehaviour {

	public GameObject winText;
	private bool playerWon;

	CharacterController charControl;

	// Use this for initialization
	void Start () {
//		charControl = GetComponent<CharacterController> ();
		playerWon = false;
	}

	// Update is called once per frame
	void Update () {

	}

	void OnTriggerStay(Collider other){
		InitWinText ("Shipment Sorted!", other);
	}

	void InitWinText(string text, Collider other){
		if(playerWon == false){
			if(other.gameObject.CompareTag("Player")){
				CharacterController charControl = other.GetComponent<CharacterController>();
				if (charControl.won) {
					playerWon = true;

					GameObject winner = Instantiate (winText) as GameObject;
					RectTransform winnerRect = winner.GetComponent<RectTransform> ();
					winner.transform.SetParent (transform.FindChild ("DistributorCanvas"));
					winnerRect.transform.localPosition = winText.transform.localPosition;
					winnerRect.transform.localScale = winText.transform.localScale;
					winnerRect.transform.localRotation = winText.transform.localRotation;

					winner.GetComponent<Text> ().text = text;
					winner.GetComponent<Animator> ().SetTrigger ("InitWinText");
					Destroy (winner.gameObject, 5);
				}
			}
		}
	}
}
