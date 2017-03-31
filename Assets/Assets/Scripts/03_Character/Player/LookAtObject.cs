using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtObject : MonoBehaviour {

    public GameObject TargetObject;
    public Transform saveData;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        this.gameObject.transform.LookAt(TargetObject.transform);
        
    }
}
