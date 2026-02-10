using Deviloop;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Codecks.Runtime
{
    public class CodecksCardCreatorForm : MonoBehaviour
    {
        /// <summary>
        /// Reference to the CardCreator class inside the hierarchy.
        /// </summary>
        public CodecksCardCreator cardCreator;

        [SerializeField, TextArea()] private string defaultText;

        [Header("UI References")]
        public TMP_Dropdown categoryDropdown;
        public TMP_InputField textArea;
        public TMP_InputField emailInput;
        public TMP_Text statusText;
        public Button sendButton;

        [Header("Texts")]
        public string statusShortText;
        public string statusSending;
        public string statusSent;
        public string statusError;

        private byte[] queuedScreenshot;


        /// <summary>
        /// Shows the Codecks Report Form.
        /// </summary>
        public void ShowCodecksForm()
        {
            cardCreator.StartCoroutine(ShowCodecksFormCoroutine());
        }

        /// <summary>
        /// The coroutine that shows Codecks Report Form.
        /// </summary>
        private IEnumerator ShowCodecksFormCoroutine()
        {
            yield return new WaitForEndOfFrame();

            var screenshotTex = ScreenCapture.CaptureScreenshotAsTexture();

#if UNITY_STANDALONE
            queuedScreenshot = screenshotTex.EncodeToJPG();
#else
            // used on consoles to get screenshots easily
            queuedScreenshot = screenshotTex.EncodeToPNG();
#endif

            Destroy(screenshotTex);


            textArea.text = defaultText;
            sendButton.interactable = true;
            gameObject.SetActive(true);
        }

        byte[] QueueToBytes(Queue<string> textQueue)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Unity Logs:");
            sb.AppendLine("run seed: " + SeededRandom.GetSeed());

            foreach (string line in textQueue)
            {
                sb.AppendLine(line);
            }

            if(textQueue.Count == 0)
            {
                sb.AppendLine("<No logs captured.>");
            }

            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        /// <summary>
        /// Hides the Codecks Report Form
        /// </summary>
        public void HideCodecksForm()
        {
            queuedScreenshot = null;
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Hides the Codecks Report Form after a short delay so that there is enough time to read the status text that
        /// confirms sent reports.
        /// </summary>
        private IEnumerator HideCodecksFormWithDelayCoroutine()
        {
            yield return new WaitForSeconds(1);
            HideCodecksForm();
        }

        /// <summary>
        /// Called when the -Send Report- button is clicked.
        /// </summary>
        public void OnButtonSend()
        {
            if (textArea.text.Length < 10)
            {
                statusText.text = statusShortText;
                return;
            }

            string reportText = $"{textArea.text}\n\n";
            reportText += GetMetaText();

            var files = new Dictionary<string, (byte[], CodecksCardCreator.CodecksFileType)>();

            // TODO: can include log file in the report
#if UNITY_STANDALONE
            files["screenshot.jpg"] = (queuedScreenshot, CodecksCardCreator.CodecksFileType.JPG);
            files[$"unity_log_{System.DateTime.Now:yyyyMMdd_HHmmss}.txt"] = (QueueToBytes(CheatManager.GetLogs()), CodecksCardCreator.CodecksFileType.PlainText);
#else
            files["screenshot.png"] = (queuedScreenshot, CodecksCardCreator.CodecksFileType.PNG);
            files[$"unity_log_{System.DateTime.Now:yyyyMMdd_HHmmss}.txt"] = (QueueToBytes(CheatManager.GetLogs()), CodecksCardCreator.CodecksFileType.PlainText);
#endif

            statusText.text = statusSending;
            sendButton.interactable = false;

            cardCreator.CreateNewCard(
                text: reportText,
                files: files,
                severity: (CodecksCardCreator.CodecksSeverity)categoryDropdown.value,
                userEmail: emailInput.text,
                resultDelegate: (success, result) =>
                {
                    if (success)
                    {
                        statusText.text = statusSent;
                        sendButton.interactable = false;
                        StartCoroutine(HideCodecksFormWithDelayCoroutine());
                    }
                    else
                    {
                        sendButton.interactable = true;
                        statusText.text = statusError;
                    }
                });
        }

        /// <summary>
        /// Called when the Cancel button is clicked.
        /// </summary>
        public void OnButtonCancel()
        {
            HideCodecksForm();
        }

        /// <summary>
        /// This adds some game-related text information to the card content of the report. Feel free to add your own
        /// game data here that you want to be able see it at a glance.
        /// </summary>
        private static string GetMetaText()
        {
            StringBuilder metaText = new StringBuilder();
            metaText.AppendLine($"```");
            metaText.AppendLine($"Platform: {Application.platform.ToString()}");
            metaText.AppendLine($"App Version: {Application.version}");
            metaText.AppendLine("```");
            return metaText.ToString();
        }


    }
}
