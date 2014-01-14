using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.CodeRush.Core;
using DevExpress.CodeRush.PlugInCore;
using DevExpress.CodeRush.StructuralParser;

namespace CR_TemplateExpandWithCollapsedRegions
{
    public class DelayedRunner
    {
        private Timer _timer = new Timer();
        private System.Func<bool> _func;
        private int _maxTicks;
        public void RunFuncUntilSuccess(Func<bool> Func, int Delay, int MaxDelay)
        {
            _maxTicks = Environment.TickCount + MaxDelay;
            _func = Func;
            _timer.Interval = Delay;
            _timer.Tick += Tick;
            _timer.Start();

        }
        private void Tick(object sender, EventArgs e)
        {
            if (_func.Invoke() || Environment.TickCount >= _maxTicks)
            {
                _timer.Stop();
                _timer.Tick -= Tick;
            }
        }
    }
}
