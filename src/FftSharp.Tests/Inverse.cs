﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Numerics;
using System.Text;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FftSharp.Tests;

class Inverse
{
    [Test]
    public void Test_IDFT_MatchesOriginal()
    {
        Random rand = new Random(0);
        System.Numerics.Complex[] original = new System.Numerics.Complex[1024];
        for (int i = 0; i < original.Length; i++)
            original[i] = new System.Numerics.Complex(rand.NextDouble() - .5, rand.NextDouble() - .5);

        System.Numerics.Complex[] fft = FftSharp.DFT.Forward(original);
        System.Numerics.Complex[] ifft = FftSharp.DFT.Inverse(fft);

        for (int i = 0; i < ifft.Length; i++)
        {
            Assert.AreEqual(original[i].Real, ifft[i].Real, 1e-6);
            Assert.AreEqual(original[i].Imaginary, ifft[i].Imaginary, 1e-6);
        }
    }

    [Test]
    public void Test_IFFT_MatchesOriginal()
    {
        Random rand = new Random(0);
        Complex[] original = new Complex[1024];
        for (int i = 0; i < original.Length; i++)
            original[i] = new Complex(rand.NextDouble() - .5, rand.NextDouble() - .5);

        Complex[] fft = new Complex[original.Length];
        Array.Copy(original, 0, fft, 0, original.Length);
        FftSharp.FFT.Forward(fft);

        Complex[] ifft = new Complex[fft.Length];
        Array.Copy(fft, 0, ifft, 0, fft.Length);
        FftSharp.FFT.Inverse(ifft);

        for (int i = 0; i < ifft.Length; i++)
        {
            Assert.AreEqual(original[i].Real, ifft[i].Real, 1e-6);
            Assert.AreEqual(original[i].Imaginary, ifft[i].Imaginary, 1e-6);
        }
    }

    [Test]
    public void Test_InverseReal_MatchesOriginal()
    {
        Random rand = new Random(0);
        double[] original = new double[1024];
        for (int i = 0; i < original.Length; i++)
            original[i] = rand.NextDouble() - .5;

        System.Numerics.Complex[] rfft = FftSharp.FFT.ForwardReal(original);
        double[] irfft = FftSharp.FFT.InverseReal(rfft);

        Assert.AreEqual(original.Length, irfft.Length);

        for (int i = 0; i < irfft.Length; i++)
        {
            Assert.AreEqual(original[i], irfft[i], 1e-10);
        }
    }
}
