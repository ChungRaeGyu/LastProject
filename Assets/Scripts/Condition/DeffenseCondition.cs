public class DeffenseCondition : Condition
{
    public override void Turn(Character character)
    {
        if (stackCount > 0)
        {
            stackCount--;
            UpdateStackText();
        }
    }
}
