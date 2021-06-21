using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TcpTyping_CSharp_Client
{

    // a~z랜덤을 index랜덤으로 바꾸어야함 (리스트 인덱스 개수 생각해야함)
    // 서버에서 올 단어 리스트 만들어야함
    // 서버에서 올 속도 변수 만들어야함
    // 클라이언트 서버 생각해야함 
   

    public partial class Form1 : Form
    {
        private char c;

        //all_str 전체 문자수, correct_str 맞춘 개수
        private string str;
        private string current_str;
     
        private int x, y, all_int, correct_int;
        Random random = new Random();
        Graphics drawString;
        Brush b = new SolidBrush(Color.White);
        bool isStart;

        public Form1()
        {
            isStart = false;
            InitializeComponent();

            //문자를 출력시킬 picture박스를 개체로 잡았음
            drawString = pictureBox1.CreateGraphics();
            all_int = 0;
            correct_int = 0;
            str = "";     // 출제 문자
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            //타이머 뛸 때마다 다시 그리도록 작성한다
            Timer T = new Timer();
            T.Interval = 500;
            T.Tick += new EventHandler(Form1_Timer);
            T.Start();
        }

        private void Form1_Timer(object sender, System.EventArgs e)
        {
            if (isStart == true)
            {
                y += 30;
                if (y > 224)  // drawString 그래픽 범위를 벗어날 때
                {
                    next_char();
                }
                else
                {
                    //Rectangle rc = new Rectangle( x, y-30, 40, 20);  //삭제 경로 확인하고 싶을 때 쓰기

                    // rc는 문자를 가리는 역할
                    // drawString의 시작 위치에서 글자의 위치를 더함.
                    Rectangle rc = new Rectangle(x, y - 30, 40, 20);
                    Invalidate(rc);
                    Graphics drawString2 = pictureBox1.CreateGraphics();
                    drawString2.DrawString(str, Font, Brushes.Black, x, y);
                    drawString2.FillRectangle(b, rc);

                }
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            str = "" + current_str;

            // 아래는 맞춘 수와 총 글자수 출력시킴
            e.Graphics.DrawString("" + all_int, Font, Brushes.Black, 296, 53);
            e.Graphics.DrawString("" + correct_int, Font, Brushes.Black, 296, 68);
        }

        private void next_char()
        {
            all_int += 1;
            c = Convert.ToChar(random.Next('a', 'z' + 1));
            current_str = c.ToString();
            x = random.Next(0, 248);  // pictureBox의 가로 길이 - (글자길이 예상수)

            y = -30; // pictureBox에서 글자가 내릴 시작점
            Invalidate();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (isStart == true)
            {
                if (textBox2.Text.Equals(current_str) == true)
                {
                    correct_int += 1;
                }
                textBox2.Text = "";
                Rectangle rc = new Rectangle(x, y, 40, 20);
                Invalidate(rc);
                Graphics drawString2 = pictureBox1.CreateGraphics();
                drawString2.FillRectangle(b, rc);
                next_char();
            }

        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) // 엔터키를 눌렀을 때
            {
                label1.Text = "입력했군요";
                if (textBox2.Text.Equals(current_str)==true)
                {
                    correct_int += 1;
                }
                textBox2.Text = "";
                Rectangle rc = new Rectangle(x, y, 40, 20);
                Invalidate(rc);
                Graphics drawString2 = pictureBox1.CreateGraphics();
                drawString2.FillRectangle(b, rc);
                next_char();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            isStart = true;
            label1.Text = "   글자가 내려옵니다";
            textBox2.Text = "";
            next_char();
        }
    }
}

