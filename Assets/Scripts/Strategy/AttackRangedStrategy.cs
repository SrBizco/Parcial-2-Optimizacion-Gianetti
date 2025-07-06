using UnityEngine;

public class AttackRangedStrategy : IActionStrategy
{
    public void Execute(Character executor, Character target)
    {
        if (target == null) return;

        float dist = Vector2Int.Distance(executor.GridPosition, target.GridPosition);
        if (dist <= executor.RangedRange)
        {
            target.ReceiveDamage(executor.RangedDamage);
            Debug.Log($"{executor.name} atacó a distancia a {target.name} por {executor.RangedDamage} de daño.");
        }
        else
        {
            Debug.Log($"{executor.name} está fuera de rango de disparo.");
        }
    }
}
