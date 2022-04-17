using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Threading;

namespace PhilosofersForms
{
    public partial class Form1 : Form
    {

        private const int PHILOSOFER_COUNT = 5;
        private List<Philosofer> philosofers;


        public Form1()
        {
            InitializeComponent();

            philosofers = InitializePhilosofers();

        }

        private  void StatusChangedHadler(object sender, StatusChangedEventArgs e)
        {
            Color changeColor;
            if (e.State == State.Eating) changeColor = Color.Red;
            else changeColor = Color.Blue;

            switch (e.Index)
            {
                case 0:
                    {
                        label1.ForeColor = changeColor;
                    }
                    break;
                case 1:
                    {
                        label2.ForeColor = changeColor;
                    }
                    break;
                case 2:
                    {
                        label3.ForeColor = changeColor;
                    }
                    break;
                case 3:
                    {
                        label4.ForeColor = changeColor;
                    }
                    break;
                case 4:
                    {
                        label5.ForeColor = changeColor;
                    }
                    break;
            }
        }

        private  List<Philosofer> InitializePhilosofers()
        {
           
            List<Philosofer> philosofers = new List<Philosofer>(PHILOSOFER_COUNT);
            for (int i = 0; i < PHILOSOFER_COUNT; i++)
            {
                Philosofer philosofer = new Philosofer(philosofers, i);
                philosofer.StatusChanged += StatusChangedHadler;
                philosofers.Add(philosofer);
            }

            foreach (Philosofer philosofer in philosofers)
            {
                philosofer.LeftFork = philosofer.LeftPhilosofer.RightFork ?? new Fork();
                philosofer.RightFork = philosofer.RightPhilosofer.LeftFork ?? new Fork();

            }

            return philosofers;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<Thread> philosoferThreads = new List<Thread>();

            foreach (Philosofer philosofer in philosofers)
            {
                Thread philosoferThread = new Thread(new ThreadStart(philosofer.StartStateCycle));
                philosoferThreads.Add(philosoferThread);
                philosoferThread.Start();
            }

            foreach (Thread thread in philosoferThreads)
            {
                thread.Join();
            }

        }
    }
}
