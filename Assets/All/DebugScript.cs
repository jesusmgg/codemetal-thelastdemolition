using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugScript : MonoBehaviour {

	private TextParser parser;

	// Use this for initialization
	void Start () {
		parser = GameObject.Find ("Dialogues").GetComponent<TextParser> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.C)) {
			parser.Pasar (36);
		}
	}
}
