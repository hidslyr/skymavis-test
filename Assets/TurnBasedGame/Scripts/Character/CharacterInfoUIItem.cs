using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoUIItem : MonoBehaviour
{
    [SerializeField] Text teamText;
    [SerializeField] Text HPText;
    [SerializeField] Text dmgFactorText;

    [SerializeField] string teamTextFormat;
    [SerializeField] string HPTextFormat;
    [SerializeField] string dmgFactorTextFormat;
    [SerializeField] Animator animator;

    public void ShowCharacterInfo(string teamName, string hpStr, int dmgFactor)
    {
        teamText.text = string.Format(teamTextFormat, teamName);
        HPText.text = string.Format(HPTextFormat, hpStr);
        dmgFactorText.text = string.Format(dmgFactorTextFormat, dmgFactor.ToString());

        animator.SetTrigger("show");
    }

    public void OnCharacterInfoChanged(string teamName, string hpStr, int dmgFactor)
    {
        teamText.text = string.Format(teamTextFormat, teamName);
        HPText.text = string.Format(HPTextFormat, hpStr);
        dmgFactorText.text = string.Format(dmgFactorTextFormat, dmgFactor.ToString());
    }

    public void HideCharacterInfo()
    {
        animator.SetTrigger("hide");
    }
}
