using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
namespace TCP_chat

{
    public partial class Form1 : Form
    {
        private TcpClient client;
        public StreamReader STR;
        public StreamWriter STW;
        public string receive;
        public string TextToSend;
        public Form1()
        {
            InitializeComponent();
            IPAddress[] localIP = Dns.GetHostAddresses(Dns.GetHostName());

            foreach (IPAddress address in localIP)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    textBox1.Text = address.ToString();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TcpListener listener = new TcpListener(IPAddress.Any, int.Parse(textBox2.Text));
            listener.Start();
            client = listener.AcceptTcpClient();
            STR = new StreamReader(client.GetStream());
            STW = new StreamWriter(client.GetStream());
            STW.AutoFlush = true;
            backgroundWorker1.RunWorkerAsync();
            backgroundWorker2.WorkerSupportsCancellation = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            client = new TcpClient();
            IPEndPoint IPEnd = new IPEndPoint(IPAddress.Parse(textBox3.Text), int.Parse(textBox4.Text));
            try
            {
                textBox5.AppendText("Kết nối tới máy chủ" + "\n");
                STW = new StreamWriter(client.GetStream());
                STR = new StreamReader(client.GetStream());
                STW.AutoFlush = true;
                backgroundWorker1.RunWorkerAsync();
                backgroundWorker2.WorkerSupportsCancellation = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (client.Connected)
            {
                try
                {
                    receive = STR.ReadLine();
                    this.textBox5.Invoke(new MethodInvoker(delegate ()
                    {
                        textBox5.AppendText("Bạn:  " + receive + "\n");
                    }));
                    receive = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }


            }
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            if (client.Connected)
            {
                STW.WriteLine(TextToSend);
                this.textBox5.Invoke(new MethodInvoker(delegate ()
                {
                    textBox5.AppendText("Tôi: " + TextToSend + "\n");
                }));
            }
            else
            {
                MessageBox.Show("Gửi tin thất bại");

            }
            backgroundWorker2.CancelAsync();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(textBox6.Text!= "")
            {
                TextToSend=textBox6.Text;
                backgroundWorker2.RunWorkerAsync();
            }
            textBox6.Text = " ";
        }
    }
}