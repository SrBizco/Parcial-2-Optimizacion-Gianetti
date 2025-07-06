using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterInfoUI : MonoBehaviour
{
    public Image characterSpriteImage;
    public TMP_Text nameText;
    public TMP_Text hpText;
    public Character linkedCharacter;

    public void SetInfo(Sprite sprite, string name, int hp)
    {
        characterSpriteImage.sprite = sprite;
        nameText.text = name;
        hpText.text = $"HP: {hp}";
    }

    public void UpdateHP(int hp)
    {
        hpText.text = $"HP: {hp}";
    }
}
