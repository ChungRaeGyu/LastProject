using UnityEditor.Playables;
using UnityEngine;

public class BurnCondition : Condition
{
    public override void Turn(Character character)
    {
        //TODO : 세부기능 넣기
        character.TakedamageCharacter(stackCount);
        if (stackCount > 0)
        {
            stackCount--;
            UpdateStackText();
        }
    }
}
