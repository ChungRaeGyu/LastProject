public class ForzenCondition : Condition
{
    public override void Turn(Character character)
    {
        if (stackCount > 0)
        {
            stackCount--;
            UpdateStackText();
            if (stackCount == 0)
            {
                character.AnimationStop();
            }
        }
        else
        {
            character.isFrozen = false;
        }
    }

    public override void Utility(Character character)
    {
        character.isFrozen = true;

    }
}
