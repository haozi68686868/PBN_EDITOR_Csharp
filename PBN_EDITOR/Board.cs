using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace PBN_EDITOR
{
    public class Board
    {
        public int num;
        public string Vul;
        public string dealer;
        public string[,] hand = new string[4, 4];
        private int[] vacuumCount = new int[4];
        private int vacuumTotal;
        public Board()
        {
            setBoard(1, "None", "N");
        }

        public Board(int index, string mVul, string mDealer)
        {
            setBoard(index, mVul, mDealer);
        }
        public void setBoard(int index, string mVul, string mDealer)
        {
            num = index;
            Vul = mVul;
            dealer = mDealer;
        }
        public void setHand(string handStr)
        {
            string[] str = handStr.Split(' ');
            if (str.Length != 4)
            {
                MessageBox.Show("文件格式错误!");
                return;
            }
            for (int i = 0; i < 4; i++)
            {
                string[] strr = str[i].Split('.');
                for (int j = 0; j < 4; j++)
                {
                    hand[i, j] = strr[j];
                }
            }
        }
        public void setHand(string handStr,int startHand)
        {
            string[] str = handStr.Split(' ');
            if (str.Length != 4)
            {
                MessageBox.Show("文件格式错误!");
                return;
            }
            for (int i = 0; i < 4; i++)
            {
                int realHand = (i + startHand) % 4;
                string[] strr = str[i].Split('.');
                for (int j = 0; j < 4; j++)
                {
                    hand[realHand, j] = strr[j];
                }
            }
        }
        public string toText()
        {
            string s = "";
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    s = s + hand[i, j];
                    s += (j != 3 ? "." : "");
                }
                s += (i != 3 ? " " : "");
            }
            return s;
        }
        public bool check(out string errorMessage,bool checkComplete = true)
        {
            int i, j;
            string temp;
            string pattern;
            string messageTemp = "";
            errorMessage = "";
            for (i = 0; i < 4; i++)
            {
                temp = hand[i, 0] + hand[i, 1] + hand[i, 2] + hand[i, 3];
                if (checkComplete && temp.Length != 13)
                {
                    switch (i)
                    {
                        case 0: messageTemp = "北"; break;
                        case 1: messageTemp = "东"; break;
                        case 2: messageTemp = "南"; break;
                        case 3: messageTemp = "西"; break;
                    }
                    errorMessage = String.Format("{0}家不为13张牌", messageTemp);
                    return false;
                }
            }
            for (i = 0; i < 4; i++)
            {
                temp = hand[0, i] + hand[1, i] + hand[2, i] + hand[3, i];
                if (checkComplete && temp.Length != 13)
                {
                    switch (i)
                    {
                        case 0: messageTemp = "黑桃"; break;
                        case 1: messageTemp = "红桃"; break;
                        case 2: messageTemp = "方片"; break;
                        case 3: messageTemp = "草花"; break;
                    }
                    errorMessage = String.Format("{0}不为13张牌", messageTemp);
                    return false;
                }
                pattern = "(\\w).*\\1";
                if (Regex.IsMatch(temp, pattern, RegexOptions.IgnoreCase))
                {
                    switch (i)
                    {
                        case 0: messageTemp = "黑桃"; break;
                        case 1: messageTemp = "红桃"; break;
                        case 2: messageTemp = "方片"; break;
                        case 3: messageTemp = "草花"; break;
                    }
                    errorMessage = String.Format("{0}中出现重复的牌张", messageTemp);
                    return false;
                }
            }
            pattern = "[^AKQJT23456789]";
            for (i = 0; i < 4; i++)
            {
                for (j = 0; j < 4; j++)
                {
                    if (Regex.IsMatch(hand[i, j], pattern, RegexOptions.IgnoreCase))
                    {
                        errorMessage = "出现无法识别的牌张";
                        return false;
                    }
                }
            }
            return true;
        }
        public void EXG_EW()
        {
            string temp;
            for (int i = 0; i < 4; i++)
            {
                temp = hand[1, i];
                hand[1, i] = hand[3, i];
                hand[3, i] = temp;
            }
        }
        public void EXG_NS()
        {
            string temp;
            for (int i = 0; i < 4; i++)
            {
                temp = hand[0, i];
                hand[0, i] = hand[2, i];
                hand[2, i] = temp;
            }
        }
        public void Rotate()
        {
            string temp;
            for (int i = 0; i < 4; i++)
            {
                temp = hand[0, i];
                hand[0, i] = hand[3, i];
                hand[3, i] = hand[2, i];
                hand[2, i] = hand[1, i];
                hand[1, i] = temp;
            }
        }
        public void RandomDeal()
        {
            for (int i = 0; i < 4; i++)
            {
                vacuumCount[i] = 13;
                for (int j = 0; j < 4; j++)
                {
                    hand[i, j] = "";
                }
            }
            vacuumTotal = 52;
            for (int i = 0; i < 4; i++)
            {
                RandomCards(i, "AKQJT98765432");
            }
        }
        public void Random_Rest()
        {
            freshVaccumCount();
            for (int i = 0; i < 4; i++)
            {
                string cards_assigned = "AKQJT98765432";
                string temp = hand[0, i] + hand[1, i] + hand[2, i] + hand[3, i];
                foreach (char c in cards_assigned)
                {
                    if (!temp.Contains(c))
                    {
                        Random_card(i, c);
                    }
                }
            }
        }
        public void RandomCards(int suit, string cards)
        {
            
            foreach (char c in cards)
            {
                Random_card(suit, c);
            }
        }
        private void Random_card(int suit,char card)
        {
            int tmp;
            tmp = Global_Random.rd.Next(vacuumTotal);
            for (int i = 0; i < 4; i++)
            {
                if (tmp < vacuumCount[i])
                {
                    vacuumCount[i]--;
                    vacuumTotal--;
                    hand[i, suit] += card;
                    break;
                }
                else
                {
                    tmp -= vacuumCount[i];
                }
            }
        }
        private void freshVaccumCount()
        {
            vacuumTotal = 0;
            //已保证格式正确
            for (int i = 0; i < 4; i++)
            {
                vacuumCount[i] = 13 - (hand[i, 0].Length + hand[i, 1].Length + hand[i, 2].Length + hand[i, 3].Length);
                vacuumTotal += vacuumCount[i];
            }
        }
        public void exchangeSuit(int suit1, int suit2)
        {
            string temp;
            for (int i = 0; i < 4; i++)
            {
                temp = hand[i, suit1];
                hand[i, suit1] = hand[i, suit2];
                hand[i, suit2] = temp;
            }
        }
    }
}
