using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandContoller : MonoBehaviour
{
    public float handHeight = -37;

    public Vector3 mousePos;
    public Transform holder;
    public Wrench toolInRange, attachedTool;
    public Animator animator;
    private bool b_IstoolInRange = false;
    private float wrenchHeadToHandLength;
    const float basePlaneheight = 2.7f;
    #region Unity Methods
    private void OnEnable()
    {
        MouseInputHandler.OnMouseLeftClick += HandleMouseInput;
        MouseInputHandler.UpdateMousePosition += UpdateMouseWorldPosition;
    }
    // Update is called once per frame
    void Update()
    {
        UpddatePosition();
    }
    private void OnTriggerEnter(Collider other)
    {//hand layer can only interact with tool layer
        b_IstoolInRange = true;
        toolInRange = other.GetComponent<Wrench>();
    }
    private void OnTriggerExit(Collider other)
    {//hand layer can only interact with tool layer
        b_IstoolInRange = false;
        toolInRange = null;
    }
    private void OnTriggerStay(Collider other)
    {//hand layer can only interact with tool layer
        if (!b_IstoolInRange)
        {
            toolInRange = other.GetComponent<Wrench>();
            b_IstoolInRange = true;
        }
    }
    #endregion

    #region HandMovement
    void UpddatePosition()
    {
        if (attachedTool)
        {
            if (attachedTool.lockedScrew)
            {
                ApplyConstrainedMotion(); //Constrain movement when locked with screw
            }
            else { FollowMousePosition(); }
        }
        else { FollowMousePosition(); } // Free movement based on mouse when not locked to screw
    }

    /// <summary>
    /// Apply lock based movement of hand and apply screw rotation
    /// </summary>
    void ApplyConstrainedMotion()
    {
        mousePos.y = attachedTool.lockedScrew.screwHead.transform.position.y;
        Vector3 screwHeadToMouse = attachedTool.lockedScrew.screwHead.transform.position- mousePos;
        screwHeadToMouse = Vector3.Normalize(screwHeadToMouse);
        Vector3 screwHeadToHand = attachedTool.lockedScrew.screwHead.transform.position - transform.position;
        float angle = Vector3.SignedAngle(screwHeadToHand, screwHeadToMouse, Vector3.up);
        switch (ModesManager.mode)
        {
            case OperationMode.TotalLockDown:
                attachedTool.lockedScrew.ApplyRotationBasedMovement(angle);
                break;
            case OperationMode.ClockwiseLock:
                if (angle < 0) { attachedTool.lockedScrew.ApplyRotationBasedMovement(angle); }
                break;
            case OperationMode.AntiClockwiseLock:
                if (angle > 0) { attachedTool.lockedScrew.ApplyRotationBasedMovement(angle); }
                break;
            default:
                break;
        }
        transform.rotation = Quaternion.LookRotation(screwHeadToMouse);
        transform.position = attachedTool.lockedScrew.screwHead.position - screwHeadToMouse * wrenchHeadToHandLength;

    }
    /// <summary>
    /// Follow mouse 3D position
    /// </summary>
    void FollowMousePosition()
    {
        transform.position = mousePos;
    }
    #endregion

    #region EventReceivers
    void HandleMouseInput()
    {
        if (attachedTool) { DetachTool(); }
        else { AttachTool(); }
    }
    void UpdateMouseWorldPosition(Vector3 pos)
    {
        mousePos = pos;
    }
    /// <summary>
    /// Apply all necessary transform changes for locking tool to screw
    /// </summary>
    void LockToScrew()
    {
        MouseInputHandler.instance.pointerHeight = MouseInputHandler.instance.mainCamera.transform.position.y - attachedTool.lockedScrew.screwHead.position.y;
        MouseInputHandler.instance.b_LockPointerHeight = true;
        Vector3 screwHeadToHand = attachedTool.lockedScrew.screwHead.position - transform.position;
        wrenchHeadToHandLength = (attachedTool.lockedWrenchHead.transform.position
                                                - attachedTool.attachmentPoint.position).magnitude;
        transform.rotation = Quaternion.LookRotation(screwHeadToHand);
        transform.position = attachedTool.lockedScrew.screwHead.position
                                                - screwHeadToHand.normalized * wrenchHeadToHandLength;
        transform.position = new Vector3(transform.position.x,
                                            attachedTool.lockedScrew.screwHead.transform.position.y,
                                            transform.position.z);
    }
    #endregion

    #region ToolAttachment
    /// <summary>
    /// Attach wrench when in range
    /// </summary>
    void AttachTool()
    {
        if (toolInRange)
        {
            attachedTool = toolInRange;
            attachedTool.transform.parent = holder;
            attachedTool.transform.localRotation = Quaternion.identity;
            attachedTool.transform.localPosition = attachedTool.transform.position 
                                                        - attachedTool.attachmentPoint.position;
            attachedTool.m_Rigidbody.isKinematic = true;
            attachedTool.OnScrewLock += LockToScrew;
            animator.SetBool("Hold", true);
        }
    }
    /// <summary>
    /// Detach Attached tool
    /// </summary>
    void DetachTool()
    {
        toolInRange = attachedTool;
        attachedTool.OnScrewLock -= LockToScrew;
        attachedTool.m_Rigidbody.isKinematic = false;
        attachedTool.transform.parent = null;
        attachedTool.lockedWrenchHead = null;
        attachedTool.lockedScrew = null;
        attachedTool = null;
        transform.rotation = Quaternion.identity;
        MouseInputHandler.instance.b_LockPointerHeight = false;
        MouseInputHandler.instance.pointerHeight = basePlaneheight;
        animator.SetBool("Hold", false);
    }
    #endregion

}
