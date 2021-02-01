namespace WebAPI.Models
{
    public class HourlyEmployee : Employee
    {
        public override double GetSalary() => 20.8 * 8 * Rate;
    }
}
