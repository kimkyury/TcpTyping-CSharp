using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using Timer = System.Windows.Forms.Timer;

namespace TcpTyping_CSharp_Client
{

    /* Server가 가진 정보를 요청하는 게임 실행 유저, client파일임 */

    public partial class Form1 : Form
    {
        // 산성비에 필요한 변수
        private string str;
        private string current_str;
        private int x, y, all_int, correct_int;  // 좌표값들, 점수값들
        Random random = new Random(); //위치설정
        Graphics drawString; // 문자가 나타날 바탕
        Brush b = new SolidBrush(Color.White); // 문자를 가리기 위한 도구
        bool isStart; // 게임실행조건
        System.Windows.Forms.Timer T;


        // 통신관련 변수
        TcpClient client;
        Thread t1;
        int speed;       // server에서 넘어온 speed
        List<string> strList = new List<string>();  // server에서 온 제시어 여기다 붙일려고 했음.
        int listIndex; // strList의 index

        private void socketSendReceive()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 9000); // 다른 파일과 이곳 다름
            listener.Start();

            TcpClient serverClient = listener.AcceptTcpClient();
            if (serverClient.Connected)
            {
                NetworkStream ns = serverClient.GetStream();
                Encoding unicode = Encoding.Unicode;
                while (true)
                {
                    string c = "";
                    BinaryFormatter bf = new BinaryFormatter();
                    c = (string)bf.Deserialize(ns);
                    string[] array = c.Split(new char[] { ' ' });
                    set_speed(int.Parse(array[0]));
                    set_strList(array[1]);
                    Invalidate();
                }
            }
        }

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
            t1 = new Thread(new ThreadStart(socketSendReceive));
            //t1.IsBackground = true;
            t1.Start();
            //타이머 뛸 때마다 다시 그리도록 작성한다
            T = new Timer();
            T.Interval = 500;
            T.Tick += new EventHandler(Form1_Timer);
            T.Start();
        }


        /* 서버연결 */
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                client = new TcpClient("127.0.0.1", 8000);
                if (client.Connected)
                {
                    //t1 = new Thread(new ThreadStart(recive)); //데이터 받아오는 함수 실행
                    //t1.IsBackground = true;
                    //t1.Start();
                    label6.Text = "연결 성공";
                }
            }
            catch (Exception ex)
            {
                label6.Text = "소켓 미연결";
            }
       
            //기능판독을 위하여 임시로 만든 string 리스트을 초기화시킴
            //set_strList("초기단어");
            //set_strList("기본단어");
            //set_strList("어쩌구저쩌구");
        }

        /* 통신 정보 붙여넣기 위한 메소드 */
        private void set_strList(string str)
        {
            strList.Add(str);
            //stringList = new string[] { "임시문자", "출력중이야", "몇개일까", "일곱개", "떨어진다", "짠짠", "와르르륵르" };
        }
        private void set_speed(int speedbyte)
        {
            speed = speedbyte; 
        }

        /* 타이머에 따라 글자가 내려감 */
        private void Form1_Timer(object sender, System.EventArgs e)
        {
            if (isStart == true)
            {
                //y += 30;
                y += 30 + 5 * (speed - 1);
                if (y > 224)  // drawString 그래픽 범위를 벗어날 때
                {
                    next_char();
                }
                else
                {
                    //Rectangle rc = new Rectangle( x, y-30, 40, 20);  //삭제 경로 확인하고 싶을 때 쓰기

                    // rc는 문자를 가리는 역할
                    // drawString의 시작 위치에서 글자의 위치를 더함.
                    Rectangle rc = new Rectangle(x, y - (30 + 5 * (speed - 1)), 80, 20);
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
            e.Graphics.DrawString("" + all_int, Font, Brushes.Black, 296, 75);
            e.Graphics.DrawString("" + correct_int, Font, Brushes.Black, 296, 90);
        }

        /* 다음으로 보낼 문자 결정 메소드 */
        private void next_char()
        {
            all_int += 1;
            listIndex = random.Next(0, strList.Count);
            current_str = strList[listIndex];
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
                isStart = false;
                NetworkStream ns = client.GetStream();
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ns, "close");
                T.Stop();
                client.Close();
                t1.Abort();
                MessageBox.Show("5개를 맞췄어요!", "종료", MessageBoxButtons.OK);
                Application.Exit();
            }
        }

        /* 시작버튼 */
        private void button2_Click(object sender, EventArgs e)
        {
            isStart = true;
            label1.Text = "글자가 내려옵니다";
            label3.Text = speed.ToString();
            textBox2.Text = "";
            next_char();
        }
    }
}

