﻿using System;
using UnityEngine;
using UnityEngine.UI;

/* Created by Josh. Controls the tilt of the watering can based on the user's EEG data. */
public class WateringCanController : MonoBehaviour {
	public MuseDataHandler M;
	public Text EEGText;
	float EEGDataVal;
	float EEGBufferedDataVal;

	float bufferDelta; // Stores speed at which sphere grows/shrinks

	private float wateringCanTilt;

	// Use this for initialization
	void Start () {
		wateringCanTilt = 0;
		EEGDataVal = 0;
		EEGBufferedDataVal = 0;
		bufferDelta = 0.008f;
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
			EEGDataVal = M.GetNormalizedEEGData();
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

	void UpdateTilt() {
		// Update new tilt based on eeg value
		float newTilt = 1f + EEGBufferedDataVal * 30f;

		// Set new tilt
		this.transform.eulerAngles = new Vector3(1 + newTilt, 0, 0);
	}
}
