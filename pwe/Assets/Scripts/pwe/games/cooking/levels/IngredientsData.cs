using System;
using UnityEngine;
using System.Collections.Generic;

namespace Pwe.Games.Cooking
{

	[CreateAssetMenu(fileName = "CookingIngredients", menuName = "Cooking/Data/Ingredients", order = 0)]
	public class IngredientsData : ScriptableObject
    {
        [field: SerializeField] public List<BaseData> bases { get; private set; }
        [field: SerializeField] public List<IngredientData> ingredients { get; private set; }

        [Serializable]
        public class IngredientData
        {
            public ItemData.Items item;
            public Sprite asset;
            public Sprite[] assets;
            public Color color;
            public Sprite GetSprite(int id = 0)
            {
                if (id == 0 || asset == null || assets.Length == 0)
                    return asset;
                else if (id<assets.Length)
                    return assets[id];
                else return asset;
            }
        }

        [Serializable]
        public class BaseData
        {
            public string baseName;
            public Sprite asset;
        }
    }
}
