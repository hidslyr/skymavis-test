using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineConstant
{
    // State
    public static string LOADING = "loading";
    public static string RUNNING = "running";
    public static string PAUSING = "pausing";
    public static string ENDING = "ending";

    // Transition
    public static string LOADING_TO_RUNNING = "loading_to_running";
    public static string RUNNING_TO_PAUSING = "running_to_pausing";
    public static string PAUSING_TO_RUNNING = "pausing_to_running";
    public static string RUNNING_TO_ENDING = "running_to_ending";
}


public class BasicStateGameplay : MonoBehaviour
{
    protected StateMachine stateMachine;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        InitStateMachine();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        stateMachine.Update();
    }

    protected virtual void InitStateMachine()
    {
        stateMachine = new StateMachine();
        stateMachine.CreateState(StateMachineConstant.LOADING, LoadingUpdate);
        stateMachine.CreateState(StateMachineConstant.RUNNING, PlayingUpdate);
        stateMachine.CreateState(StateMachineConstant.PAUSING, PausingUpdate);
        stateMachine.CreateState(StateMachineConstant.ENDING, EndingUpdate);

        stateMachine.CreateTransition(StateMachineConstant.LOADING,
            StateMachineConstant.RUNNING,
            StateMachineConstant.LOADING_TO_RUNNING,
            OnEnterPlay);

        stateMachine.CreateTransition(StateMachineConstant.RUNNING,
            StateMachineConstant.PAUSING,
            StateMachineConstant.RUNNING_TO_PAUSING,
            OnEnterPause);

        stateMachine.CreateTransition(StateMachineConstant.PAUSING,
            StateMachineConstant.RUNNING,
            StateMachineConstant.PAUSING_TO_RUNNING,
            OnResume);

        stateMachine.CreateTransition(StateMachineConstant.RUNNING,
            StateMachineConstant.ENDING,
            StateMachineConstant.RUNNING_TO_ENDING,
            OnGameEnd);

        stateMachine.SetCurrentState(StateMachineConstant.LOADING);

        OnEnterLoading();
    }

    protected void StartPlayGame()
    {
        stateMachine.ProcessTriggerEvent(StateMachineConstant.LOADING_TO_RUNNING);
    }

    protected void Pause()
    {
        stateMachine.ProcessTriggerEvent(StateMachineConstant.RUNNING_TO_PAUSING);
    }

    protected void Resume()
    {
        stateMachine.ProcessTriggerEvent(StateMachineConstant.PAUSING_TO_RUNNING);
    }

    protected void EndGame()
    {
        stateMachine.ProcessTriggerEvent(StateMachineConstant.RUNNING_TO_ENDING);
    }

    protected virtual void LoadingUpdate()
    {

    }

    protected virtual void PlayingUpdate()
    {

    }

    protected virtual void PausingUpdate()
    {

    }

    protected virtual void EndingUpdate()
    {

    }

    protected virtual void OnEnterLoading()
    {

    }

    protected virtual void OnEnterPlay()
    {

    }

    protected virtual void OnEnterPause()
    {

    }

    protected virtual void OnResume()
    {

    }

    protected virtual void OnGameEnd()
    {

    }
}
