using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour {

    public GameObject cut;
    public float cutSize = 15f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (TouchInput.GetSwipe() != null)
        {
            Swipe swipe = TouchInput.GetSwipe().Normalized(cutSize);
            //CutAnimation c = Instantiate(cut).GetComponent<CutAnimation>();
            //c.Initialize(swipe.start, swipe.end);
        }
    }
}
