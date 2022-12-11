using System;

namespace Operator.Data
{
    internal class glOperator : IEquatable<glOperator>
    {
        public int? Id { get; set; }

        public string OperatorName { get; set; }

        public string OperatorPatronymic { get; set; }

        public string OperatorSurname { get; set; }

        public string Password { get; set; }

        public string Ipaddress { get; set; }

        public int? Ipport { get; set; }

        #region IEquatable method
        public bool Equals(glOperator other)
        {
            if (this.Id == other.Id)
                return true;
            return false;
        }
        #endregion

        public override int GetHashCode()
        {
            if (Id != null)
                return Id.GetHashCode();
            else
                return -1;
        }
    }
 
    public class currOperator : IEquatable<currOperator>
    {
        public int Id { get; set; }
        
        public string OperatorSurname { get; set; }

        public string Password { get; set; }

        public string Ipaddress { get; set; }

        public int Ipport { get; set; }

        #region IEquatable method
        public bool Equals(currOperator other)
        {
            if (this.Id == other.Id)
                return true;
            return false;
        }
        #endregion

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
