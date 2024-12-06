namespace PaymentService.Entities
{
    public class PaymentUser
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal Balance { get; set; }
    }
}
