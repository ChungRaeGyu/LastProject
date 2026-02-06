public class WeakerCondition : Condition
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
            character.BaseWeakerMethod();
        }
    }
    public override void Utility(Character character)
    {
        character.WeakingMethod(0.25f);
    }
}
