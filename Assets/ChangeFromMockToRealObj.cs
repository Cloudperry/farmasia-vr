using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ChangeFromMockToRealObj : MonoBehaviour
{
    // Start is called before the first frame update

    public List<GameObject> gameobjTODisable;
    public List<GameObject> gameobjTOEnable;

    public XRGrabInteractable interactableTOGrabAfterEnalbe;

    XRInteractionManager manager;
    void Start()
    {
        manager = GetComponent<XRBaseInteractable>().interactionManager;
    }

   
    public void changeToRealObj(SelectEnterEventArgs args)
    {
        
        foreach (GameObject obj in gameobjTODisable)
        {
            obj.SetActive(false);
        }
        foreach (GameObject obj in gameobjTOEnable)
        {
            obj.SetActive(true);
        }
        
        

        manager.SelectEnter(args.interactorObject.transform.GetComponent<IXRSelectInteractor>(), interactableTOGrabAfterEnalbe);
        this.GetComponent<Collider>().enabled = false;
    }
}
