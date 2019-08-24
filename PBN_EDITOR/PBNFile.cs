using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace PBN_EDITOR
{
    public class PBNFile
    {
        public List<Board> boardList;
        public int index;
        public static Form1 form1;
        public string name;
        public bool textChanged;
        public PBNFile()
        {
            init();
            newDeal();
        }
        private void init()
        {
            boardList = new List<Board>();
            index = 0;
            name = "";
            textChanged = false;
        }
        public void Save(string fileName)
        {
            using (StreamWriter writer = new StreamWriter(File.Open(fileName, FileMode.Create),System.Text.Encoding.Default))
            {
                writer.WriteLine("%[ThisName1]:");
                writer.WriteLine("%[ThisNote1]:");
                writer.WriteLine("%[ThisNums1]:{0}",boardList.Count);
                foreach (Board b in boardList)
                {
                    writer.WriteLine("[Board \"{0}\"]",b.num);
                    writer.WriteLine("[Dealer \"{0}\"]", b.dealer);
                    writer.WriteLine("[Vulnerable \"{0}\"]", b.Vul);
                    writer.WriteLine("[Deal \"N:{0}\"]", b.toText());
                    writer.WriteLine();
                }
                writer.Close();
            }
            int a = fileName.LastIndexOf("\\");
            name = fileName.Substring(a + 1);
            Show();
            textChanged = false;
        }
        public bool Load(string fileName)
        {
            init();
            int a = fileName.LastIndexOf("\\");
            name = fileName.Substring(a + 1);
            return Insert(fileName);
        }
        public bool Insert(string fileName)
        {
            string vul;
            string dealer;
            string tempHand = "";
            int bdNum;
            string temp;
            string[] tempstr;
            int startHand = 0;
            Board b;
            if (File.Exists(fileName))
            {
                using (StreamReader reader = new StreamReader(File.Open(fileName, FileMode.Open), System.Text.Encoding.Default))
                {
                    try
                    {
                        while (!reader.EndOfStream)
                        {
                            temp = reader.ReadLine();
                            if (temp.StartsWith("%")) continue;
                            if (temp.StartsWith("[Board"))
                            {
                                vul = "Bo";
                                dealer = "Q";
                                tempstr = temp.Split('\"');
                                bdNum = Convert.ToInt32(tempstr[1]);
                                while (!reader.EndOfStream)
                                {
                                    temp = reader.ReadLine();
                                    if (temp == "") break;
                                    if (temp.StartsWith("[Dealer "))
                                    {
                                        tempstr = temp.Split('\"');
                                        dealer = tempstr[1];
                                    }
                                    if (temp.StartsWith("[Vulnerable "))
                                    {
                                        tempstr = temp.Split('\"');
                                        vul = tempstr[1];
                                    }
                                    if (temp.StartsWith("[Deal "))
                                    {
                                        tempstr = temp.Split('\"');
                                        tempHand = tempstr[1]; 
                                        //tempHand = tempstr[1].Substring(2);

                                        if (tempHand.StartsWith("N")) startHand = 0;
                                        else if (tempHand.StartsWith("E")) startHand = 1;
                                        else if (tempHand.StartsWith("S")) startHand = 2;
                                        else if (tempHand.StartsWith("W")) startHand = 3;
                                        else
                                            startHand = 0;
                                        tempHand = tempHand.Substring(2);
                                    }
                                }
                                b = new Board(bdNum, vul, dealer);
                                b.setHand(tempHand,startHand);
                                boardList.Add(b);
                            }
                            else
                                continue;
                        }
                        reader.Close();
                    }
                    catch (IOException e)
                    {
                        MessageBox.Show("文件读入错误!\n" + e.ToString());
                        return false;
                    }
                }
                Show();
                textChanged = false;
                return true;
            }
            return false;
        }
        public void Show()
        {
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    form1.TBArray[i, j].Text = boardList[index].hand[i, j];
                }
            form1.labelBoardNum.Text = Convert.ToString(boardList[index].num);
            form1.labelDealer.Text = boardList[index].dealer;
            form1.labelVul.Text = boardList[index].Vul;
            if (name.Equals(""))
                form1.Text = "PBN编辑器";
            else
                form1.Text = "PBN编辑器 - " + name;
        }
        public void saveTemp()
        {
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    boardList[index].hand[i, j] = form1.TBArray[i, j].Text;
                }
        }
        public void newDeal()
        {
            int temp;
            if (boardList.Count == 0)
            {
                temp = 1;
            }
            else
            {
                saveTemp();
                temp = boardList.Last().num + 1;
            }
            string tempvul,tempdlr;
            GetDlrAndVul(temp,out tempdlr,out tempvul);
            Board b = new Board(temp,tempvul,tempdlr);
            boardList.Add(b);
            index = boardList.Count-1;
            Show();
        }
        public void ResetBoradNum(int startIndex)
        {
            string tempvul, tempdlr;
            foreach (Board b in boardList)
            {
                b.num = startIndex;
                GetDlrAndVul(startIndex, out tempdlr, out tempvul);
                b.dealer = tempdlr;
                b.Vul = tempvul;
                startIndex++;
            }
            Show();
        }
        public void DeleteDeal(int indexDel)
        {
            boardList.RemoveAt(indexDel);
            if (indexDel == boardList.Count) index = indexDel - 1;
            else
                index = indexDel;
            if (MessageBox.Show("是否重新对牌组编号？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ResetBoradNum(boardList[0].num);
            }
            Show();
        }
        public void EXG_EW(int index)
        {
            boardList[index].EXG_EW();
            Show();
        }
        public void EXG_NS(int index)
        {
            boardList[index].EXG_NS();
            Show();
        }
        public void Rotate(int index)
        {
            boardList[index].Rotate();
            Show();
        }
        public void RandomDeal(int index)
        {
            boardList[index].RandomDeal();
            Show();
        }
        public void Random_Rest(int index)
        {
            saveTemp();
            boardList[index].Random_Rest();
            Show();
        }
        public void ExChange_Suit(int index,int suit1,int suit2)
        {
            saveTemp();
            boardList[index].exchangeSuit(suit1,suit2);
            Show();
        }
        public void Shuffle_All_Deals()
        {
            saveTemp();
            Board b;
            for (int i = boardList.Count-1; i >= 0; i--)
            {
                int rnd = Global_Random.rd.Next(i);
                b = boardList[rnd];
                boardList[rnd] = boardList[i];
                boardList[i] = b;
                boardList[i].num = i + 1;
            }
            Show();
        }
        public void GetDlrAndVul(int num, out string dlr, out string vul)
        {
            switch (num % 16)
            {
                case 0: dlr = "W"; vul = "EW"; break;
                case 1: dlr = "N"; vul = "None"; break;
                case 2: dlr = "E"; vul = "NS"; break;
                case 3: dlr = "S"; vul = "EW"; break;
                case 4: dlr = "W"; vul = "All"; break;
                case 5: dlr = "N"; vul = "NS"; break;
                case 6: dlr = "E"; vul = "EW"; break;
                case 7: dlr = "S"; vul = "All"; break;
                case 8: dlr = "W"; vul = "None"; break;
                case 9: dlr = "N"; vul = "EW"; break;
                case 10: dlr = "E"; vul = "All"; break;
                case 11: dlr = "S"; vul = "None"; break;
                case 12: dlr = "W"; vul = "NS"; break;
                case 13: dlr = "N"; vul = "All"; break;
                case 14: dlr = "E"; vul = "None"; break;
                case 15: dlr = "S"; vul = "NS"; break;
                default: dlr = "N"; vul = "None"; break;
            }
        }
        
    }
}
