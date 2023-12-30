using AxieMixer.Unity;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace TurnBaseGame
{
    public class CustomizedAxieFigure : MonoBehaviour
    {
        public string id;
        public string genes;
        private SkeletonAnimation skeletonAnimation;

        [SerializeField] private bool _flipX = false;
        public bool flipX
        {
            get
            {
                return _flipX;
            }
            set
            {
                _flipX = value;
                if (skeletonAnimation != null)
                {
                    skeletonAnimation.skeleton.ScaleX = (_flipX ? -1 : 1) * Mathf.Abs(skeletonAnimation.skeleton.ScaleX);
                }
            }
        }

        private void Awake()
        {
            skeletonAnimation = gameObject.GetComponent<SkeletonAnimation>();
            
            // Shouldn't be here, but it's useful
            Mixer.Init();
            Mixer.SpawnSkeletonAnimation(skeletonAnimation, id, genes);

            skeletonAnimation.transform.localPosition = new Vector3(0f, -0.32f, 0f);
            skeletonAnimation.transform.SetParent(transform, false);
            skeletonAnimation.transform.localScale = new Vector3(1, 1, 1);
            skeletonAnimation.skeleton.ScaleX = (_flipX ? -1 : 1) * Mathf.Abs(skeletonAnimation.skeleton.ScaleX);
            skeletonAnimation.timeScale = 0.5f;
            skeletonAnimation.skeleton.FindSlot("shadow").Attachment = null;
            skeletonAnimation.state.SetAnimation(0, "action/idle/normal", true);
            skeletonAnimation.state.End += SpineEndHandler;
        }

        private void OnDisable()
        {
            if (skeletonAnimation != null)
            {
                skeletonAnimation.state.End -= SpineEndHandler;
            }
        }

        public void TurnLeft()
        {
            skeletonAnimation.skeleton.ScaleX = 1;
        }

        public void TurnRight()
        {
            skeletonAnimation.skeleton.ScaleX = -1;
        }

        public void PauseAnimation()
        {
            skeletonAnimation.timeScale = 0;
        }

        public void ResumeAnimation()
        {
            skeletonAnimation.timeScale = Gameplay.gameplaySpeed;

            TrackEntry currentTrack = skeletonAnimation.AnimationState.Tracks.Items[0];

            if (currentTrack.Animation.Name == "action/idle/normal")
            {
                skeletonAnimation.timeScale = 0.5f * Gameplay.gameplaySpeed;
            }
        }

        public void DoJumpAnim()
        {
            skeletonAnimation.timeScale = Gameplay.gameplaySpeed;
            skeletonAnimation.AnimationState.SetAnimation(0, "action/move-forward", false);
        }

        public void DoAttackMeleeAnim()
        {
            skeletonAnimation.timeScale = Gameplay.gameplaySpeed;
            skeletonAnimation.AnimationState.SetAnimation(0, "attack/melee/normal-attack", false);
        }

        public void DoWinAnim()
        {
            skeletonAnimation.timeScale = Gameplay.gameplaySpeed;
            skeletonAnimation.AnimationState.SetAnimation(0, "activity/victory-pose-back-flip", false);
        }


        private void SpineEndHandler(TrackEntry trackEntry)
        {
            string animation = trackEntry.Animation.Name;
            if (animation == "action/move-forward")
            {
                skeletonAnimation.state.SetAnimation(0, "action/idle/normal", true);
                skeletonAnimation.timeScale = 0.5f * Gameplay.gameplaySpeed;
            }
        }
    }
}
