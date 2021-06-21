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

    // 서버에서 올 단어를 담을 리스트 만들어야함
    // 서버에서 올 속도를 담을 변수 만들어야함
    // 클라이언트 서버 생각해야함 
   

    public partial class Form1 : Form
    {
        private string str;
        private string current_str;
     
        //all_str 전체 문자수, correct_str 맞춘 개수
        private int x, y, all_int, correct_int;
        Random random = new Random();
        Graphics drawString; // 문자가 나타날 바탕
        Brush b = new SolidBrush(Color.White); // 문자를 가리기 위한 도구
        bool isStart; //게임시작





        // 통신을 통해 받아올 데이터
        // 점찍기처럼 class로 담아올까? 암튼암튼
        string[] stringList;
        int listIndex;
        
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


        /* 연결버튼 */
        private void button1_Click(object sender, EventArgs e)
        {
            label6.Text = "연결 성공";
            set_stringList(); //일단 임시로 만든 string배열을 초기화시킴
        }

        /* 연결 성공시, 리스트의 내용을 받아옴 */
        private void set_stringList()
        {
            stringList = new string[] { "임시문자", "출력중이야", "몇개일까", "일곱개", "떨어진다", "짠짠", "와르르륵르" };
        }



        /* 타이머에 따라 글자가 내려감 */
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
                    Rectangle rc = new Rectangle(x, y - 30, 80, 20);
                    Invalidate(rc);
                    Graphics drawString2 = pictureBox1.CreateGraphics();
                    drawString2.DrawString(str, Font, Brushes.Black, x, y);
                    drawString2.FillRectangle(b, rc);

                }
            }
        }


        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            str = "" + current_str;

            // 아래는 맞춘 수와 총 글자수 출력시킴
            e.Graphics.DrawString("" + all_int, Font, Brushes.Black, 296, 53);
            e.Graphics.DrawString("" + correct_int, Font, Brushes.Black, 296, 68);
        }

        /* 다음으로 보낼 문자 결정 메소드 */
        private void next_char()
        {
            all_int += 1;
            listIndex = random.Next(0, stringList.Length);
            current_str = stringList[listIndex];
            x = random.Next(0, 218);  // pictureBox의 가로 길이 - (5글자 이내의 크기)

            y = -30; // pictureBox에서 글자가 내릴 시작점
            Invalidate();
        }

        /* 입력란 옆 버튼클릭, 엔터 키보드와 내용 동일 */
        private void button3_Click(object sender, EventArgs e)
        {
            if (isStart == true)
            {
                if (textBox2.Text.Equals(current_str) == true)
                {
                    cnt_correct();
                }
                textBox2.Text = "";
                Rectangle rc = new Rectangle(x, y, 80, 20);
                Invalidate(rc);
                Graphics drawString2 = pictureBox1.CreateGraphics();
                drawString2.FillRectangle(b, rc);
                next_char();
            }
        }

        /* 입력란에서 엔터키보드 이벤트 */
        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) // 엔터키를 눌렀을 때
            {
                label1.Text = "5개 맞추면 종료";
                if (textBox2.Text.Equals(current_str)==true)
                {
                    cnt_correct();
                }
                textBox2.Text = "";
                Rectangle rc = new Rectangle(x, y, 80, 20);
                Invalidate(rc);
                Graphics drawString2 = pictureBox1.CreateGraphics();
                drawString2.FillRectangle(b, rc);
                next_char();
            }
        }

        /* 맞춘숫자 세다가 종료팝업, 종료 결정을 함 */
        private void cnt_correct()
        {
            correct_int++;
            if(correct_int >= 5)
            {
                MessageBox.Show("10개를 맞췄어요!", "종료", MessageBoxButtons.OK);
                Application.Exit();
            }
        }



        /* 시작버튼 */
        private void button2_Click(object sender, EventArgs e)
        {
            isStart = true;
            label1.Text = "   글자가 내려옵니다";
            textBox2.Text = "";
            next_char();
        }




        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
        }
        private void label4_Click(object sender, EventArgs e)
        {
        }
    }
}

