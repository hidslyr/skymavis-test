using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace TurnBaseGame
{
    public static class GameplayGlobalProp
    {
        public static float gameplaySpeed;
    }

    public class Gameplay : BasicStateGameplay
    {
        public static float gameplaySpeed { get; private set; } = 1;

        [Header("References")]
        [SerializeField] Board board;
        [SerializeField] CameraController cameraController;
        [SerializeField] CanvasGroup loadingUI;

        [Header("Gameplay UI")]
        [SerializeField] CanvasGroup gameplayUI;
        [SerializeField] GameObject pauseButton;
        [SerializeField] GameObject resumeButton;
        [SerializeField] GameObject pauseOverlay;
        [SerializeField] GameObject endGameOverlay;
        [SerializeField] Text speedInfoText;
        [SerializeField] Text winningTeamText;
        [SerializeField] Slider powerBarSlider;

        [Space(10)]
        [Header("Configable properties")]
        [SerializeField] float turnInterval;
        [SerializeField] float speedChangePerClick;
        [SerializeField] float minGameplaySpeed;
        [SerializeField] float maxGameplaySpeed;
        [SerializeField] float uiAnimateDuration;

        private CooldownTimer turnTimer = new CooldownTimer();

        protected override void OnEnterLoading()
        {
            loadingUI.alpha = 1;
            loadingUI.gameObject.SetActive(true);
            gameplayUI.alpha = 0;
            gameplayUI.gameObject.SetActive(false);

            board.LoadDefaultBoard();
            turnTimer.Init(turnInterval);
        }

        protected override void PlayingUpdate()
        {
            float desiredDelta = Time.deltaTime * gameplaySpeed;

            turnTimer.Update(desiredDelta);

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

        protected override void OnEnterPause()
        {
            UpdatePauseResumeObjects(true);

            DOTween.PauseAll();
            board.Pause();
        }

        protected override void OnResume()
        {
            UpdatePauseResumeObjects(false);

            DOTween.PlayAll();
            board.Resume();
        }

        protected override void OnGameEnd()
        {
            endGameOverlay.transform.DOScale(Vector3.one, uiAnimateDuration);
            winningTeamText.text = board.GetWinningTeamString();
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
            powerBarSlider.DOValue(board.EstimatedPowerDiff(), uiAnimateDuration);

            if (board.IsGameEnd())
            {
                EndGame();
            }
        }

        public void OnTapToPlay()
        {
            loadingUI.DOFade(0, uiAnimateDuration).OnComplete(() =>
            {
                StartPlayGame();

                loadingUI.gameObject.SetActive(false);
                gameplayUI.gameObject.SetActive(true);
                gameplayUI.DOFade(1, uiAnimateDuration);
            });
        }

        public void OnPauseButton()
        {
            Pause();
        }

        public void OnResumeButton()
        {
            Resume();
        }

        public void OnSpeedUp()
        {
            gameplaySpeed += speedChangePerClick;
            gameplaySpeed = Mathf.Clamp(gameplaySpeed, minGameplaySpeed, maxGameplaySpeed);

            speedInfoText.text = gameplaySpeed.ToString();
        }

        public void OnSpeedDown()
        {
            gameplaySpeed -= speedChangePerClick;
            gameplaySpeed = Mathf.Clamp(gameplaySpeed, minGameplaySpeed, maxGameplaySpeed);

            speedInfoText.text = gameplaySpeed.ToString();
        }

        public void OnRestartButton()
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }

        private void UpdatePauseResumeObjects(bool isPaused)
        {
            pauseButton.SetActive(!isPaused);
            resumeButton.SetActive(isPaused);
            pauseOverlay.SetActive(isPaused);
        }
    }
}
