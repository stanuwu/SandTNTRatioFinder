using System;
using System.Collections.Generic;
using System.Text;

namespace SandTNTRatioFinderPublic
{
    class SandTNTSyncRatio
    {
        public SandTNTSyncRatio(int sandGT, int tNTGT, string sandGuider, int sandGuiderY, string tNTGuider, int tNTGuiderY, int droppedGT, double difference, double sandEyePos, double tNTExpPos, double sandPos, double tNTPos, double sandYVel, double tNTYVel)
        {
            SandGT = sandGT;
            TNTGT = tNTGT;
            SandGuider = sandGuider;
            SandGuiderY = sandGuiderY;
            TNTGuider = tNTGuider;
            TNTGuiderY = tNTGuiderY;
            DroppedGT = droppedGT;
            Difference = difference;
            SandEyePos = sandEyePos;
            TNTExpPos = tNTExpPos;
            SandPos = sandPos;
            TNTPos = tNTPos;
            SandYVel = sandYVel;
            TNTYVel = tNTYVel;
        }

        public int SandGT { get; set; }
        public int TNTGT { get; set; }
        public string SandGuider { get; set; }
        public int SandGuiderY { get; set; }
        public string TNTGuider { get; set; }
        public int TNTGuiderY { get; set; }
        public int DroppedGT { get; set; }
        public double Difference { get; set; }
        public double SandEyePos { get; set; }
        public double TNTExpPos { get; set; }
        public double SandPos { get; set; }
        public double TNTPos { get; set; }
        public double SandYVel { get; set; }
        public double TNTYVel { get; set; }

        public override string ToString()
        {
            string sout = "Ratio <x>:\n" +
                $"Sand Boost GT: {this.SandGT} | TNT Boost GT: {this.TNTGT}\n" +
                $"Sand Guider: {this.SandGuider} | TNT Guider: {this.TNTGuider}\n" +
                $"Sand Guider Y: {this.SandGuiderY} | TNT Guider Y: {TNTGuiderY}\n" +
                $"Gameticks Dropped: {this.DroppedGT} | Effective Difference: {this.Difference}\n" +
                $"Sand Pos: {this.SandEyePos} ({this.SandPos}) | TNT Pos: {this.TNTExpPos} ({this.TNTPos})\n" +
                $"Sand Y Vel: {this.SandYVel} | TNT Y Vel: {this.TNTYVel}";
            return sout;
        }
    }
}