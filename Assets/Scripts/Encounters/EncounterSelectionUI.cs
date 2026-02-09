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
                    EncounterItems[i].Title.text = encounters[i].EncounterName;
#if DEBUG
                    EncounterItems[i].DebugTitle.text = "debug name: " + encounters[i].EncounterDebugName;
#else
                    EncounterItems[i].DebugTitle.gameObject.SetActive(false);
#endif

                    EncounterItems[i].Icon.sprite = encounters[i].EncounterIcon;
                    EncounterItems[i].EliteReward.SetActive(encounters[i] is CombatEncounter ce && ce.IsElite);

                    int index = i; // Capture the current index for the lambda
                    EncounterItems[i].Select.onClick.RemoveAllListeners();
                    EncounterItems[i].Select.onClick.AddListener(() =>
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
