﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

#pragma warning disable CA1416 // Validate platform compatibility

namespace FftSharp.Demo
{
    public partial class FormWindowInspector : Form
    {
        public FormWindowInspector()
        {
            InitializeComponent();
            lbWindows.Items.AddRange(Window.GetWindows());
            lbWindows.SelectedIndex = Window.GetWindows().ToList().FindIndex(x => x.Name == "Hanning");
        }

        private void lbWindows_SelectedIndexChanged(object sender, EventArgs e)
        {
            IWindow window = (IWindow)lbWindows.SelectedItem;
            if (window is null)
                return;

            rtbDescription.Text = window.Description;
            UpdateTimePlot(window);
            UpdateFrequencyPlot(window);
        }

        private void UpdateTimePlot(IWindow window)
        {
            double[] xs = ScottPlot.Generate.Consecutive(101);
            double[] ys = window.Create(xs.Length);

            plotWindow.Plot.Clear();
            var sp = plotWindow.Plot.Add.ScatterLine(xs, ys);
            sp.LineWidth = 2;
            plotWindow.Plot.YLabel("Amplitude");
            plotWindow.Plot.XLabel("Samples");
            plotWindow.Refresh();
        }

        private void UpdateFrequencyPlot(IWindow window)
        {
            int fftSize = (int)Math.Pow(2, 14);
            double[] xs = ScottPlot.Generate.Consecutive(fftSize);
            double[] ys = xs.Select(x => Math.Sin(x / fftSize * Math.PI * fftSize / 2)).ToArray();
            double[] windowed = window.Apply(ys);
            System.Numerics.Complex[] spectrum = FftSharp.FFT.Forward(windowed);
            double[] power = FFT.Power(spectrum);

            // hide DC component
            power[0] = power[1];

            plotFreq.Plot.Clear();
            var sig = plotFreq.Plot.Add.Signal(power, 1.0 / (fftSize / 2));
            sig.Data.XOffset = -.5;
            plotFreq.Plot.YLabel("Power (dB)");
            plotFreq.Plot.XLabel("Frequency (cycles/sample)");
            plotFreq.Refresh();
        }
    }
}
