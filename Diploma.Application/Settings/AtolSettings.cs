namespace Diploma.Application.Settings;

public class AtolSettings
{
    public string Url { get; set; }
    public string GroupCode { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string Inn { get; set; }

    public string Address { get; set; }
    public string PaymentMethod { get; set; }
    public string TypeOfVat { get; set; }
    public int TaxRate { get; set; }
    public string Cashier { get; set; }
    public int TypeOfPayment { get; set; }
    public string Sno { get; set; }
    public string CallbackUrl { get; set; }


    public AtolTokenInfo AtolTokenInfo { get; set; }
}