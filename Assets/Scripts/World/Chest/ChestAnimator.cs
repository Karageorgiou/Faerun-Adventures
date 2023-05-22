using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestAnimator : MonoBehaviour {

    [SerializeField] private Chest chest;
    private Animator animator;

    private const string IS_OPEN = "isOpen";

    // Start is called before the first frame update
    void Start() {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        animator.SetBool(IS_OPEN, chest.IsOpen());
    }

}
