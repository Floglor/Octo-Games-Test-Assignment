using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CSScripts
{
    public class QuestLog : MonoBehaviour
    {
        [SerializeField] private List<Log> _logs;
        [SerializeField] private GameObject _questDescriptionWindow;

        private void Start()
        {
            foreach (Log log in _logs)
            {
                log.TextToggle.onValueChanged.AddListener(delegate(bool toggle)
                {
                    _questDescriptionWindow.SetActive(toggle);
                    foreach (Log logInternal in _logs)
                    {
                        if (logInternal == log) continue;
                        logInternal.DisableImage();
                        logInternal.TextToggle.SetIsOnWithoutNotify(false);
                    }
                
                    if (toggle == false)
                    {
                        log.DisableImage();
                        return;
                    }
                
                    _questDescriptionWindow.GetComponentInChildren<TextMeshProUGUI>().text = log.GetDescription();
                    log.EnableImage();
                });
            
                if (log.ProgressStep > 0)
                {
                    log.gameObject.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
                    log.gameObject.GetComponentInChildren<Image>().enabled = false;
                }
            }
        }

        private void OnDisable()
        {
            _questDescriptionWindow.gameObject.SetActive(false);

            foreach (Log log in _logs)
            {
                log.TextToggle.SetIsOnWithoutNotify(false);
                log.DisableImage();
            }
        }

        public void ProgressQuest(int progress)
        {
            foreach (Log log in _logs)
            {
                if (log.ProgressStep == progress)
                {
                    log.gameObject.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
                    log.gameObject.GetComponentInChildren<Image>().enabled = true;

                }
                if (log.ProgressStep < progress)
                {
                    log.ToggleCheckBox();
                }
            }
        }
    }
}