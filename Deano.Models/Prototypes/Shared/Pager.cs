using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deano.Models.Prototypes.Shared
{
    public class Pager : Prototype
    {
        public decimal Total { get; set; } //total number of pages

        public Decimal Available { get; set; } //number of pages available before showing ... or next

        public int FirstIndex { get; set; }

        public int LastIndex { get; set; }

        public int SelectedIndex { get; set; }

        public List<Page> Pages { get; set; }

        public Pager()
        {
            Total = 0;
            FirstIndex = 0;
            LastIndex = 0;
            SelectedIndex = 0;
            Available = 5;
            Pages = new List<Page>();
        }
        public Pager(int firstIndex, decimal total, int selectedIndex)
            : this(firstIndex, total, selectedIndex, 5)
        {

        }

        public Pager(int firstIndex, decimal total, int selectedIndex, int available)
            : this()
        {
            this.Available = available;
            if (selectedIndex - firstIndex >= Available) //click next ...
            {
                firstIndex = selectedIndex;
            }
            else if (firstIndex > 0 && firstIndex == selectedIndex + 1) //clicked previous ...
            {
                firstIndex = firstIndex - int.Parse(Available.ToString());
            }

            Total = total;
            FirstIndex = firstIndex;
            SelectedIndex = selectedIndex;

            if (FirstIndex + Available > Total)
            {
                FirstIndex = int.Parse(Total.ToString()) - int.Parse(Available.ToString());
            }
            FirstIndex = FirstIndex < 0 ? 0 : FirstIndex;

            LastIndex = FirstIndex + int.Parse(Available.ToString()) - 1;
            if (LastIndex >= Total)
            {
                LastIndex = int.Parse(Total.ToString()) - 1;
            }
            LastIndex = LastIndex < 0 ? 0 : LastIndex;

            Page page = null;
            int pageCount = (int)Math.Ceiling(total / Available);
            for (int i = 0; i < pageCount; i++)
            {
                if (i < FirstIndex - 1 || i > LastIndex + 1)
                {
                    page = new Page((i + 1).ToString(), i, false);
                }
                else if (i == FirstIndex - 1)
                {
                    page = new Page("...", i, true);
                }
                else if (i >= FirstIndex && i <= LastIndex)
                {
                    page = new Page((i + 1).ToString(), i, true);
                }
                else if (i == LastIndex + 1)
                {
                    page = new Page("...", i, true);
                }

                if (page != null)
                {
                    page.Selected = i == selectedIndex;
                    Pages.Add(page);
                }
            }
        }

    }
}
