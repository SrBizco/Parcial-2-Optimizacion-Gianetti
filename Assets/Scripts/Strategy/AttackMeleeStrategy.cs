using UnityEngine;

public class AttackMeleeStrategy : IActionStrategy
{
    public void Execute(Character executor, Character target)
    {
        if (target == null) return;

        int dx = Mathf.Abs(executor.GridPosition.x - target.GridPosition.x);
        int dy = Mathf.Abs(executor.GridPosition.y - target.GridPosition.y);

        if (dx <= 1 && dy <= 1 && (dx != 0 || dy != 0))
        {
            target.ReceiveDamage(executor.MeleeDamage);
            Debug.Log($"{executor.name} atacó con melee a {target.name} por {executor.MeleeDamage} de daño.");
        }
        else
        {
            Debug.Log($"{executor.name} está fuera de rango melee.");
        }
    }
}