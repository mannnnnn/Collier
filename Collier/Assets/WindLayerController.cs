using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindLayerController : MonoBehaviour {

void Start ()
     {
		 	// This is infurryating
             particleSystem.renderer.sortingLayerName = "Foreground";
     }
}
