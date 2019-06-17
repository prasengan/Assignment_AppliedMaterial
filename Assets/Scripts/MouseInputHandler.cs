using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MouseInputHandler : MonoBehaviour {
    public Camera mainCamera;
    public static Action OnMouseLeftClick, OnMouseRightClick;
    public static Action<Vector3> UpdateMousePosition;
    public static MouseInputHandler instance;
    public Vector3 mousePos = new Vector3();
    public float pointerHeight = 200;
    public Vector2 delta;
    public float scrollSensitivity=10;
    public bool b_LockPointerHeight;

    private void Awake()
    {
        instance = this;
    }
	// Update is called once per frame
	void Update () {
        CheckForMouseInput();
        UpdateMouseWorldPosition();
	}

    /// <summary>
    /// For Updating position of pointer in 3d space
    /// </summary>
    void UpdateMouseWorldPosition()
    {
        if (!b_LockPointerHeight)
        {// Don't allow scroll based height change if pointer height is contrained 
            delta = Input.mouseScrollDelta;
            pointerHeight += delta.y * scrollSensitivity;
        }

        Vector3 pos = Input.mousePosition;
        Ray camRay = mainCamera.ScreenPointToRay(pos);
        Plane movementPlane = new Plane(Vector3.up * pointerHeight, mainCamera.transform.position-Vector3.up * pointerHeight);
        float enter=0;

        // Raycast to obtain an intersecting point on plane in x distance away
        movementPlane.Raycast(camRay, out enter);
        mousePos = camRay.GetPoint(enter);
        if (UpdateMousePosition != null) { UpdateMousePosition(mousePos); }
    }

    /// <summary>
    /// Cast event On Mouse Button Press
    /// </summary>
    void CheckForMouseInput()
    {
        if (Input.GetMouseButtonDown(0)) {
            if (OnMouseLeftClick != null)
                OnMouseLeftClick();
        }
        if (Input.GetMouseButtonDown(1)) {
            if (OnMouseRightClick != null)
                OnMouseRightClick();
        }
    }
}
