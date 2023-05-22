using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerAnimator : MonoBehaviour {
    [SerializeField] private Player player;
    private Animator animator;

    private const string IS_DEAD = "isDead";
    private const string IS_WALKING = "isWalking";
    private const string ATTACK = "attack";
    private const string RECEIVE_ATTACK = "receiveAttack";


    // Start is called before the first frame update
    void Start() {
        if (player == null) {
            player = GetComponentInParent<Player>();
        }

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        animator.SetBool(IS_WALKING, player.IsWalking());
        animator.SetBool(IS_DEAD, player.IsDead());
    }


    public void PlayAttackAnimation(Action onComplete = null) {
        animator.SetTrigger(ATTACK);

        if (onComplete != null) {
            StartCoroutine(InvokeOnComplete(onComplete));
        }
    }

    private IEnumerator InvokeOnComplete(Action onComplete) {
        yield return new WaitForEndOfFrame(); // Wait for the end of the frame to ensure the animation state is updated
        while (animator.GetCurrentAnimatorStateInfo(0).IsTag(ATTACK)) {
            yield return null; // Wait until the attack animation has finished playing
        }
        onComplete?.Invoke(); // Invoke the onComplete action
    }
}
