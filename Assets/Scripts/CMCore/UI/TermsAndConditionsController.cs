using CMCore.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace CMCore.UI
{
    public class TermsAndConditionsController : MonoBehaviour
    {
        private const string TermsUrl = "";
        private const string PrivacyUrl = "";

        public Button termsButton;
        public Button privacyButton;
        public Button acceptButton;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            Check(out var isAccepted);
            if (isAccepted)
            {
                gameObject.SetActive(false);
                return;
            }

            acceptButton.onClick.AddListener(Accept_OnClick);
            termsButton.onClick.AddListener(Terms_OnClick);
            privacyButton.onClick.AddListener(Privacy_OnClick);
        }

        private static void Check(out bool isAccepted)
        {
            isAccepted = GameManager.Instance.DataManager.GetData("TermsAndPrivacyAccepted", 0) == 1;
        }

        private static void Terms_OnClick()
        {
            Application.OpenURL(TermsUrl);
        }

        private static void Privacy_OnClick()
        {
            Application.OpenURL(PrivacyUrl);
        }

        private void Accept_OnClick()
        {
            GameManager.Instance.DataManager.SetData("TermsAndPrivacyAccepted", 1);
            Initialize();
        }
    }
}