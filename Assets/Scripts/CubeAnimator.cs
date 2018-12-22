using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeAnimator : MonoBehaviour {
    public float animator;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.GetComponent<Transform>().position = new Vector3 (0, Mathf.Sin(animator), 0);
	}
}
