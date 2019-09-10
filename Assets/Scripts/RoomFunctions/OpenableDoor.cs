﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenableDoor : MonoBehaviour {

    #region fields

    [SerializeField]
    private float maxAngle = 90;
    private float startAngle;

    public float Velocity { get; set; }
    private float minVelocity = 0.1f;

    [SerializeField]
    private float friction = 0.9f;

    public bool Locked { get; set; } = true;

    private Vector3 lastEulerAngles;
    #endregion

    private void Start() {
        startAngle = transform.eulerAngles.y;
    }

    public void SetByHandPosition(Vector3 handPos) {
        lastEulerAngles = transform.eulerAngles;

        Vector3 initialRot = transform.eulerAngles;

        Vector3 rot = -(handPos - transform.position);
        transform.right = rot;
        rot = transform.eulerAngles;
        rot.x = initialRot.x;
        rot.z = initialRot.z;
        transform.eulerAngles = rot;
    }

    public void ReleaseDoor() {

        Velocity = (transform.eulerAngles.y - lastEulerAngles.y) / Time.deltaTime;

        Logger.PrintVariables("Velocity", Velocity);

        Locked = false;
    }

    private void Update() {
        UpdateVelocity();
        RotateDoor();
    }

    private void UpdateVelocity() {

        Velocity *= friction;

        if (Mathf.Abs(Velocity) < minVelocity) {
            Velocity = Velocity > 0 ? minVelocity : -minVelocity;
        }
    }

    private void RotateDoor() {

        if (Locked) {
            return;
        }

        Vector3 rotateVector = Vector3.up * Velocity * Time.deltaTime;

        if (rotateVector.magnitude > 0) {

            transform.Rotate(rotateVector);

            AngleLock.ClampAngleDeg(Angle, startAngle + maxAngle, startAngle);
            if (Angle > maxAngle) {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, startAngle + maxAngle, transform.eulerAngles.z);
            }

        } else {

            transform.Rotate(rotateVector);

            if (Angle < startAngle) {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, startAngle, transform.eulerAngles.z);
            }
        }
    }

    private float Angle {
        get {
            return Mathf.Abs(transform.eulerAngles.y - startAngle);
        }
    }
}
