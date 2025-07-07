using UnityEngine;
using System.Collections.Generic;

public class CharacterUIManager : MonoBehaviour
{
    [SerializeField] private Transform enemyInfoContainer;
    [SerializeField] private Transform playerInfoContainer;
    [SerializeField] private GameObject characterInfoPrefab;
    [SerializeField] private GameObject activePlayerFrame;

    private List<CharacterInfoUI> enemyUIList = new List<CharacterInfoUI>();
    private List<CharacterInfoUI> playerUIList = new List<CharacterInfoUI>();

    public void BuildPanels(List<Enemy> enemies, List<Player> players)
    {
        foreach (Transform child in enemyInfoContainer)
            Destroy(child.gameObject);

        foreach (Transform child in playerInfoContainer)
            Destroy(child.gameObject);

        enemyUIList.Clear();
        playerUIList.Clear();

        foreach (var enemy in enemies)
        {
            var uiObj = Instantiate(characterInfoPrefab, enemyInfoContainer);
            var ui = uiObj.GetComponent<CharacterInfoUI>();
            ui.linkedCharacter = enemy;
            ui.SetInfo(enemy.GetCharacterSprite(), enemy.name, enemy.CurrentHP);
            enemyUIList.Add(ui);
        }

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
        for (int i = 0; i < enemyUIList.Count; i++)
        {
            if (enemyUIList[i].linkedCharacter == character)
            {
                Destroy(enemyUIList[i].gameObject);
                enemyUIList.RemoveAt(i);
                break;
            }
        }

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
    public void HighlightActivePlayer(Character activeCharacter)
    {

        foreach (var ui in enemyUIList)
        {
            if (ui.linkedCharacter == activeCharacter)
            {
                activePlayerFrame.SetActive(true);
                activePlayerFrame.transform.SetParent(ui.transform, false);
                activePlayerFrame.transform.SetAsFirstSibling();
                activePlayerFrame.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                return;
            }
        }

        foreach (var ui in playerUIList)
        {
            if (ui.linkedCharacter == activeCharacter)
            {
                activePlayerFrame.SetActive(true);
                activePlayerFrame.transform.SetParent(ui.transform, false);
                activePlayerFrame.transform.SetAsFirstSibling();
                activePlayerFrame.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                return;
            }
        }
    }
}