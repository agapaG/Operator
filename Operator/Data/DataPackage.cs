using System;

namespace Operator.Data
{
    public class DataPackage : IEquatable<DataPackage>
    {
        public string Tcentral { get; set; }
        public string Description { get; set; }
        public string Q { get; set; }
        public string Code { get; set; }
        public short Gru { get; set; }
        public short NumZ { get; set; }
        
        public string Time { get; set; }
        public int Rec { get; set; }
        public byte Line { get; set; }
        public string Tel { get; set; }

        public string NZone { get; set; }

        public short Color { get; set; }    


        #region IEquatable method
        public bool Equals(DataPackage other)
        {
            if (this.Rec == other.Rec)
                return true;
            return false;
        }
        #endregion

        public override int GetHashCode()
        {
            return Rec.GetHashCode();
        }
    }   
}
