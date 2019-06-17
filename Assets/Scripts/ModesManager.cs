using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OperationMode { TotalLockDown=0, ClockwiseLock=1, AntiClockwiseLock=2 }
public class ModesManager : MonoBehaviour {
    public static OperationMode mode;
}
