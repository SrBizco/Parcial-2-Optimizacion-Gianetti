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
            Debug.Log($"{executor.name} atac� a distancia a {target.name} por {executor.RangedDamage} de da�o.");
        }
        else
        {
            Debug.Log($"{executor.name} est� fuera de rango de disparo.");
        }
    }
}
