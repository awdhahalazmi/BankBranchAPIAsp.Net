namespace BankBranchAPI.Models
{
    public class AddBankRequest
    {
        public string Name {get; set;}
        public string Location { get; set; }
        public string BankManager { get; set; }


    }
    public class BankBranchResponse
    {
        public string name { get; set; }

        public string location { get; set; }

        public string branchManager { get; set; }
    }
}

