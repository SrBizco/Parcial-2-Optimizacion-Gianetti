using UnityEngine;

public class SelfHealStrategy : IActionStrategy
{
    public void Execute(Character executor, Character target)
    {
        executor.Heal(executor.HealAmount);
        Debug.Log($"{executor.name} se curó a sí mismo por {executor.HealAmount}.");
    }
}