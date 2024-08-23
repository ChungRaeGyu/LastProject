public class BleedingCondition : Condition
{
    public override void Turn(Character character)
    {
        character.TakedamageCharacter(stackCount);
        if (stackCount > 0)
        {
            stackCount--;
            UpdateStackText();
        }
    }
}
