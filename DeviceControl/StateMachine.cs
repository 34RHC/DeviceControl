using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceControl
{
    class StateMachine
    {
        public enum States
        {
            Stopped,
            Agitating,
            Heating,
            AgitatingAndHeating,
            Regulating
        }
        private States _currentState;
        public States CurrentState
        {
            get { return _currentState; }
            private set
            {
                _currentState = value;
            }
        }

        public StateMachine()
        {
            _currentState = States.Stopped;
        }

        public event EventHandler StateChanged;

        public void ChangeState(States newState)
        {
            CurrentState = newState;
            StateChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
