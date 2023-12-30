using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using DG.Tweening;
using System;
using Random = UnityEngine.Random;
using MarchingBytes;

namespace TurnBaseGame
{
    public class Character : MonoBehaviour
    {
        [SerializeField] CharacterRenderer characterRenderer;
        [SerializeField] CharacterHPBar hpBar;
        [SerializeField] protected int HP;
        [SerializeField] protected BoardNodeType boardNodeType;
        [SerializeField] public bool debug;

        protected Action<Character> onDead;
        protected int currentHP;
        [SerializeField] protected Location location;
        public Location Location
        {
            get
            {
                return location;
            }
        }

        protected int damageFactor;

        private float moveAnimateDuration = 0.3f;

        public void Init(Location location)
        {
            currentHP = HP;
            this.location = location;
            damageFactor = Random.Range(0, 3);
            onDead = null;

            UpdateHealthBar();
        }

        public void SetOnDeadListener(Action<Character> listener)
        {
            onDead += listener;
        }

        public void Move(Vector3 position, Location targetLocation)
        {
            if (targetLocation.x > location.x)
            {
                characterRenderer.TurnRight();
            }
            else
            {
                characterRenderer.TurnLeft();
            }
            
            characterRenderer.Jump();
            location = targetLocation;

            transform.DOMove(position, moveAnimateDuration * Gameplay.gameplaySpeed);
        }

        public void Attack(Character target)
        {
            if (target.Location.x > location.x)
            {
                characterRenderer.TurnRight();
            }
            else
            {
                characterRenderer.TurnLeft();
            }

            characterRenderer.Attack();

            int damageDealt = CalculateDamageDealt(target.damageFactor);
            target.Attacked(damageDealt);
        }

        public void Attacked(int damageReceive)
        {
            LoweringHP(damageReceive);
        }

        public int GetCurrentHP()
        {
            return currentHP;
        }

        public BoardNodeType GetBoardNodeType()
        {
            return boardNodeType;
        }

        public int Distance(Character target)
        {
            return Location.Distance(Location, target.Location);
        }

        public virtual bool IsOnDifferentTeam(Character other)
        {
            throw new NotImplementedException();
        }

        public void PauseAnimation()
        {
            characterRenderer.PauseAnimation();
        }

        public void ResumeAnimation()
        {
            characterRenderer.ResumeAnimation();
        }

        public void HighLight()
        {
            characterRenderer.HighLight();
        }

        public void UnHighLight()
        {
            characterRenderer.UnHighLight();
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
}
