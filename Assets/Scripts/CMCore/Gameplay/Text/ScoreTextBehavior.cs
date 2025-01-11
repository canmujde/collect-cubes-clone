using System;
using CMCore.Gameplay.Character;
using CMCore.Interfaces;
using CMCore.Managers;
using CMCore.Util;
using TMPro;
using UnityEngine;

namespace CMCore.Gameplay.Text
{
    public class ScoreTextBehavior : MonoBehaviour, IResetable
    {
        [SerializeField] private TextMeshPro text;
        [field: SerializeField] public CharacterBase BelongsTo { get; private set; }
        
        public void ResetBehaviour()
        {
            
            UpdateText(0);

            this.DelayedAction(FindBelong,0.4f);
        }

        private void FindBelong()
        {
            var characters = FindObjectsOfType<CharacterBase>();
            var z = transform.position.z;
            
            var find = Array.Find(characters, cb => z > 0 ? cb.transform.position.z > 0 : cb.transform.position.z < 0);
            BelongsTo = find;
            gameObject.name = find.name+"ScoreText";
            GameManager.Instance.UIManager.scoreTexts.Add(this);
            SetColor();
        }

        private void SetColor()
        {
            if (gameObject.name == Constants.Words.PlayerScoreText)
            {
                text.color =  new Color(1, 0.4141115f, 0, 1);
            }
            else
            {
                text.color = Color.red;
            }
        }

        public void UpdateText(int playerScore)
        {
            text.text = playerScore.Abbreviate();
        }
    }
}
