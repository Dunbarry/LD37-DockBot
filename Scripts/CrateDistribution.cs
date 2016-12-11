using UnityEngine;
using System.Collections;

public class CrateDistribution : MonoBehaviour {

	string[] gases = new string[] {"N", "Ar", "O", "P", "K", "He"};
	public GameObject Crates;
	public GameObject CratesContain;
//	string loadCrate;
	int loadControl;
	int count;

	private int xCoord;
	private int zCoord;

	// Use this for initialization
	void Start () {
		placeCrates ();
	}

	// Update is called once per frame
	void Update () {

	}

	void placeCrates () {
		for (int c = 0; c <= 11; c++) {
			count = c;
			if(count >= 6){
				loadControl = c - 6;
			}
			else {
				loadControl = c;
			}

			xCoord = Random.Range (-20, 20);
			zCoord = Random.Range (-20, 20);
			var newCrate = Instantiate (Crates, new Vector3 (xCoord, 0, zCoord), Quaternion.identity) as GameObject;
			newCrate.transform.SetParent (CratesContain.transform, false);
//			newCrate.transform.localPosition = Crates.transform.localPosition;
//			newCrate.transform.localScale = Crates.transform.localScale;
			newCrate.transform.localRotation = Crates.transform.localRotation;

			CrateController crateControl = newCrate.GetComponent<CrateController>();
			crateControl.crateContents = gases[loadControl];
		}
	}
}
