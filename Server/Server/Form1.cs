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


namespace Server
{
    public partial class Form1 : Form
    {
        //TcpListener listener;
        TcpClient client;
        Thread t1;

        private void socketSendReceive()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 8000); // listener 클래스
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

                    if(c.Equals("close"))
                    {
                        listBox1.Items.Clear();
                        comboBox1.SelectedIndex = 0;
                        label2.Text = "미연결";
                        t1.Abort();
                    }
                }
            }
        }

        //ArrayList ar;

        public Form1()
        {
            InitializeComponent();
            //ar = new ArrayList();
            comboBox1.SelectedIndex = 0;
            t1 = new Thread(new ThreadStart(socketSendReceive)); // 스레드
            t1.Start(); // 스레드 시작
        }

        private void button1_Click(object sender, EventArgs e)
        {
            client = new TcpClient("127.0.0.1", 9000);
            if (client.Connected)
            {
                label2.Text = "연결 성공";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add(textBox1.Text);
            //ar.Add(textBox1.Text);
            if (client.Connected)
            {
                NetworkStream ns = client.GetStream();
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ns, comboBox1.Text + " " + textBox1.Text);
            }
            textBox1.Text = "";
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) button2_Click(sender, e);
        }
    }
}
