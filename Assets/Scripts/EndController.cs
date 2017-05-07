using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndController : MonoBehaviour {

    public AudioSource ohYeah;
    public AudioSource siren;
    public AudioSource ohShit;

    // Use this for initialization
    void Start () {
        ohYeah.Play();
        siren.PlayDelayed(0.8f);
        ohShit.PlayDelayed(3f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
