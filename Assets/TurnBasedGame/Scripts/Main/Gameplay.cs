using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Gameplay : BasicStateGameplay
{
    [SerializeField] Board board;
    [SerializeField] CanvasGroup loadingPanel;
    [SerializeField] float loadingFadeDuration;
    [SerializeField] float turnInterval;

    private CooldownTimer turnTimer = new CooldownTimer();

    protected override void OnEnterLoading()
    {
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

    private void ProcessGameLogic()
    {
        
    }

    public void OnTapToPlay()
    {
        loadingPanel.DOFade(0, loadingFadeDuration).OnComplete(() =>
        {
            StartPlayGame();
        });
    }
}
