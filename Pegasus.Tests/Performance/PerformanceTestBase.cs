﻿// -----------------------------------------------------------------------
// <copyright file="PerformanceTestBase.cs" company="(none)">
//   Copyright © 2014 John Gietzen.  All Rights Reserved.
//   This source is subject to the MIT license.
//   Please see license.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Pegasus.Tests.Performance
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using NUnit.Framework;

    [TestFixture]
    [Category("Performance")]
    public abstract class PerformanceTestBase
    {
        protected readonly TestCaseData[] Methods;

        private static double[] tDistribution = new[] {
            0.00, 0.00, 12.71, 4.30, 3.18, 2.78, 2.57, 2.45, 2.36, 2.31,
            2.26, 2.23, 2.20, 2.18, 2.16, 2.14, 2.13, 2.12, 2.11, 2.10,
            2.09, 2.09, 2.08, 2.07, 2.07, 2.06, 2.06, 2.06, 2.05, 2.05,
            2.05, 2.04, 2.04, 2.04, 2.03, 2.03, 2.03, 2.03, 2.03, 2.02,
            2.02, 2.02, 2.02, 2.02, 2.02, 2.02, 2.01, 2.01, 2.01, 2.01,
            2.01, 2.01, 2.01, 2.01, 2.01, 2.00, 2.00, 2.00, 2.00, 2.00,
            2.00, 2.00, 2.00, 2.00, 2.00, 2.00, 2.00, 2.00, 2.00, 2.00,
            1.99, 1.99, 1.99, 1.99, 1.99, 1.99, 1.99, 1.99, 1.99, 1.99,
            1.99, 1.99, 1.99, 1.99, 1.99, 1.99, 1.99, 1.99, 1.99, 1.99,
            1.99, 1.99, 1.99, 1.99, 1.99, 1.99, 1.99, 1.98, 1.98, 1.98,
            1.98, 1.98, 1.98, 1.98, 1.98, 1.98, 1.98, 1.98, 1.98, 1.98,
            1.98, 1.98, 1.98, 1.98, 1.98, 1.98, 1.98, 1.98, 1.98, 1.98,
            1.98, 1.98, 1.98, 1.98, 1.98, 1.98, 1.98, 1.98, 1.98, 1.98,
            1.98, 1.98, 1.98, 1.98, 1.98, 1.98, 1.98, 1.98, 1.98, 1.98,
            1.98, 1.98, 1.98, 1.98, 1.98, 1.98, 1.98, 1.98, 1.98, 1.98,
            1.98, 1.98, 1.98, 1.98, 1.98, 1.98, 1.98, 1.98, 1.98, 1.98,
            1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97,
            1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97,
            1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97,
            1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97,
            1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97,
            1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97,
            1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97,
            1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97,
            1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97,
            1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97,
            1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97,
            1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97,
            1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97,
            1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97,
            1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97,
            1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97,
            1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97,
            1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97,
            1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97,
            1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97,
            1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97,
            1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97,
            1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97,
            1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97,
            1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97,
            1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97,
            1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97,
            1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97,
            1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97,
            1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97,
            1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97, 1.97,
            1.97, 1.97, 1.97, 1.97, 1.96
        };

        public PerformanceTestBase()
        {
            this.Methods = (from m in this.GetType().GetMethods()
                            from e in m.GetCustomAttributes(typeof(EvaluateAttribute), inherit: true).Cast<EvaluateAttribute>()
                            select new TestCaseData(m)).ToArray();
        }

        [TestCaseSource("Methods")]
        public void Evaluate(MethodInfo method)
        {
            var action = MakeAction(this, method);

            var measure = new Func<int, RunningStat>(samples =>
            {
                var runningStat = new RunningStat();
                var sw = new Stopwatch();

                while (samples-- > 0)
                {
                    sw.Restart();
                    action();
                    runningStat.Push(sw.Elapsed.TotalMilliseconds);
                }

                return runningStat;
            });

            var initialTime = measure(1);
            Trace.WriteLine(FormatTime("initialTime", initialTime));
            var baseTime = measure(1);
            Trace.WriteLine(FormatTime("baseTime", baseTime));

            var warmupSamples = (int)Math.Max(1, TimeSpan.FromSeconds(0.1).TotalMilliseconds / baseTime.Mean);
            Trace.WriteLine(string.Format("warmupSamples: {0}", warmupSamples));
            var warmupTime = measure(warmupSamples);
            Trace.WriteLine(FormatTime("warmupTime", warmupTime));

            var testSamples = (int)Math.Max(30, TimeSpan.FromSeconds(1).TotalMilliseconds / warmupTime.Mean);
            Trace.WriteLine(string.Format("testSamples: {0}", testSamples));
            var testTime = measure(testSamples);
            Trace.WriteLine(FormatTime("testTime", testTime));
        }

        private static string FormatTime(string name, RunningStat time)
        {
            Func<double, TimeSpan> convert = ms => TimeSpan.FromTicks((long)Math.Round(TimeSpan.TicksPerMillisecond * ms));
            return time.Count > 1
                ? string.Format("{0}: {1}±{2}", name, convert(time.Mean), convert(tDistribution[Math.Min(time.Count, tDistribution.Length - 1)] * time.StandardDeviation))
                : string.Format("{0}: {1}", name, convert(time.Mean));
        }

        private static Action MakeAction(object fixture, MethodInfo method)
        {
            return (Action)Expression.Lambda(Expression.Call(Expression.Constant(fixture), method)).Compile();
        }

        [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
        protected class EvaluateAttribute : Attribute
        {
        }

        private class RunningStat
        {
            private int n = 0;
            private double oldM, newM, oldS, newS;

            public int Count
            {
                get { return this.n; }
            }

            public double Mean
            {
                get { return this.n > 0 ? this.newM : 0.0; }
            }

            public double StandardDeviation
            {
                get { return Math.Sqrt(this.Variance); }
            }

            public double Variance
            {
                get { return this.n > 1 ? this.newS / (this.n - 1) : 0.0; }
            }

            public void Clear()
            {
                this.n = 0;
            }

            public void Push(double value)
            {
                this.n++;

                // See Knuth TAOCP vol 2, 3rd edition, page 232
                if (this.n == 1)
                {
                    this.oldM = this.newM = value;
                    this.oldS = 0.0;
                }
                else
                {
                    this.newM = this.oldM + (value - this.oldM) / this.n;
                    this.newS = this.oldS + (value - this.oldM) * (value - this.newM);

                    // set up for next iteration
                    this.oldM = this.newM;
                    this.oldS = this.newS;
                }
            }
        }
    }
}