using System;
using Moein.ComponentInterface;
using UnityEngine;

namespace Moein.TimeSystem
{
    public class RewindableAnimator : MonoBehaviour
    {
        public static readonly string SPEED_FACTOR_ID = "timescale_speed_factor";

        private Action<AnimatorSnapshot> onCapture;
        [SerializeField] private Animator animator;
        public Animator Animator => animator;

        public float Speed => animator.speed;

        public bool ApplyRootMotion => animator.applyRootMotion;

        public void SetApplyRootMotion(bool value)
        {
            animator.applyRootMotion = value;

            AnimatorSnapshot snapshot =
                new AnimatorSnapshot(0, "", value, AnimatorSnapshot.ActionType.RootMotion);
            Capture(snapshot);
        }

        public void SetTrigger(string name)
        {
            animator.SetTrigger(name);

            AnimatorSnapshot snapshot =
                new AnimatorSnapshot(0, "", name, AnimatorSnapshot.ActionType.Trigger);
            Capture(snapshot);
        }

        public void SetInteger(string name, int value)
        {
            if (animator.GetInteger(name) == value) return;
            animator.SetInteger(name, value);

            AnimatorSnapshot snapshot =
                new AnimatorSnapshot(0, name, value, AnimatorSnapshot.ActionType.Int);
            Capture(snapshot);
        }

        public void SetFloat(string name, float value)
        {
            if (animator.GetFloat(name) == value) return;
            animator.SetFloat(name, value);

            AnimatorSnapshot snapshot =
                new AnimatorSnapshot(0, name, value, AnimatorSnapshot.ActionType.Float);
            Capture(snapshot);
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
            if (animator.speed < 0) return;
            onCapture?.Invoke(snapshot);
        }

        public void ApplySnapshot(AnimatorSnapshot snapshot)
        {
            switch (snapshot.type)
            {
                case AnimatorSnapshot.ActionType.Bool:
                    animator.SetBool(snapshot.name, (bool) snapshot.value);
                    break;
                case AnimatorSnapshot.ActionType.Float:
                    animator.SetFloat(snapshot.name, (float) snapshot.value);
                    break;
                case AnimatorSnapshot.ActionType.Int:
                    animator.SetInteger(snapshot.name, (int) snapshot.value);
                    break;
                case AnimatorSnapshot.ActionType.Trigger:
                    animator.SetTrigger((string) snapshot.value);
                    break;
                case AnimatorSnapshot.ActionType.RootMotion:
                    animator.applyRootMotion = (bool) snapshot.value;
                    break;
            }
        }
    }
}