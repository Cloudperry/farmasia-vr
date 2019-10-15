﻿using UnityEngine;

public class GeneralItem : Grabbable {

    #region fields
    [SerializeField]
    protected ObjectType objectType = ObjectType.None;
    public ObjectType ObjectType { get => objectType; set { objectType = value; } }

    [SerializeField]
    private bool isClean;
    public bool IsClean { get => isClean; set => isClean = value; }
    #endregion

    protected virtual void OnCollisionEnter(Collision coll) {
        if (coll.gameObject.tag == "Floor") IsClean = false;
    }
}
