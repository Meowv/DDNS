namespace DDNS.ViewModel.Response
{
    public class ResponseViewModel<T>
    {
        public int Code { get; set; } = 0;

        public int Count { get; set; } = 0;

        public string Msg { get; set; } = "success";

        public T Data { get; set; }
    }
}