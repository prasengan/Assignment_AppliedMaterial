using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ScrewType { Hex12, Hex6, RobertStone, Philips }
public class Screw : MonoBehaviour {
    public ScrewType screwType;
    public Transform screwHead;
    public float pitch = 0.08f;
    public float maxHeight=0.21f, minHeight=-0.076f;

    /// <summary>
    /// Apply up down movement based on angular motion of locked wrench and provided pitch with max and min height
    /// </summary>
    /// <param name="angle"></param>
    public void ApplyRotationBasedMovement(float angle)
    {
        Vector3 updatedPos = transform.position;
        updatedPos .y -= pitch*angle/ 360;
        //Check if screw movement is within Constraints
        if (updatedPos.y < maxHeight && updatedPos.y > minHeight)
        {
            transform.position = updatedPos;
            transform.rotation *= Quaternion.Euler(angle, 0, 0);
        }
    }
}
