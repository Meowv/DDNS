namespace DDNS.ViewModel.Response
{
    public class ResponseViewModel<T>
    {
        public int Code { get; set; }

        public string Msg { get; set; }

        public T Data { get; set; }
    }
}