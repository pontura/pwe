using UnityEngine;

namespace Pwe.Games.Common
{
    public class BigNumberSignal : MonoBehaviour
    {
        [SerializeField] TMPro.TMP_Text field;

        public void Init(int num)
        {
            field.text = num.ToString();
        }
    }
}
