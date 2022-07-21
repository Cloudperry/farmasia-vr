﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glasses : Grabbable {

    private bool insideCollider;

    public HandWashingLiquid sink;
    public GameObject tapCollider;

    protected override void Start() {
        base.Start();
        Type.On(InteractableType.Interactable);
    }

    public override void OnGrab(Hand hand) {
        if (!insideCollider || !sink.IsRunning()) return;
        base.OnGrab(hand);
        Events.FireEvent(EventType.CleaningGlasses, CallbackData.Object(this));
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.transform == tapCollider.transform) insideCollider = true;
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.transform == tapCollider.transform) insideCollider = false;
    }
}
