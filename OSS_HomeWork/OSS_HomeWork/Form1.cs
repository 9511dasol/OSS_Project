﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Json;
using System.IO;

namespace OSS_HomeWork
{
    public partial class Form1 : Form
    {
        int count = 0;
        List<int> list = new List<int>();
        Random random = new Random();
        public int[] lot = new int[6];
        
        public Form1()
        {
            InitializeComponent();
        }
        private bool IsNullString(string str)
        {
            return string.IsNullOrEmpty(str); 
        }
        private int IsInt(object obj)
        {
            if(obj == null) return 0;

            int check = 0;
            bool bch = int.TryParse(obj.ToString(), out check);

            if(!bch) return 0;

            return check;
        }
        private int getCurrentRound()
        {
            var nowDt = DateTime.Now;
            var firstround = new DateTime(2002, 12, 07); // 1회차

            TimeSpan ts = nowDt.Subtract(firstround);
            var diffDay = ts.Days;

            var current_round = diffDay / 7;
            return current_round;
        }
        private string GetLottoString(string strURI)
        {
            string strrspText = string.Empty;

            var request = (HttpWebRequest)WebRequest.Create(strURI);
            request.Method = "GET";

            request.Timeout = 20 * 1000; // 20초
            using(var response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream resStream = response.GetResponseStream();
                    using (var reader = new StreamReader(resStream))
                    {
                        strrspText = reader.ReadToEnd();
                    }
                }
                else
                {
                    strrspText = "";
                }
            }
            return strrspText;
        }
        private bool Ischeck()
        {
            if(IsNullString(textBox1.Text) || IsNullString(textBox2.Text) || IsNullString(textBox3.Text) ||
                IsNullString(textBox4.Text) || IsNullString(textBox5.Text) || IsNullString(textBox6.Text))
            {
                MessageBox.Show("빈 값일 수는 없습니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if(IsInt(textBox2.Text) == 0 || IsInt(textBox1.Text) == 0 || IsInt(textBox3.Text) == 0 ||
                IsInt(textBox5.Text) == 0 || IsInt(textBox6.Text) == 0 || IsInt(textBox4.Text) == 0 )
            {
                MessageBox.Show("숫자만 입력해주세요.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if(IsInt(textBox8.Text) == 0 || IsNullString(textBox8.Text))
            {
                MessageBox.Show("숫자만 입력하거나 빈 값일 수는 없습니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox8.Text = "";
                return false;
            }
            return true;
        }
        private bool IscheckForRound()
        {
            if (IsNullString(textBox8.Text))
            {
                MessageBox.Show("회차는 빈 값일 수는 없습니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox8.Text = "";
                return false;
            }

            if (IsInt(textBox8.Text) == 0)
            {
                MessageBox.Show("0이상의 정수만 입력하세요.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox8.Text = "";
                return false;
            }
            
            return true;
        }
        private void CR(int INum)
        {
            for(int a = 0; a < list.Count - 1; a++)
            {
                if(INum == list[a])
                {
                    count++;
                    return;
                }
            }
        }

        private void MessageResult(int bonus)
        {
            switch(count)
            {
                case 6:
                    {
                        GB.Text = "축하합니다 1등입니다.";
                        //GB.Text = ""; // 당첨 금액 각각 추가하기
                        break;
                    }
                case 5:
                    {
                        if(bonus == 1)
                        {
                            GB.Text = "축하합니다 2등입니다.";
                            //GB.Text = "";
                        }
                        else
                        {
                            GB.Text = "축하합니다 3등입니다.";
                            //GB.Text = "";
                        }
                        break;
                    }
                case 4:
                    {
                        GB.Text = "축하합니다 4등입니다.";
                        //GB.Text = "";
                        break;
                    }
                case 3:
                    {
                        GB.Text = "축하합니다 5등입니다.";
                        //GB.Text = "";
                        break;
                    }
                default:
                    {
                        GB.Text = "꽝 ㅠㅠ";
                        break;
                    }
            }
        }
        public static bool IsConnectedToInternet()
        {
            return System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
        }
        private void Confirm_Click(object sender, EventArgs e)
        {
            List<int> Lotto_Num = new List<int>();
            int bonus = 0;

            
            count = 0;

            if (!Ischeck())
            {
                return;
            }
            Lotto_Num.Add(Convert.ToInt32(textBox1.Text.Trim()));
            Lotto_Num.Add(Convert.ToInt32(textBox2.Text.Trim()));
            Lotto_Num.Add(Convert.ToInt32(textBox3.Text.Trim()));
            Lotto_Num.Add(Convert.ToInt32(textBox4.Text.Trim()));
            Lotto_Num.Add(Convert.ToInt32(textBox5.Text.Trim()));
            Lotto_Num.Add(Convert.ToInt32(textBox6.Text.Trim()));
            
            for(int y = 0; y < Lotto_Num.Count; y++)
            {
                if (Lotto_Num[y] > 45)
                {
                    MessageBox.Show("로또 번호는 45를 넘길 수 없습니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    GB.Text = "";
                    return;

                }
                for (int z =1 + y; z < Lotto_Num.Count; z++)
                {
                    if(Lotto_Num[y] == Lotto_Num[z])
                    {
                        MessageBox.Show("같은 번호가 들어갈 수 없습니다.", "Warning", MessageBoxButtons.OK ,MessageBoxIcon.Warning);
                        GB.Text = "";
                        return;
                    } // i 0 12345 j1234
                }
            }
            
            for(int i = 0; i < Lotto_Num.Count ; i++)
            {
                CR(Lotto_Num[i]);
                if (Lotto_Num[i] == list[list.Count-1])
                {
                    bonus = 1;
                }
            }
            //MessageBox.Show($"{bonus}");
            MessageResult(bonus);

        }

        private void round_Click(object sender, EventArgs e)
        {
            string strrv = GetLottoString("https://www.dhlottery.co.kr/common.do?method=getLottoNumber&drwNo=" + textBox8.Text);

            if(!IsConnectedToInternet())
            {
                MessageBox.Show("인터넷 연결 실패..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            if(!IscheckForRound())
            {
                return;
            }
            var parser = new JsonTextParser();
            JsonObject OB = parser.Parse(strrv);
            var OBC = (JsonObjectCollection)OB;
            
            if(Convert.ToInt32(textBox8.Text) > getCurrentRound() + 1)
            {
                MessageBox.Show($"최근 진행된 회차는 {getCurrentRound() + 1} 입니다." , "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            list.Clear();

            if (OBC["returnValue"].GetValue().ToString() == "success")
            {
                F1.Text = OBC["drwtNo1"].GetValue().ToString();
                F2.Text = OBC["drwtNo2"].GetValue().ToString();
                F3.Text = OBC["drwtNo3"].GetValue().ToString();
                F4.Text = OBC["drwtNo4"].GetValue().ToString();
                F5.Text = OBC["drwtNo5"].GetValue().ToString();
                F6.Text = OBC["drwtNo6"].GetValue().ToString();
                Bonus.Text = OBC["bnusNo"].GetValue().ToString();

                list.Add(Convert.ToInt32(OBC["drwtNo1"].GetValue().ToString().Trim()));
                list.Add(Convert.ToInt32(OBC["drwtNo2"].GetValue().ToString().Trim()));
                list.Add(Convert.ToInt32(OBC["drwtNo3"].GetValue().ToString().Trim()));
                list.Add(Convert.ToInt32(OBC["drwtNo4"].GetValue().ToString().Trim()));
                list.Add(Convert.ToInt32(OBC["drwtNo5"].GetValue().ToString().Trim()));
                list.Add(Convert.ToInt32(OBC["drwtNo6"].GetValue().ToString().Trim()));
                list.Add(Convert.ToInt32(OBC["bnusNo"].GetValue().ToString().Trim()));
            }



        }

        private void EXIT_Click(object sender, EventArgs e)
        {
            Close();
            
        }

        private void Make_Lotto_num_Click(object sender, EventArgs e)
        {
            int n = 0;
            for (int i = 0; i < 6; i++)
            {
                lot[i] = -1;
            }
            for (int i = 0; i < 6; i++)
            {
                while (true)
                {
                    n = random.Next(1, 46);
                    if (!lot.Contains(n))
                    {
                        lot[i] = n;
                        break;
                    }
                }
            }
            Array.Sort(lot);
            GB.Text = $"{lot[0]} {lot[1]} {lot[2]} {lot[3]} {lot[4]} {lot[5]}";
            return;
        }
    }
}
