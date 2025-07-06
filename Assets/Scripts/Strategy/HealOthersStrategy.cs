using UnityEngine;

public class HealOthersStrategy : IActionStrategy
{
    public void Execute(Character executor, Character target)
    {
        if (target == null) return;

        float dist = Vector2Int.Distance(executor.GridPosition, target.GridPosition);
        if (dist <= 2f)
        {
            target.Heal(executor.HealAmount);
            Debug.Log($"{executor.name} curó a {target.name} por {executor.HealAmount}.");
        }
        else
        {
            Debug.Log($"{target.name} está fuera de rango para curar.");
        }
    }
}