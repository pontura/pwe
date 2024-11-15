using UnityEngine;
using YaguarLib.UI;

namespace Pwe.Core
{
    public class ButtonProgressBar : ButtonUIIcon
    {
        [SerializeField] GameObject doneGO;
        [SerializeField] ProgressBar progressBar;
        System.Action<bool> OnClicked;
        float progress = 0f;
        bool isDone;

        private void NextClicked()
        {
            OnClicked(isDone);
        }
        public void SetProgress(int value, int total)
        {
            float progress = (float)value / (float)total;
            print("progress_____________" + progress + " numPiecesDone:" + value + "/" + total);
            progressBar.SetValue(progress);
            if (value >= total)
                Ready();
        }
        public bool IsReady()
        {
            return isDone;
        }
        void Ready()
        {
            if (!isDone)
            {
                isDone = true;
                doneGO.SetActive(isDone);
            }
        }
    }
}
