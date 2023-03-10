﻿using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using FarmasiaVR.Legacy;

public class CabinetBase : MonoBehaviour {

    private TriggerInteractableContainer itemContainer;
    private bool itemPlaced;
    private bool sterileDrapefolded;
    private bool syringeCapBagEntered;

    public enum CabinetType { PassThrough, Laminar }
    public CabinetType type;
    public Animator sterileDrape;
    public GameObject childCollider;
    public GameObject syringeCapFactory;
    public GameObject syringeCapFactoryPos;

    private void Start() {
        itemContainer = childCollider.AddComponent<TriggerInteractableContainer>();
        itemContainer.OnEnter = OnCabinetEnter;
        itemContainer.OnExit = OnCabinetExit;
        if (syringeCapFactory != null) {
            syringeCapFactory.SetActive(false);
        }
    }

    private void OnCabinetEnter(Interactable other) {
        GeneralItem item = other as GeneralItem;
       
        if (item == null) {
            return;
        }
     
        if (type == CabinetType.Laminar) {
            Events.FireEvent(EventType.CheckLaminarCabinetItems);

            if (item.Contamination == GeneralItem.ContaminateState.FloorContaminated) {
                Task.CreateGeneralMistake("Lattialla olevia esineitä ei saa tuoda laminaarikaappiin.", 1);
            }

            if (item is SyringeCapBag) {
                SyringeCapBagEnteredLaminarCabinet(item);
            }

            if (Time.timeSinceLevelLoad > 1.0f) {
                UnfoldSterileDrape();
            }
        }
       
        if (item.Contamination == GeneralItem.ContaminateState.Contaminated) {
            Task.CreateGeneralMistake("Kaappiin tuotu esine ei ollut puhdas.", 1);
        }
      
        if (!itemPlaced) {
            Events.FireEvent(EventType.ItemPlacedForReference, CallbackData.Object(this));
            itemPlaced = true;
           
        }
      
    }

    // Currently unused
    private void OnCabinetExit(Interactable other) {
        GeneralItem item = other as GeneralItem;

        if (item == null) {
            return;
        }
    }

    private void SyringeCapBagEnteredLaminarCabinet(GeneralItem syringeCapBag) {
        if (syringeCapBagEntered == true) {
            return;
        }
        syringeCapBagEntered = true;
        StartCoroutine(MoveSyringeCapBagAndEnableFactory(syringeCapBag));
    }

    private IEnumerator MoveSyringeCapBagAndEnableFactory(GeneralItem syringeCapBag) {
        yield return new WaitForSeconds(2.0f);

        syringeCapBag.GetComponent<SyringeCapBag>().DisableSyringeCapBag();

        Vector3 startPos = syringeCapBag.transform.position;
        Vector3 targetPos = syringeCapFactoryPos.transform.position;
        Quaternion startRot = syringeCapBag.transform.rotation;
        Quaternion targetRot = syringeCapFactoryPos.transform.rotation;

        float time = 1.0f;
        float currentTime = 0.0f;

        while (currentTime < time) {
            currentTime += Time.deltaTime;
            float progress = currentTime / time;
            syringeCapBag.transform.SetPositionAndRotation(Vector3.Slerp(startPos, targetPos, progress), Quaternion.Slerp(startRot, targetRot, progress));
            yield return null;
        }

        syringeCapBag.transform.SetPositionAndRotation(targetPos, targetRot);
        syringeCapFactory.SetActive(true);
    }

    private void UnfoldSterileDrape() {
        if (sterileDrapefolded) {
            return;
        }
        sterileDrapefolded = true;
        sterileDrape.SetBool("ItemPlaced", true);
    }

    /// <summary>
    /// Returns list of all items inside the cabinet.
    /// </summary>
    public List<Interactable> GetContainedItems() {
        return itemContainer.Objects.ToList();
    }
}
