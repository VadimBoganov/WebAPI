using Newtonsoft.Json.Linq;
using System;
using WebAPI.Models;

namespace WebAPI.Converters
{
    public class EmployeeJsonConverter : JsonCreationConverter<Employee>
    {
        protected override Employee Create(Type objectType, JObject jObject)
        {
            if (jObject == null)
                throw new ArgumentNullException("Jobject can not be null...");

            if (jObject["discriminator"].ToString() == nameof(HourlyEmployee))
                return new HourlyEmployee();

            else if (jObject["discriminator"].ToString() == nameof(FixedEmployee))
                return new FixedEmployee();

            else
                throw new Exception("Implementation doesn't exist...");

        }
    }
}
