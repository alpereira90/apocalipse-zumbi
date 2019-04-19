﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlaArma : MonoBehaviour {

	public GameObject Bala;
	public GameObject CanoDaArma;
	public AudioClip SomDeTiro;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Fire1") && Time.timeScale != 0){
			Instantiate(Bala, CanoDaArma.transform.position, CanoDaArma.transform.rotation);
			ControlaAudio.instancia.PlayOneShot(SomDeTiro);
		}
	}
}
