using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sense {
    public enum SenseType {
        Darkvision,
        Blindsight,
        Tremorsense,
        Truesight
    }

    public string senseTitle;

    public SenseType senseType;
    public int senseRange;

    public Sense(SenseType senseType, int senseRange) {
        this.senseType = senseType;
        this.senseRange = senseRange;
    }

}