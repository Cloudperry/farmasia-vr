using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;
using UnityEngine.XR;
// This will add a new particle system to FireGridObject, not necessary now
//[RequireComponent(typeof(ParticleSystem))]

public class FireGrid : MonoBehaviour
{
    public KeyCode toggleKey = KeyCode.Space;
    //public InputActionReference igniteEvent;
    // Different particle effect fields
    [SerializeField]
    private ParticleSystem fireParticle;
    [SerializeField]
    private ParticleSystem igniteParticle;
    [SerializeField]
    private ParticleSystem extinguishParticle;
    // Light source of the fire
    [SerializeField]
    private GameObject pointLight;

    [SerializeField]
    private GameObject colliderCube;

    private bool isIgnited;
    private bool isExtinguished;
    [SerializeField]
    private int degrees;

    // Start is called before the first frame update
    void Start()
    {
        //fireParticle = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        // Hit spacebar to lower degrees by ten
        if (Input.GetKeyDown(toggleKey))
        {
            if (degrees >= 10)
            {
                degrees -= 10;
                Debug.Log("degrees should go down by -10: " + degrees);
            }
        }
        // If at 0 degrees stop the fire particle effect and play the extinguish particle once
        if (degrees <= 0)
        {
            Extinguish();
        }
        else if (degrees > 0)
        {
            Ignite();
        }

    }

    public void Extinguish()
    {
        fireParticle.Stop();
        pointLight.SetActive(false);
        if (extinguishParticle != null && isExtinguished == false)
        {
            extinguishParticle.Play();
            isExtinguished = true;
        }
        Debug.Log("degrees should be zero: " + degrees);
    }

    public void Ignite()
    {
        fireParticle.Play();
        pointLight.SetActive(true);
        if (igniteParticle != null && isIgnited == false)
        {
            igniteParticle.Play();
            isIgnited = true;
        }
    }
}
