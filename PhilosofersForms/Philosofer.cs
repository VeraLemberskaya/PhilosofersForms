using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Threading;

namespace PhilosofersForms
{

    class StatusChangedEventArgs
    {
        public int Index { get; private set; }
        public State State { get; private set; }

        public StatusChangedEventArgs(int index, State state)
        {
            this.Index = index;
            this.State = state;
        }
    }
    enum State
    {
        Thinking,
        Eating
    }

    delegate void StatusChangedEventHandler(Object sender, StatusChangedEventArgs args);

    internal class Philosofer
    {
       
        private readonly List<Philosofer> _allPhilosofers;
        private readonly int _index;
        public event StatusChangedEventHandler StatusChanged;

        public Philosofer(List<Philosofer> allPhilosofers, int index)
        {
            this._allPhilosofers = allPhilosofers;
            this._index = index;
            this.Name = string.Format("Philosofer {0}", _index);
            this.State = State.Thinking;
        }

        public string Name { get; private set; }
        public State State { get; private set; }
        public Fork LeftFork { get; set; }
        public Fork RightFork { get; set; }

        public Philosofer LeftPhilosofer
        {
            get
            {
                if (_index == 0)
                {
                    return _allPhilosofers[_allPhilosofers.Count - 1];
                }
                return _allPhilosofers[_index - 1];
            }
        }

        public Philosofer RightPhilosofer
        {
            get
            {
                if (_index == _allPhilosofers.Count - 1)
                {
                    return _allPhilosofers[0];
                }
                return _allPhilosofers[_index + 1];
            }
        }

        public void StartStateCycle()
        {
            while (true)
            {
                this.Think();
                if (this.PickUp())
                {
                    this.Eat();
                    this.PutDownLeft();
                    this.PutDownRight();
                }
            }
        }

        private bool PickUp()
        {
            if (Monitor.TryEnter(this.LeftFork))
            {
                if (Monitor.TryEnter(this.RightFork))
                {
                    return true;
                }
                else
                {
                    Random random = new Random();
                    Thread.Sleep(random.Next(100, 400));
                    if (Monitor.TryEnter(this.RightFork))
                    {
                        return true;
                    }
                    else
                    {
                        this.PutDownLeft();
                    }
                }
            }
            return false;
        }

        private void PutDownLeft()
        {
            Monitor.Exit(this.LeftFork);
        }

        private void PutDownRight()
        {
            Monitor.Exit(this.RightFork);
        }

        private void Eat()
        {
            Random random = new Random();
            this.State = State.Eating;
            RaiseStatusChangedEvent();
            Thread.Sleep(random.Next(1000,4000));
        }

        private void Think()
        {
            Random random = new Random();
            this.State = State.Thinking;
            RaiseStatusChangedEvent();
            Thread.Sleep(random.Next(1000,4000));
        }

        private void RaiseStatusChangedEvent()
        {
            StatusChanged?.Invoke(this, new StatusChangedEventArgs(_index, State));
        }

    }
}
