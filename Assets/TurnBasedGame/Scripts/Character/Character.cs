using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using DG.Tweening;
using System;
using Random = UnityEngine.Random;
using MarchingBytes;

public class Character : MonoBehaviour
{
    [SerializeField] CharacterRenderer characterRenderer;
    [SerializeField] CharacterHPBar hpBar;
    [SerializeField] protected int HP;
    [SerializeField] protected bool canMove;
    [SerializeField] protected BoardNodeType boardNodeType;

    protected Action<Character> onDead;
    protected int currentHP;
    protected Location currentIndexes;
    protected int damageFactor;

    private float moveAnimateDuration = 0.3f;

    public void Init(Location startingIndexes)
    {
        currentHP = HP;
        currentIndexes = startingIndexes;
        damageFactor = Random.Range(0, 3);
        onDead = null;

        UpdateHealthBar();
    }

    public Location GetCurrentLocation()
    {
        return currentIndexes;
    }

    public void SetOnDeadListener(Action<Character> listener)
    {
        onDead += listener;
    }

    public bool CanMove()
    {
        return canMove;
    }

    public void Move(Vector3 position, Location indexes)
    {
        characterRenderer.Jump();
        currentIndexes = indexes;

        this.transform.DOMove(position, moveAnimateDuration);
    }

    public void Attack(Character target)
    {
        int damageDealt = CalculateDamageDealt(target.damageFactor);
        target.Attacked(damageDealt);
    }

    public void Attacked(int damageReceive)
    {
        LoweringHP(damageReceive);
    }

    public BoardNodeType GetBoardNodeType()
    {
        return boardNodeType;
    }

    protected int CalculateDamageDealt(int targetDamageFactor)
    {
        int magicNumber = 3 + damageFactor - targetDamageFactor;

        if (magicNumber % 3 == 0)
        {
            return 4;
        }

        if (magicNumber % 3 == 1)
        {
            return 5;
        }

        return 3;
    }

    protected void LoweringHP(int amount)
    {
        currentHP -= amount;
        currentHP = Mathf.Clamp(currentHP, 0, currentHP);

        UpdateHealthBar();

        if (currentHP == 0)
        {
            Death();
        }
    }

    protected void Death()
    {
        onDead?.Invoke(this);

        DOVirtual.DelayedCall(hpBar.GetAnimateDuration(), () =>
        {
            EasyObjectPool.instance.ReturnObjectToPool(gameObject);
        });
    }

    protected void UpdateHealthBar()
    {
        float percentageToMax = (float)currentHP / HP;
        hpBar.SetHealthBarTo(percentageToMax);
    }
}
