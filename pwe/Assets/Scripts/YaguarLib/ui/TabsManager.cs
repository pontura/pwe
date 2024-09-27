using UnityEngine;
using System;
using System.Collections.Generic;
using YaguarLib.Xtras;

namespace YaguarLib.UI
{
    public class TabsManager : MonoBehaviour
    {
        [SerializeField] List<ListItemData> all;


        [Serializable]
        public class ListItemData
        {
            public string name;
            public Tab tab;
            public Sprite icon;
        }

        [SerializeField] ButtonUIIcon button;
        List<ButtonUIIcon> buttons;

        [SerializeField] Transform container;

        System.Action<string> OnTabClicked; // if null: just opens the tab

        public void Init(System.Action<string> OnTabClicked = null)
        {
            this.OnTabClicked = OnTabClicked;
            SetButtons();
            Select(0);
        }
        public void SetButtons()
        {
            buttons = new List<ButtonUIIcon>();
            Utils.RemoveAllChildsIn(container);
            int id = 0;
            foreach (ListItemData data in all)
            {
                ButtonUIIcon b = Instantiate(button, container);
                b.Init(Select, id);
                if (name == "") name = data.name;
                b.SetText(name);
                b.SetIcon(data.icon);
                buttons.Add(b);
                id++;
            }
        }
        int lastSelected;
        public void Refresh()
        {
            Select(lastSelected);
        }
        public void Select(int id)
        {
            lastSelected = id;
            foreach (ButtonUI b in buttons)
                b.OnSelected(false);

            if (OnTabClicked != null)
            {
                OnTabClicked(all[id].name);
            }
            int _id = 0;
            foreach (ListItemData data in all)
            {
                if (data.tab != null)
                    data.tab.SetTab(_id == id);
                _id++;
            }

            buttons[id].OnSelected(true);
        }
        public void SetTabText(int tabID, string text)
        {
            buttons[tabID].SetText(text);
        }
        public int GetTotal()
        {
            return all.Count;
        }
        public int GetButtonByKey(string key)
        {
            int id = 0;
            foreach (ListItemData b in all)
            {
                if (b.name == key)
                    return id;
                id++;
            }
            return 0;
        }
    }
}