using Steamwar.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

namespace Steamwar.UI
{
    public class LoadingScreen : Singleton<LoadingScreen>
    {
        private const float MIN_TIME_TO_SHOW = 1f;
        private AsyncOperation currentLoadingOperation;
        private bool isLoading;
        [SerializeField]
        private RectTransform barFillRectTransform;
        private Vector3 barFillLocalScale;
        [SerializeField]
        private Text percentLoadedText;
        [SerializeField]
        private Text loadingText;
        private float timeElapsed;
        [SerializeField]
        private bool hideProgressBar;
        [SerializeField]
        private bool hidePercentageText;
        private Animator animator;
        private bool didTriggerFadeOutAnimation;

        private void Awake()
        {
            Configure();
            Hide();
        }

        private void Configure()
        {
            barFillLocalScale = barFillRectTransform.localScale;
            barFillRectTransform.transform.parent.gameObject.SetActive(!hideProgressBar);
            percentLoadedText.gameObject.SetActive(!hidePercentageText);
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (isLoading)
            {
                SetProgress(currentLoadingOperation.progress);
                if (currentLoadingOperation.isDone && !didTriggerFadeOutAnimation)
                {
                    animator.SetTrigger("Hide");
                    didTriggerFadeOutAnimation = true;
                }
                else
                {
                    timeElapsed += Time.deltaTime;
                    if (timeElapsed >= MIN_TIME_TO_SHOW)
                    {
                        currentLoadingOperation.allowSceneActivation = true;
                    }
                }
            }
        }

        private void SetProgress(float progress)
        {
            barFillLocalScale.x = progress;
            barFillRectTransform.localScale = barFillLocalScale;
            percentLoadedText.text = Mathf.CeilToInt(progress * 100).ToString() + "%";
        }

        public void Show(AsyncOperation loadingOperation)
        {
            gameObject.SetActive(true);
            currentLoadingOperation = loadingOperation;
            currentLoadingOperation.allowSceneActivation = false;
            SetProgress(0f);
            timeElapsed = 0f;
            animator.SetTrigger("Show");
            didTriggerFadeOutAnimation = false;
            isLoading = true;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            currentLoadingOperation = null;
            isLoading = false;
        }
    }
}
