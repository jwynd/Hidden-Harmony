using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotePlay : MonoBehaviour {

    public AudioClip audioClip;    // The track associated with this note
    private AudioSource aS;        // Audio Source reference, used to play, pause, and manage the audio

	// Use this for initialization
	void Start () {
        aS = gameObject.GetComponent<AudioSource>();

        // Testing, can be deleted 
        playClip(); // shouldn't work and should print an error message
        addClip(audioClip); // ideally use this from whatever code instantiates this
        playClip();
    }
	
    //Loads the audio clip you input. Hopefully used by other code
    public void addClip(AudioClip inputClip) {
        audioClip = inputClip;
        aS.clip = audioClip;
    }

    // Checks if you have an inputted clip and plays it
    public void playClip() {
        if(aS.clip != null) {
            aS.Play();
        } else {
            print("no audio clip buddy :( Try using the addClip function with an AudioClip");
        }
    }
}
