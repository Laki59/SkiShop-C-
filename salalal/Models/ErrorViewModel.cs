namespace salalal.Models
{
    public class ErrorViewModel
    {
        //Ovo svojstvo ?uva ID trenutnog zahteva,da se vidi koji je ID greske
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
