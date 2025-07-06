using UnityEngine;
using System.Collections.Generic;

public class CharacterUIManager : MonoBehaviour
{
    public Transform enemyInfoContainer; // ✅ Nuevo: container interno
    public Transform playerInfoContainer; // ✅ Nuevo: container interno
    public GameObject characterInfoPrefab;

    private List<CharacterInfoUI> enemyUIList = new List<CharacterInfoUI>();
    private List<CharacterInfoUI> playerUIList = new List<CharacterInfoUI>();

    public void BuildPanels(List<Enemy> enemies, List<Player> players)
    {
        // Limpiar solo containers internos (NO borra textos fijos)
        foreach (Transform child in enemyInfoContainer)
            Destroy(child.gameObject);

        foreach (Transform child in playerInfoContainer)
            Destroy(child.gameObject);

        enemyUIList.Clear();
        playerUIList.Clear();

        // Crear UI para enemigos
        foreach (var enemy in enemies)
        {
            var uiObj = Instantiate(characterInfoPrefab, enemyInfoContainer);
            var ui = uiObj.GetComponent<CharacterInfoUI>();
            ui.linkedCharacter = enemy;
            ui.SetInfo(enemy.GetCharacterSprite(), enemy.name, enemy.CurrentHP);
            enemyUIList.Add(ui);
        }

        // Crear UI para players
        foreach (var player in players)
        {
            var uiObj = Instantiate(characterInfoPrefab, playerInfoContainer);
            var ui = uiObj.GetComponent<CharacterInfoUI>();
            ui.linkedCharacter = player;
            ui.SetInfo(player.GetCharacterSprite(), player.name, player.CurrentHP);
            playerUIList.Add(ui);
        }
    }

    public void UpdateHP(List<Enemy> enemies, List<Player> players)
    {
        for (int i = 0; i < enemyUIList.Count; i++)
        {
            if (i < enemies.Count)
                enemyUIList[i].UpdateHP(enemies[i].CurrentHP);
        }

        for (int i = 0; i < playerUIList.Count; i++)
        {
            if (i < players.Count)
                playerUIList[i].UpdateHP(players[i].CurrentHP);
        }
    }

    public void RemovePanel(Character character)
    {
        // Para enemies
        for (int i = 0; i < enemyUIList.Count; i++)
        {
            if (enemyUIList[i].linkedCharacter == character)
            {
                Destroy(enemyUIList[i].gameObject);
                enemyUIList.RemoveAt(i);
                break;
            }
        }

        // Para players
        for (int i = 0; i < playerUIList.Count; i++)
        {
            if (playerUIList[i].linkedCharacter == character)
            {
                Destroy(playerUIList[i].gameObject);
                playerUIList.RemoveAt(i);
                break;
            }
        }
    }
}
