using UnityEngine;
using System.Collections;

public class CrateDistribution : MonoBehaviour {


	public GameObject Crates;
	public GameObject CratesContain;

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
			xCoord = Random.Range (-15, 15);
			zCoord = Random.Range (-15, 15);
			var newCrate = Instantiate (Crates, new Vector3 (xCoord, 0, zCoord), Quaternion.identity) as GameObject;
			newCrate.transform.SetParent (CratesContain.transform, false);
//			newCrate.transform.localPosition = Crates.transform.localPosition;
//			newCrate.transform.localScale = Crates.transform.localScale;
			newCrate.transform.localRotation = Crates.transform.localRotation;
		}
	}
}
