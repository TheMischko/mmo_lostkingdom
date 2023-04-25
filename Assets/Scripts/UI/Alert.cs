using System;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class Alert : MonoBehaviour {
        private Image image;
        private Text text;
        private CanvasGroup group;

        private bool showMessageSig = false;
        private string showMessage = string.Empty;
        private AlertType showAlertType = AlertType.Success;

        private void Awake() {
            image = GetComponent<Image>();
            text = GetComponentInChildren<Text>();
            group = GetComponent<CanvasGroup>();
        }

        private void FixedUpdate() {
            if (showMessageSig) {
                showMessageSig = false;
                text.text = showMessage;
                ChangeType(showAlertType);
                Show();
            }
        }

        public void Show() {
            group.alpha = 1f;
        }

        public void Hide() {
            group.alpha = 0f;
        }

        public void ShowMessage(string message, AlertType type) {
            showMessage = message;
            showAlertType = type;
            showMessageSig = true;
        }

        public void ChangeType(AlertType type) {
            switch (type) {
                case AlertType.Success:
                    image.color = new Color(85/255f, 138/255f, 80/255f);
                    text.color = Color.white;
                    break;
                case AlertType.Error:
                    image.color = new Color(70/255f, 54/255f, 54/255f);
                    text.color = new Color(255/255f, 97/255f, 97/255f);
                    break;
            }
            Canvas.ForceUpdateCanvases();
        }
    }

    public enum AlertType {
        Success,
        Error
    }
}