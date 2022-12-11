using System;
using System.Collections.Generic;

using Operator.Data;
using Operator.ViewModels.Base;

namespace Operator.Models
{
    public class DrawDown : BaseViewModel, IEquatable<DrawDown>
    {
        public string Tcentral { get; set; }
        public string Description { get; set; }
        public string Time { get; set; }

        private short color;
        public short Color { get => color; set => Set(ref color, value); }

        private short status;
        public short Status { get => status; set => Set(ref status, value); }

        public ICollection<DataPackage> dataPackages { get; set; }

        #region IEquatable method
        public bool Equals(DrawDown other)
        {
            if (this.Tcentral == other.Tcentral)
                return true;
            return false;
        }
        #endregion

        public override int GetHashCode()
        {
            if (Tcentral != null)
                return Tcentral.GetHashCode();
            else
                return -1;
        }

        public DrawDown()
        {
            dataPackages = new List<DataPackage>();
        }

    }

    public class ZonObjZak
    {
        public int Id { get; set; }
        public string Gry { get; set; }
        public short Zone { get; set; }
        public string NZone { get; set; }
        public string Reserve { get; set; }
        public string Object { get; set; }
        public string Tcentral { get; set; }
        public string Status03 { get; set; }
        public string Inzen { get; set; }
        public string Status04 { get; set; }
        public string Otv { get; set; }
        public string Tda { get; set; }
        public string Ot { get; set; }
        public string Adr { get; set; }
        public string Fn { get; set; }
        public string Ps { get; set; }
        public string Psn07 { get; set; }
        public string Model { get; set; }
    }

}
