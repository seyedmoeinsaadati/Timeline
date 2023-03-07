using System;
using Moein.ComponentInterface;
using UnityEngine;

namespace Moein.TimeSystem
{
    public class RewindableAnimator : MonoBehaviour, IAnimator
    {
        public static readonly string SPEED_FACTOR_ID = "timescale_speed_factor";

        private Action<AnimatorSnapshot> onCapture;
        [SerializeField] private Animator animator;
        public Animator Animator => animator;

        public float Speed
        {
            get { return animator.speed; }
            set { animator.SetFloat(SPEED_FACTOR_ID, value); }
        }

        public bool ApplyRootMotion { get; set; }

        public void SetTrigger(int id)
        {
            animator.SetTrigger(id);
        }

        public void SetTrigger(string name)
        {
            animator.SetTrigger(name);

            AnimatorSnapshot snapshot =
                new AnimatorSnapshot(0, "", name, AnimatorSnapshot.ActionType.Trigger);
            Capture(snapshot);
        }

        public void SetInteger(int id, int value)
        {
            animator.SetInteger(id, value);
        }

        public void SetInteger(string name, int value)
        {
            if (animator.GetInteger(name) == value) return;
            animator.SetInteger(name, value);

            AnimatorSnapshot snapshot =
                new AnimatorSnapshot(0, name, value, AnimatorSnapshot.ActionType.Int);
            Capture(snapshot);
        }

        public void SetFloat(int id, float value)
        {
            animator.SetFloat(id, value);
        }

        public void SetFloat(string name, float value)
        {
            if (animator.GetFloat(name) == value) return;
            animator.SetFloat(name, value);

            AnimatorSnapshot snapshot =
                new AnimatorSnapshot(0, name, value, AnimatorSnapshot.ActionType.Float);
            Capture(snapshot);
        }

        public void SetFloat(int id, float value, float dampTime, float deltaTime)
        {
            animator.SetFloat(id, value, dampTime, deltaTime);
        }

        public void SetFloat(string name, float value, float dampTime, float deltaTime)
        {
            animator.SetFloat(name, value, dampTime, deltaTime);
        }

        public void SetBool(int id, bool value)
        {
            animator.SetBool(id, value);
        }

        public void SetBool(string name, bool value)
        {
            if (animator.GetBool(name) == value) return;
            animator.SetBool(name, value);
            AnimatorSnapshot snapshot =
                new AnimatorSnapshot(0, name, value, AnimatorSnapshot.ActionType.Bool);
            Capture(snapshot);
        }

        public void SetSpeed(float timescale)
        {
            animator.SetFloat(SPEED_FACTOR_ID, timescale);
        }

        private void Reset()
        {
            animator = GetComponent<Animator>();
        }

        public void OnChangeAnimator(Action<AnimatorSnapshot> onCapture = null)
        {
            this.onCapture = onCapture;
        }

        private void Capture(AnimatorSnapshot snapshot)
        {
            onCapture?.Invoke(snapshot);
        }
    }
}