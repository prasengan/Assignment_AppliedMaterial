using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrenchHead : MonoBehaviour {
    public Wrench wrench;
    public ScrewType screwType;
    public Screw screwInRange;

    private void OnEnable()
    {
        MouseInputHandler.OnMouseRightClick += HandleRightClick;
    }
    private void OnDisable()
    {
        MouseInputHandler.OnMouseRightClick -= HandleRightClick;
    }

    private void OnTriggerEnter(Collider other)
    {
        // when a srew is in range cache it
        if (other.CompareTag("Screw")){
            Screw screw = other.GetComponent<Screw>();
            if (screwType == screw.screwType)
            {
                screwInRange = screw;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        // clear cache when screw exits
        if (other.CompareTag("Screw"))
        {
            Screw screw = other.GetComponent<Screw>();
            if (screwType == screw.screwType)
            {
                if(screw==screwInRange)
                    screwInRange = null;
            }
        }
    }

    void HandleRightClick()
    {
        if (screwInRange)
        {
            // On right click if screw is in range lock it if it is not already locked else release
            if (wrench.lockedScrew == screwInRange)
            {
                wrench.lockedScrew = null;
                wrench.lockedWrenchHead = null;
                MouseInputHandler.instance.b_LockPointerHeight = false;
            }
            else {
                wrench.lockedScrew = screwInRange;
                wrench.lockedWrenchHead = this;
                if(wrench.OnScrewLock!=null)
                    wrench.OnScrewLock();
            }   
        }
    }
}
