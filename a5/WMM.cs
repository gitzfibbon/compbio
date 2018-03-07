using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a5
{
    public class WMM
    {
        // Every WMM: row 1 is A, 2 is C, 3 is G, 4 is T

        public double[,] WMM0 { get; set; }
        public double[,] WMM1 { get; set; }
        public double[,] WMM2a { get; set; }
        public double[,] WMM2b { get; set; }

        public WMM()
        {
            this.InitializeWMM();
        }

        private void InitializeWMM()
        {
            this.InitializeWMM0();
            this.InitializeWMM1();
            this.InitializeWMM2a();
            this.InitializeWMM2b();
        }

        private void InitializeWMM0()
        {
            // 100% consensus

            this.WMM0 = new double[4, 6];

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    this.WMM0[i, j] = 0;
                }
            }

            this.WMM0[0, 0] = 1;
            this.WMM0[0, 1] = 1;
            this.WMM0[3, 2] = 1;
            this.WMM0[0, 3] = 1;
            this.WMM0[0, 4] = 1;
            this.WMM0[0, 5] = 1;
        }

        private void InitializeWMM1()
        {
            // 85% consensus

            this.WMM1 = new double[4, 6];

            this.WMM1[0, 0] = 0.85;
            this.WMM1[1, 0] = 0.05;
            this.WMM1[2, 0] = 0.05;
            this.WMM1[3, 0] = 0.05;

            this.WMM1[0, 1] = 0.85;
            this.WMM1[1, 1] = 0.05;
            this.WMM1[2, 1] = 0.05;
            this.WMM1[3, 1] = 0.05;

            this.WMM1[0, 2] = 0.05;
            this.WMM1[1, 2] = 0.05;
            this.WMM1[2, 2] = 0.05;
            this.WMM1[3, 2] = 0.85;

            this.WMM1[0, 3] = 0.85;
            this.WMM1[1, 3] = 0.05;
            this.WMM1[2, 3] = 0.05;
            this.WMM1[3, 3] = 0.05;

            this.WMM1[0, 4] = 0.85;
            this.WMM1[1, 4] = 0.05;
            this.WMM1[2, 4] = 0.05;
            this.WMM1[3, 4] = 0.05;

            this.WMM1[0, 5] = 0.85;
            this.WMM1[1, 5] = 0.05;
            this.WMM1[2, 5] = 0.05;
            this.WMM1[3, 5] = 0.05;
        }

        private void InitializeWMM2a()
        {
            this.WMM2a = new double[4, 6];
        }

        private void InitializeWMM2b()
        {
            this.WMM2b = new double[4, 6];
        }


    }
}
