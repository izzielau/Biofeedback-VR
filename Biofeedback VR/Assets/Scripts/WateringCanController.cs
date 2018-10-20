using System;
using UnityEngine;
using UnityEngine.UI;
using VRTK;


/* Created by Josh. Controls the tilt of the watering can based on the user's EEG data. */
public class WateringCanController : MonoBehaviour {
	public MuseDataHandler M;
	public Text EEGText;
	float EEGDataVal;
	float EEGBufferedDataVal;
    public VRTK_SnapDropZone snapZone;

	float bufferDelta; // Stores speed at which sphere grows/shrinks

	private float wateringCanTilt;

	// Use this for initialization
	void Start () {
		wateringCanTilt = 0;
		EEGDataVal = 0;
		EEGBufferedDataVal = 0;
		bufferDelta = 0.004f;
	}
	
	// Update is called once per frame
	void Update () {
		RecieveAndBufferData();
		DisplayData();
		UpdateTilt();
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
				EEGText.text = "Gamma: " + M.GetEEGData();
			}
		} catch (Exception e) {
		}
	}

	void UpdateTilt() {
        if (snapZone.checkSnap() == true)
        {
            Debug.Log("haha");

            //       snapZone.
            // Update new tilt based on eeg value
            float newTilt = 1f + EEGBufferedDataVal * 100f;

            // Set new tilt
            this.transform.eulerAngles = new Vector3(1 + newTilt, 0, 0);
            // this.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y, this.transform.eulerAngles.z);
        }
    }
}
