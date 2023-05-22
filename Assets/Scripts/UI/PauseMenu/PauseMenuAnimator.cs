using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuAnimator : MonoBehaviour {
    [SerializeField] private PauseMenu pauseMenu;
    private Animator animator;

    private const string PAGE_1 = "page1";
    private const string PAGE_2 = "page2";
    private const string PAGE_12 = "page12";
    private const string PAGE_21 = "page21";
     
    // Start is called before the first frame update
    void Start() {
        animator = GetComponent<Animator>();
    } 

      

    // Update is called once per frame
    void Update() {
        
        if (pauseMenu.Page12()) {
            animator.SetTrigger(PAGE_2);
            animator.ResetTrigger(PAGE_1);
        }
        if (pauseMenu.Page21()) {
            animator.SetTrigger(PAGE_1);
            animator.ResetTrigger(PAGE_2);
        }
        
        //animator.SetBool(PAGE_12, pauseMenu.Page12());
        //animator.SetBool(PAGE_21, pauseMenu.Page21());



    }
}
