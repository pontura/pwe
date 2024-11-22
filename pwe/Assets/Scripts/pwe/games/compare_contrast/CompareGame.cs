using Pwe.Core;
using Pwe.Games.Common;
using Pwe.Games.Cooking;
using System.Collections.Generic;
using UnityEngine;
using Yaguar.Inputs2D;

namespace Pwe.Games.Compare
{
    public class CompareGame : GameMain
    {
        public states state;
    public enum states
    {
        playing,
        done
    }
    [SerializeField] InteractiveElement[] pieces;
    [SerializeField] Transform dragContainer;


    Dictionary<string, int> ingredients;
    Dictionary<string, int> ingredientsAdded;
    int totalPieces;

    int total;
    int num;
    List<ItemData> items;
    int itemID;
    string lastIngredient;
    public override void OnInit()
    {
        int level = 0;

        if (GamesManager.Instance != null)
            level = GamesManager.Instance.GetGame(GameData.GAMES.COOKING).level;

        items = new List<ItemData>();
        //items = (Game as CompareGame).CompareData.GetItems(level);
        //ingredients = new Dictionary<string, int>();
        //ingredientsAdded = new Dictionary<string, int>();
        //foreach (ItemData item in items)
        //{
        //    totalPieces += item.num;
        //    ingredients.Add(item.item.ToString(), item.num);
        //    ingredientsAdded.Add(item.item.ToString(), 0);
        //}
    }

    private void NextClicked()
    {
    }

    void InitIngredient()
    {
        num = 0;
        state = states.playing;
        total = items[itemID].num;

        string ingredient = items[itemID].item.ToString();
    }
    void ResetOtherIngredients(string ingredient)
    {
        foreach (ItemData itemData in items)
        {
            string s = itemData.item.ToString();
            if (s != ingredient)
                ResetIngredient(s);// RESET
        }
    }
    public void InitDrag()
    {
    }
    void OnPieceToDragReady()
    {
        string ingredient = items[itemID].item.ToString();
        //mainPiece.InitIngredient("qty_" + ingredient, total);
        Invoke("InitPieceToDragDelayed", 0.1f);
        //dragInputManager.ForceDrag(Input.mousePosition, newPieceToDrag);
    }
    void ResetIngredient(string ingredient)
    {
    }
    void CheckFinish()
    {
        print("DONE_____________" + ingredients.Keys.Count);
        if (ingredients.Keys.Count <= 0)
            print("DONE");
    }
    public void OnPieceAdded(string ingredient)
    {
      
    }
    public bool CanMove()
    {
        return true;
    }
}

}