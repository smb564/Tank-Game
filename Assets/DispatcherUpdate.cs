using UnityEngine;
using System.Collections;
using Assets;

public class DispatcherUpdate : MonoBehaviour {
	// Update is called once per frame
	void Update () {
        Dispatcher dispatcher = Dispatcher.getInstance();
        dispatcher.invokePending();
	
	}
}
