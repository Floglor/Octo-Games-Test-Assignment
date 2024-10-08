using System.Collections.Generic;
using Naninovel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CSScripts
{
    public class QuestLog : MonoBehaviour
    {
        [SerializeField] private List<Log> _logs;
        [SerializeField] private GameObject _questDescriptionWindow;

        private void OnEnable()
        {
            AddLogListeners();
            ReinitializeLog();
        }

        private void AddLogListeners()
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
                        logInternal.ToggleCheckBoxOff();
                    }

                    if (toggle == false)
                    {
                        log.DisableImage();
                        return;
                    }

                    _questDescriptionWindow.GetComponentInChildren<TextMeshProUGUI>().text = log.GetDescription();
                    log.EnableImage();
                });
            }
        }

        private void ReinitializeLog()
        {
            ICustomVariableManager variableManager = Engine.GetService<ICustomVariableManager>();
            int progressStep = 0;
            variableManager.TryGetVariableValue<int>("QuestProgress", out progressStep);

            foreach (Log log in _logs)
            {
                log.ToggleCheckBoxOff();
                if (log.ProgressStep > progressStep)
                {
                    log.gameObject.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
                    log.gameObject.GetComponentInChildren<Image>().enabled = false;
                }

                if (log.ProgressStep == progressStep)
                {
                    log.ToggleCheckBoxOff();
                }
                else if (log.ProgressStep < progressStep + 1)
                {
                    log.ToggleCheckBoxOn();
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
            ReinitializeLog();
            foreach (Log log in _logs)
            {
                if (log.ProgressStep == progress)
                {
                    log.gameObject.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
                    log.gameObject.GetComponentInChildren<Image>().enabled = true;
                }

                if (log.ProgressStep < progress)
                {
                    log.ToggleCheckBoxOn();
                }
            }
        }
    }
}