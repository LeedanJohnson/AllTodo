using AllTodo.Shared.Exceptions;
using AllTodo.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllTodo.Shared.Models.Primitives
{
    public class MachineIDTokenDTO
    {
        public string IDToken { get; set; }

        public MachineIDTokenDTO(string idtoken)
        {
            this.IDToken = idtoken;
        }

        public (bool success, string message) Validate()
        {
            return MachineIDToken.Validate(this.IDToken);
        }

        override
        public string ToString()
        {
            return this.IDToken;
        }

        public MachineIDToken GetObject(IDateTimeProvider datetime_provider)
        {
            return new MachineIDToken(this.IDToken, datetime_provider);
        }
    }

    public class MachineIDToken : DomainPrimitive<MachineIDToken>
    {
        private IDateTimeProvider datetime_provider;
        private string idtoken;
        public string Token { get { return idtoken; } }

        public static MachineIDToken Generate(IDateTimeProvider datetime_provider)
        {
            DateTime expiry = datetime_provider.Now.AddHours(24);

            byte[] time = BitConverter.GetBytes(expiry.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();
            string token = Convert.ToBase64String(time.Concat(key).ToArray());

            return new MachineIDToken(token, datetime_provider);
        }

        public MachineIDToken(string idtoken, IDateTimeProvider datetime_provider)
        {
            this.datetime_provider = datetime_provider;

            var validation_result = MachineIDToken.Validate(idtoken);
            if (!validation_result.success)
                throw new InvalidInitializationException(validation_result.message);
            this.idtoken = idtoken;
        }

        // TODO: Validate token
        public static (bool success, string message) Validate(string idtoken)
        {
            return (true, "Validation Successful");
        }

        public bool IsExpired()
        {
            byte[] data = Convert.FromBase64String(idtoken);
            DateTime expiration = DateTime.FromBinary(BitConverter.ToInt64(data, 0));
            return expiration < datetime_provider.Now;
        }

        public bool Matches(string candidate)
        {
            return candidate == idtoken;
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

        public MachineIDTokenDTO GetDTO()
        {
            return new MachineIDTokenDTO(this.idtoken);
        }
    }
}
