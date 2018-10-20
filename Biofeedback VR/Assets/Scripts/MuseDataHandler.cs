using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Created by Josh. Handles EEG Data recieved from MuseIO.
 *
 *
 * INSTRUCTIONS to set up environment:
 * 1. Go to http://developer.choosemuse.com/tools/mac-tools/muselab and download and install MuseIO and MuseLab.
 * 2. Connect Muse headband to computer via bluetooth.
 * 3. Run this command in terminal: muse-io --device Muse-XXXX --osc osc.udp://localhost:5000, where Muse-XXXX is your muse device name.
 * 4. DON'T open port 5000 on MuseLab or else it will interfere with the OSC server generated here. Simply run this code, and the server
 *    should be recieving the EEG data.
 *
 */

public class MuseDataHandler : MonoBehaviour {

	public OSC osc;
	static float museDataVar = 0;
	static float normalizedMuseDataVar = 0;

	private static float maxVal = 1000f;
	private static float minVal = 0;


	void Start () {
		// Currently listens for general eeg data values.
		// Other data can be listened to through addresses found here: http://developer.choosemuse.com/tools/available-data
		osc.SetAddressHandler("/muse/elements/gamma_relative", OnReceiveEEG);
	}

	void OnReceiveEEG(OscMessage message) {
		// Get EEG data from message
		museDataVar = (message.GetFloat(0) + message.GetFloat(1) + message.GetFloat(2) + message.GetFloat(3)) / 4f;

		// Update min and max values if needed
		if (museDataVar < minVal) {
			minVal = museDataVar;
		}
		if (museDataVar > maxVal) {
			maxVal = museDataVar;
		}

		// Set normalized data variable
		normalizedMuseDataVar = Normalize(museDataVar, minVal, maxVal);
	}

    /* Normalizes input EEG data to spit out a number between 1 and 0 */
	float Normalize (float val, float min, float max) {
		return (val - min) / (max - min);
	}

	public float GetEEGData() {
		return museDataVar;
	}

	public float GetNormalizedEEGData() {
		return normalizedMuseDataVar;
	}

}