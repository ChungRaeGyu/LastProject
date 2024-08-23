public class DefDownCondition : Condition
{
    public override void Turn(Character character)
    {
        if (stackCount > 0)
        {
            stackCount--;
            UpdateStackText();
        }
        else
        {
            character.BasedefMethod();
        }
    }

    public override void Utility(Character character)
    {
        character.DefDownValue(0.5f);
    }
}
