using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

    private bool page12;
    private bool page21;

    
     
    public bool Page12() { return page12; }
    public void SetPage12(bool page12) { this.page12 = page12;}

    public bool Page21() { return page21; }

    public void SetPage21(bool page21) {  this.page21 = page21; }
}
