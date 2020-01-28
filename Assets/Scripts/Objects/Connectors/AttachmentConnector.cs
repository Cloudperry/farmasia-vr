﻿using System.Collections.Generic;
using UnityEngine;

public abstract class AttachmentConnector : ItemConnector {

    public struct AttachedObject {
        public Interactable Interactable;
        public Vector3 Scale;
        public GameObject GameObject {
            get {
                return Interactable?.gameObject;
            }
        }
        public Rigidbody Rigidbody {
            get {
                return Interactable?.Rigidbody;
            }
        }
    }

    public AttachmentConnector(Transform obj) : base(obj) {
    }

    protected static float angleLimit = 20;
    protected static float maxAttachDistance = 0.03f;

    #region Fields
    public GeneralItem GeneralItem { get; protected set; }
    public GameObject Collider { get; protected set; }
    // public Joint Joint { get; protected set; }
    public bool HasAttachedObject { get => attached.GameObject != null; }
    public Rigidbody AttachedRigidbody { get => attached.Rigidbody; }
    public Interactable AttachedInteractable { get => attached.Interactable; }

    protected AttachedObject attached;

    protected abstract InteractState AttachState { get; }
    #endregion

    public void Subscribe() {
        CollisionSubscription.SubscribeToTrigger(Collider, new TriggerListener().OnEnter(ObjectEnter));
    }


    #region Attaching
    protected void ReplaceObject(GameObject newObject) {
        if (attached.GameObject == newObject) {
            return;
        }

        if (attached.GameObject != null) {
            CollisionIgnore.IgnoreCollisions(GeneralItem.transform, attached.GameObject.transform, false);
        }

        if (newObject == null) {
            attached.Interactable = null;
            return;
        }

        attached.Scale = newObject.transform.localScale;
        attached.Interactable = newObject.GetComponent<Interactable>();
        SetInteractors();

        // Either override property InteractState AttachState and use State.On(AttachState) or rename LuerlockAttached to Attached
        attached.Interactable.State.On(AttachState);

        CollisionIgnore.IgnoreCollisions(GeneralItem.transform, attached.GameObject.transform, true);

        // Attaching
        SnapObjectPosition();

        // Joint = JointConfiguration.AddJoint(Luerlock.gameObject);
        // Joint.connectedBody = attached.Rigidbody;
        if (attached.GameObject == null) {
            Logger.Error("Attached gameobject null");
        }
        Connection = ItemConnection.AddChildConnection(this, GeneralItem.transform, attached.Interactable);
    }

    #region Type overrides
    protected abstract void SetInteractors();
    protected abstract void SnapObjectPosition();
    protected abstract void AttachEvents(GameObject intObject);
    #endregion


    #endregion

    protected void ObjectEnter(Collider collider) {

        Interactable interactable = Interactable.GetInteractable(collider.transform);
        if (interactable == null) {
            return;
        }

        if (interactable.Type != InteractableType.Attachable) {
            return;
        }

        if (attached.GameObject == null && ConnectingIsAllowed(Collider, collider)) {
            // Position Offset here

            ConnectItem(interactable.GetComponent<Interactable>());
            AttachEvents(interactable.gameObject);
        }
    }

    protected bool ConnectingIsAllowed(GameObject adapterCollider, Collider connectingCollider) {
        Interactable connectingInteractable = Interactable.GetInteractable(connectingCollider.transform);
        if (connectingInteractable == null) {
            return false;
        }

        float collisionAngle = Vector3.Angle(adapterCollider.transform.up, connectingInteractable.transform.up);
        if (collisionAngle > angleLimit) {
            return false;
        }

        if (!IsWithinDistance(adapterCollider, connectingInteractable.transform)) {
            return false;
        }

        if (connectingInteractable.Type.IsOff(InteractableType.Attachable)) {
            return false;
        }

        return true;
    }

    protected bool IsWithinDistance(GameObject collObject, Transform t) {
        if (t == null) {
            Logger.Print("Transfor is null");
        }
        if (collObject == null) {
            Logger.Print("Coll object is null");
        }
        if (Collider == null) {
            Logger.Print("Base collider is null");
        }
        if (LuerlockAdapter.LuerlockPosition(t) == null) {
            Logger.Print("Luerlock position is null");
        }
        return Vector3.Distance(collObject.transform.position, LuerlockAdapter.LuerlockPosition(t).position) < maxAttachDistance;
    }
}
