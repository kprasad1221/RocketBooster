using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour {

    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [Range(0,1)] [SerializeField] float movementFactor; // 0 for not moved, 1 for fully moved

    [SerializeField] float period = 2f;

    Vector3 startingPos;

	void Start ()
    {
        startingPos = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(period <= Mathf.Epsilon) { return; }
        float cycle = Time.time / period;
        const float tau = Mathf.PI * 2f;  // about 6.28
        float rawSineWave = Mathf.Sin(tau * cycle);  // goes from -1 to 1

        movementFactor = rawSineWave / 2f + 0.5f;

        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
	}
}
