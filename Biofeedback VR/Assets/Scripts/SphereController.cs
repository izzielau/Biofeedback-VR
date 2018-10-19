using System;
using UnityEngine;
using UnityEngine.UI;

/* Created by Josh. Controls scale and color of an experimental sphere based on the user's EEG data. */
public class SphereController : MonoBehaviour {
	public MuseDataHandler M;
	public Text EEGText;
	float EEGDataVal;
	float EEGBufferedDataVal;

	float bufferDelta; // Stores speed at which sphere grows/shrinks

	private float sphereScale;

	// Use this for initialization
	void Start () {
		sphereScale = 1;
		EEGDataVal = 0;
		EEGBufferedDataVal = 0;
		bufferDelta = 0.03f;
	}
	
	// Update is called once per frame
	void Update () {
		RecieveAndBufferData();
		DisplayData();
		UpdateSphereScale();
		UpdateSphereColor();
	}

	void RecieveAndBufferData() {
		try {
			// Store data from muse data handler into local variable
			EEGDataVal = M.GetEEGData();
		} catch (Exception e) {
			Debug.LogWarning("Failed to read EEG data.");
			Debug.LogWarning(e);
		}

		// Adjust the buffer based on the value of EEGDataVal
		if (EEGDataVal > EEGBufferedDataVal) {

			EEGBufferedDataVal += bufferDelta;

		} else if (EEGDataVal < EEGBufferedDataVal) {

			// Decrement by buffer decrease each loop
			EEGBufferedDataVal -= bufferDelta;
		}
	}

	void DisplayData() {
		try {
			if (EEGText) {
				EEGText.text = "EEG: " + M.GetEEGData();
			}
		} catch (Exception e) {
		}
	}

	void UpdateSphereScale() {
		// Random scaled value
		float newScale = 1f + EEGBufferedDataVal * 2f;

		// Set new scale
		this.transform.localScale = new Vector3(newScale, newScale, newScale);
	}

	void UpdateSphereColor() {
		//Fetch the Renderer from the GameObject
        Renderer rend = this.GetComponent<Renderer>();

		Color c = new Color(EEGBufferedDataVal / 2f, EEGBufferedDataVal / 2f, 0.6f + EEGBufferedDataVal / 2f, 1);
        //Set the main Color of the Material
        rend.material.color = c;
	}
}
