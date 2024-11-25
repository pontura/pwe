using Pwe.Core;
using System.Collections.Generic;
using UnityEngine;
using YaguarLib.Xtras;

namespace Pwe.Games.Cooking
{
    public class CookingPieces : MonoBehaviour
    {
        [SerializeField] PiecesContainer piece_to_add;
        [SerializeField] Transform container;
        [SerializeField] UIScroller uiScroller;
        List<ItemData> items;
        int separation_y = 270;
        List<PiecesContainer> piecesContainers;
        public void Initialize(Cooking cooking, List<ItemData> items, CookingMainPiece mainPiece)
        {
            piecesContainers = new List<PiecesContainer>(); 
            Utils.RemoveAllChildsIn(container);
            this.items = items;
            int id = 0;
            foreach (ItemData item in items)
            {
                PiecesContainer piecesContainer = Instantiate(piece_to_add, container);
                piecesContainer.Initialize(id, items, cooking, mainPiece);
                piecesContainer.transform.localPosition = new Vector2(0, id * separation_y);
                piecesContainer.SetSignalQty(item.num);
                piecesContainers.Add(piecesContainer);
                id++;
            }
            uiScroller.Init(0, items.Count, separation_y);
        }
        public void OnIngredientReady(string ingredient)
        {
            foreach (PiecesContainer pc in piecesContainers)
                pc.CheckIngredientReady(ingredient);
        }
    }
}