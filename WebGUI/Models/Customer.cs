namespace WebGUI.Models
{
    public class Customer
    {
        public int AccountNumber { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String PinNumber { get; set; }
        public double Balance { get; set; }
        public byte[] ProfilePicture { get; set; }
        public string ProfileBase64 { get; set; }
    }
}
