using UnityEditor.Playables;
using UnityEngine;

public class BurnCondition : Condition
{
    public override void Turn(Character character)
    {
        //TODO : ���α�� �ֱ�
        character.TakedamageCharacter(stackCount);
        if (stackCount > 0)
        {
            stackCount--;
            UpdateStackText();
        }
    }
}
