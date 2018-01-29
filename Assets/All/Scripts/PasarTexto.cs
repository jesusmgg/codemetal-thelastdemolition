using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PasarTexto : MonoBehaviour {

	private TextParser parser;
	public float secondsBetweenCharacters = 0.15f;
	public GameObject dialogues;
	public GameObject receptor;

	// Use this for initialization
	void Start () {
		parser = dialogues.GetComponent<TextParser> ();
		receptor.GetComponent<Text> ().text = "";
	}
	
	// Update is called once per frame
	void Update () {
	}


}
