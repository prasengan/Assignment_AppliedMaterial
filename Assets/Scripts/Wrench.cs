using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wrench : MonoBehaviour {
    public Transform attachmentPoint;
    public Rigidbody m_Rigidbody;
    public Screw lockedScrew;
    public System.Action OnScrewLock;
    public WrenchHead lockedWrenchHead;
    public Vector3 headToAttachment;
}
