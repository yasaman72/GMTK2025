using System.Collections.Generic;
using UnityEngine;

namespace Deviloop
{
    public class EncounterSelectionUI : MonoBehaviour
    {
        public EncounterItemUI[] EncounterItems;

        public void ShowNextSelections(List<BaseEncounter> encounters)
        {
            gameObject.SetActive(true);
            for (int i = 0; i < EncounterItems.Length; i++)
            {
                if (i < encounters.Count)
                {
                    EncounterItems[i].gameObject.SetActive(true);
                    EncounterItems[i].Title_txt.text = encounters[i].EncounterName;
                    EncounterItems[i].Icon_img.sprite = encounters[i].EncounterIcon;
                    int index = i; // Capture the current index for the lambda
                    EncounterItems[i].Select_btn.onClick.RemoveAllListeners();
                    EncounterItems[i].Select_btn.onClick.AddListener(() =>
                    {
                        EncounterManager.CurrentEncounter = encounters[index];
                        encounters[index].StartEncounter();
                        Hide();
                    });
                }
                else
                {
                    EncounterItems[i].gameObject.SetActive(false);
                }
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
