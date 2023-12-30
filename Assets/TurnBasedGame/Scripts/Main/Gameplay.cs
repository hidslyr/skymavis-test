using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace TurnBaseGame
{
    public class Gameplay : BasicStateGameplay
    {
        [Header("References")]
        [SerializeField] Board board;
        [SerializeField] CameraController cameraController;
        [SerializeField] CanvasGroup loadingUI;

        [Header("Gameplay UI")]
        [SerializeField] CanvasGroup gameplayUI;
        [SerializeField] GameObject pauseButton;
        [SerializeField] GameObject resumeButton;
        [SerializeField] GameObject pauseOverlay;

        [Space(10)]
        [Header("Configable properties")]
        [SerializeField] float loadingFadeDuration;
        [SerializeField] float turnInterval;
        [SerializeField] float speedChangePerClick;

        private CooldownTimer turnTimer = new CooldownTimer();

        protected override void OnEnterLoading()
        {
            loadingUI.alpha = 1;
            board.LoadDefaultBoard();
            turnTimer.Init(turnInterval);
        }

        protected override void PlayingUpdate()
        {
            turnTimer.Update(Time.deltaTime);

            if (turnTimer.IsCooleddown())
            {
                ProcessGameLogic();
                turnTimer.Cooldown();
            }
        }

        protected override void OnEnterPlay()
        {
            cameraController.EnableCameraControl(true);
        }

        private void ProcessGameLogic()
        {
            board.BothTeamsAttack();
            board.AttackersMove();

            OnTurnEnd();
        }

        private void OnTurnEnd()
        {
            board.ClearDeadCharacters();
        }

        public void OnTapToPlay()
        {
            loadingUI.DOFade(0, loadingFadeDuration).OnComplete(() =>
            {
                StartPlayGame();
                loadingUI.gameObject.SetActive(false);
                gameplayUI.DOFade(1, loadingFadeDuration);
            });
        }

        public void OnPauseButton()
        {
            Pause();

            UpdatePauseResumeObjects(true);
        }

        public void OnResumeButton()
        {
            Resume();

            UpdatePauseResumeObjects(false);
        }

        private void UpdatePauseResumeObjects(bool isPaused)
        {
            pauseButton.SetActive(!isPaused);
            resumeButton.SetActive(isPaused);
            pauseOverlay.SetActive(isPaused);
        }

        protected override void Update()
        {
            base.Update();

            if (Input.GetKeyDown(KeyCode.M))
            {
                board.AttackersMove();
            }
        }
    }
}
