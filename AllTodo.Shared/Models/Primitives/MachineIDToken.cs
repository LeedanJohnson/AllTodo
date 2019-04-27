using AllTodo.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllTodo.Shared.Models.Primitives
{
    public class MachineIDToken : DomainPrimitive<MachineIDToken>
    {
        private IDateTimeProvider datetime_provider;
        private string token;
        public string Token { get { return token; } }

        public MachineIDToken(IDateTimeProvider datetime_provider)
        {
            this.datetime_provider = datetime_provider;

            DateTime expiry = datetime_provider.Now.AddHours(24);

            byte[] time = BitConverter.GetBytes(expiry.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();
            this.token = Convert.ToBase64String(time.Concat(key).ToArray());
        }

        public MachineIDToken(string idtoken, IDateTimeProvider datetime_provider)
        {
            this.datetime_provider = datetime_provider;
            this.token = idtoken;
        }

        public bool IsExpired()
        {
            byte[] data = Convert.FromBase64String(token);
            DateTime expiration = DateTime.FromBinary(BitConverter.ToInt64(data, 0));
            return expiration < datetime_provider.Now;
        }

        public bool Matches(string candidate)
        {
            return candidate == token;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType().Equals(typeof(MachineIDToken)))
            {
                MachineIDToken other = (MachineIDToken)obj;
                if (other.Token.Equals(this.Token))
                    return true;
                return false;
            }
            return false;
        }
    }
}
