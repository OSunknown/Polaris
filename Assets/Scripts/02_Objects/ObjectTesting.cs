using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTesting : MonoBehaviour {

    public LoadData Controller;
    public string ID;
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        Controller.GetData(ID);
    }
}
